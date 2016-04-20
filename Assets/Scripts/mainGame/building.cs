using UnityEngine;
using System.Collections;

public class building : MonoBehaviour {

    public GameObject DM_prefab;
    private GameObject destructMesh;
    public int citizenCount;
	// Use this for initialization
	void Start () {
        citizenCount = Random.Range(0, 200);
        StartCoroutine(generateColliders());
        

    }

    // Update is called once per frame
    void Update() {

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
}
