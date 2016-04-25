using UnityEngine;
using System.Collections;

public class treeDestroy : MonoBehaviour, destructable {

    public GameObject fireStump;
    bool canSub = false;
	// Use this for initialization
	void Start () {
        if (fireStump != null) canSub = true;
	}
	

    public void CollisionFaker(bullet bulletScript) {
        if (canSub) {
            GameObject newStump = (GameObject)Instantiate(fireStump, gameObject.transform.position, gameObject.transform.rotation);
            newStump.SetActive(true);
        }
        HUDInfo.UpdateDestruction(5);
        AudioDB.destroyTree.Play();
        Destroy(gameObject);
    }
    public void abductCitizen() {
        return;
    }
    public int getCitizenCount() {
        return 0;
    }
}
