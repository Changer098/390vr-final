using UnityEngine;
using System.Collections;

public class building : MonoBehaviour {

    public GameObject DM_prefab;
    private GameObject destructMesh;
    public int citizenCount;
	// Use this for initialization
	void Start () {
        citizenCount = Random.Range(0, 200);
        StartCoroutine(colliderCreate());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //create colliders and run semi-asnychronously
    //optimize for every building running this on start
    IEnumerator colliderCreate() {
        destructMesh = Instantiate(DM_prefab);
        destructMesh.transform.position = transform.position;
        destructMesh.transform.rotation = transform.rotation;
        gameObject.SetActive(false);
        int childCount = DM_prefab.transform.childCount;
        yield return null;
    }
}
