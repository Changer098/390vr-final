using UnityEngine;
using System.Collections;

public class lazerBullet : MonoBehaviour, bullet {

    public int bulletType;
    public float fireForce;
    public float timeSpan;
    // Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getType() {
        return bulletType;
    }
    public void setType(int set) {
        bulletType = set;
    }
    public float getFireForce() {
        return fireForce;
    }

    public void OnCollided(Collision coll) {
        Debug.Log("calling collided");
        //Destroy(gameObject);
    }
}
