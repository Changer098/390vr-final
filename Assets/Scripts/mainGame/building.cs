using UnityEngine;
using System.Collections;

public class building : MonoBehaviour, destructable {

    public GameObject DM_prefab;
    private GameObject destructMesh;
    public destroyCall destroyScript;
    int citizenCount;
    int rightHitDestructionAmount = 5;
    int weapon1HitDestructionAmount = 7;
    int weapon2HitDestructionAmount = 25;
    int destructionAmount = 100;                   //How many points we gain for destroying


    int rightBulletHitCount = 0;
    int leftBulletHitCount = 0;
    int weapon1HitCount = 0;
    int weapon2HitCount = 0;
    int weapon3HitCount = 0;
    int weapon4HitCount = 0;
    int destructionBit;                             //How many points we gain for hit
    public Vector3 scale;
    public Vector3 newPosition;
    public bool usePosition;
    bool debug = false;
    bool DontGenerate = false;
    public bool resetPosition = false;
    public fountainQuit fountainQuitScript;
    bool isDestroyed = false;
	// Use this for initialization
	void Start () {
        citizenCount = Random.Range(0, 200);
        if (scale.x == 0) {
            scale = new Vector3(1, 1, 1);
        }
        //assign random values to fields
        destructionAmount = Random.Range(100, 500);
        rightHitDestructionAmount = Random.Range(5, 12);
        weapon1HitDestructionAmount = Random.Range(5, 15);
        weapon2HitDestructionAmount = Random.Range(25, 75);


        destructionBit = (int)(0.1f * destructionAmount); 
        if (!DontGenerate) StartCoroutine(generateColliders());
    }
    void Update() {
        if (debug && resetPosition && destructMesh != null) {
            resetPosition = false;
            destructMesh.transform.position = gameObject.transform.position;
            Debug.Log("X: " + destructMesh.transform.localScale.x + ", Y: " + destructMesh.transform.localScale.y + ", Z: " + destructMesh.transform.localScale.z);
        }
    }

    IEnumerator generateColliders() {
        Vector3 rotation = gameObject.transform.rotation.eulerAngles;
        rotation.x = 0;
        destructMesh = (GameObject)Instantiate(DM_prefab, gameObject.transform.position, QuanternionHelper.Euler(rotation.x, rotation.y, rotation.z));
        destructMesh.transform.localScale = scale;
        if (usePosition) {
            destructMesh.transform.position = newPosition;
        }
        else {
            destructMesh.transform.position = gameObject.transform.position;
        }
        Material[] materials = gameObject.GetComponent<Renderer>().materials;
        if (debug) {
            //Debug mode, so don't generate colliders or anything of the sort
            yield break;
        }
        destructMesh.AddComponent<buildDestruct>();

        int childCount = destructMesh.transform.childCount;
        for (int i = 0; i < childCount; i++) {
            Transform childT = destructMesh.transform.GetChild(i);
            if (childT.name == "Plane" || childT.name == "Circle") {
                childT.gameObject.SetActive(false);
                Debug.Log("Skipping on: " + gameObject.name);
                continue;
            }
            else {
                childT.GetComponent<Renderer>().materials = materials;
                MeshCollider collider = childT.gameObject.AddComponent<MeshCollider>();
                Rigidbody rigid = childT.gameObject.AddComponent<Rigidbody>();
                collider.convex = true;
                rigid.drag = 1;
                rigid.isKinematic = true;   //set to false, when actually do simulations
                //childT.gameObject.SetActive(false);
            }

            yield return null;
        }
        destructMesh.SetActive(false);
        for (int i = 0; i < childCount; i++) {
            Transform childT = destructMesh.transform.GetChild(i);
            if (childT.name == "Plane") {
                continue;
            }
            Rigidbody rigid = childT.GetComponent<Rigidbody>();
            rigid.isKinematic = false;
        }
    }

    void OnCollisionEnter(Collision col) {
        Debug.Log("Colliding!");
        if (col.rigidbody.gameObject.name == gameObject.name) {
            //colliding with ourselves, ignore
            Debug.Log("Colliding with ourselves");
        }
        else {
            //Colliding on impactable
            if (col.gameObject.layer == 11) {
                //Debug.Log(gameObject.name + " collided with " + col.gameObject.name);
                bullet bulletScript = col.gameObject.GetComponent<bullet>();
                int weaponType = -2;
                if (bulletScript != null) {
                    weaponType = bulletScript.getType();
                }
                switch (weaponType) {
                    case -1:
                        break;
                    case 0:
                        //Right Trigger
                        rightBulletHitCount++;
                        if (bulletScript != null) bulletScript.OnCollided(col);
                        if (rightBulletHitCount >= rightHitDestructionAmount) {
                            Debug.Log("Destroying the building: " + name);
                            destructMesh.SetActive(true);
                            GameObject.Destroy(gameObject);
                            HUDInfo.UpdateDestruction(destructionAmount);
                        }
                        else {
                            HUDInfo.UpdateDestruction(destructionBit);
                        }
                        break;
                    case 1:
                        //A weapon
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        Debug.Log("building collision: unsupported weaponType. Type: " + weaponType);
                        break;
                }
                bulletScript.OnCollided(col);
            }
        }
    }
    public void CollisionFaker(bullet bulletScript) {
        if (bulletScript == null) {
            Debug.Log("CollisionFaker null bulletScript!");
            return;
        }
        //Debug.Log("Calling CollisionFaker()");
        Debug.Log("bulletScript_type: " + bulletScript.getType());
        switch (bulletScript.getType()) {
            case -1:
                break;
            case 0:
                //Right Trigger
                rightBulletHitCount++;
                if (rightBulletHitCount >= rightHitDestructionAmount && !isDestroyed) {
                    Debug.Log("Destroying the building: " + name);
                    destructMesh.SetActive(true);
                    GameObject.Destroy(gameObject);
                    HUDInfo.UpdateDestruction(destructionAmount);
                    AudioDB.destroyBuilding.Play();
                    isDestroyed = true;
                    if (fountainQuitScript != null) fountainQuitScript.quitParticles();
                    killCitizens();
                }
                else {
                    HUDInfo.UpdateDestruction(destructionBit / rightHitDestructionAmount);
                }
                break;
            case 1:
                //A weapon
                weapon1HitCount++;
                //Debug.Log("A weapon hit, count: " + weapon1HitCount);
                if (weapon1HitCount >= weapon1HitDestructionAmount && !isDestroyed) {
                    Debug.Log("Destroying the building: " + name);
                    destructMesh.SetActive(true);
                    GameObject.Destroy(gameObject);
                    HUDInfo.UpdateDestruction(destructionAmount);
                    isDestroyed = true;
                    AudioDB.destroyBuilding.Play();
                    if (fountainQuitScript != null) fountainQuitScript.quitParticles();
                    killCitizens();
                }
                else {
                    HUDInfo.UpdateDestruction(Mathf.Clamp((int)(destructionBit / weapon1HitDestructionAmount),(int)1,(int)100));
                }
                break;
            case 2:
                //B weapon
                weapon2HitCount++;
                //Debug.Log("B weapon hit, count: " + weapon2HitCount);
                if (weapon2HitCount >= weapon2HitDestructionAmount && !isDestroyed) {
                    Debug.Log("Destroying the building: " + name);
                    destructMesh.SetActive(true);
                    GameObject.Destroy(gameObject);
                    HUDInfo.UpdateDestruction(destructionAmount);
                    isDestroyed = true;
                    AudioDB.destroyBuilding.Play();
                    if (fountainQuitScript != null) fountainQuitScript.quitParticles();
                    killCitizens();
                }
                else {
                    HUDInfo.UpdateDestruction(Mathf.Clamp((int)(destructionBit / weapon2HitDestructionAmount), (int)1, (int)100));
                }
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                Debug.Log("building collision: unsupported weaponType. Type: " + bulletScript.getType());
                break;
        }
    }

    //adds points for each citizen kill
    private void killCitizens() {
        HUDInfo.UpdateDestruction(citizenCount * HUDInfo.citizenWorth);
    }
    public void abductCitizen() {
        if (citizenCount >= 2) {
            HUDInfo.UpdateDestruction(HUDInfo.citizenWorth * 4);
            citizenCount = citizenCount - 2;
        }
        else if (citizenCount == 1) {
            HUDInfo.UpdateDestruction(HUDInfo.citizenWorth * 2);
            citizenCount = citizenCount - 1;
        }
    }

    public int getCitizenCount() { return this.citizenCount; }
    
}
