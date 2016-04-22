using UnityEngine;
using System.Collections;

public class building : MonoBehaviour {

    public GameObject DM_prefab;
    private GameObject destructMesh;
    public destroyCall destroyScript;
    public int citizenCount;
    public int rightHitDestructionAmount = 5;
    public int destructionAmount = 100;                   //How many points we gain for destroying

    int rightBulletHitCount = 0;
    int weapon1HitCount = 0;
    int weapon2HitCount = 0;
    int weapon3HitCount = 0;
    int weapon4HitCount = 0;
    int destructionBit;                             //How many points we gain for hit
    public Vector3 scale;
    public bool debug = false;
	// Use this for initialization
	void Start () {
        citizenCount = Random.Range(0, 200);
        if (scale.x == 0) {
            scale = new Vector3(1, 1, 1);
        }
        destructionBit = (int)(0.1f * destructionAmount); 
        StartCoroutine(generateColliders());
    }

    IEnumerator generateColliders() {
        Vector3 rotation = gameObject.transform.rotation.eulerAngles;
        rotation.x = 0;
        destructMesh = (GameObject)Instantiate(DM_prefab, gameObject.transform.position, QuanternionHelper.Euler(rotation.x, rotation.y, rotation.z));
        destructMesh.transform.localScale = scale;
        destructMesh.transform.position = gameObject.transform.position;
        destructMesh.AddComponent<buildDestruct>();

        int childCount = destructMesh.transform.childCount;
        for (int i = 0; i < childCount; i++) {
            Transform childT = destructMesh.transform.GetChild(i);
            if (childT.name == "Plane") {
                Debug.Log("Skipping on: " + gameObject.name);
                continue;
            }
            else {
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
        switch (bulletScript.getType()) {
            case -1:
                break;
            case 0:
                //Right Trigger
                rightBulletHitCount++;
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
                Debug.Log("building collision: unsupported weaponType. Type: " + bulletScript.getType());
                break;
        }
    }
}
