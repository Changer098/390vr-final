using UnityEngine;
using System.Collections;

public class carDestroy : MonoBehaviour, destructable {

    public GameObject deadCar;
    bool canSub = false;
    Vector3 scale = new Vector3(24.20886f, 24.20886f, 24.20886f);
	// Use this for initialization
	void Start () {
        if (deadCar != null) canSub = true;
	}
	

    public void CollisionFaker(bullet bulletScript) {
        if (canSub) {
            GameObject newStump = (GameObject)Instantiate(deadCar, gameObject.transform.position, gameObject.transform.rotation);
            newStump.transform.localScale = scale;
            newStump.GetComponent<Renderer>().materials = gameObject.GetComponent<Renderer>().materials;
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
