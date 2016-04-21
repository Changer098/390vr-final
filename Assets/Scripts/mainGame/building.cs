using UnityEngine;
using System.Collections;

public class building : MonoBehaviour {

    public GameObject DM_prefab;
    private GameObject destructMesh;
    public destroyCall destroyScript;
    public int citizenCount;

    int rightBulletHitCount = 0;
    int weapon1HitCount = 0;
    int weapon2HitCount = 0;
    int weapon3HitCount = 0;
    int weapon4HitCount = 0;
	// Use this for initialization
	void Start () {
        citizenCount = Random.Range(0, 200);
        StartCoroutine(generateColliders());
    }

    IEnumerator generateColliders() {
        Vector3 rotation = gameObject.transform.rotation.eulerAngles;
        rotation.x = 0;
        destructMesh = (GameObject)Instantiate(DM_prefab, gameObject.transform.position, QuanternionHelper.Euler(rotation.x, rotation.y, rotation.z));
        //destructMesh.transform.SetParent(gameObject.transform);
        int childCount = destructMesh.transform.childCount;
        for (int i = 0; i < childCount; i++) {
            Transform childT = destructMesh.transform.GetChild(i);
            MeshCollider collider = childT.gameObject.AddComponent<MeshCollider>();
            //Rigidbody rigid = childT.gameObject.AddComponent<Rigidbody>();
            collider.convex = true;
            //rigid.drag = 1;
            //collider.enabled = false;
            //rigid.Sleep();
            //childT.gameObject.SetActive(false);

            yield return null;
        }
        destructMesh.SetActive(false);
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
                Debug.Log(gameObject.name + " collided with " + col.gameObject.name);
                bullet bulletScript = col.gameObject.GetComponent<bullet>();
                int bType = bulletScript.getType();
                switch (bType) {
                    case -1:
                        //Left Trigger
                        //do abducting shit
                        break;
                    case 0:
                        //Right Trigger
                        rightBulletHitCount++;
                        if (rightBulletHitCount >= 5) {
                            //do destruction
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }
            Debug.Log("Colliding with " + col.collider.gameObject.name);
        }
    }
}
