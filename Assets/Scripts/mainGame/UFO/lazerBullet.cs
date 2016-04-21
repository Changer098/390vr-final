using UnityEngine;
using System.Collections;

public class lazerBullet : MonoBehaviour, bullet {

    public int bulletType;
    public float fireForce;
    public float timeSpan;
    // Use this for initialization
	void Start () {
        StartCoroutine(destroyAfterTime());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getType() {
        return bulletType;
    }
    public float getFireForce() {
        return fireForce;
    }

    public void OnCollided(Collision coll) {
        Destroy(gameObject);
    }

    IEnumerator destroyAfterTime() {
        yield return new WaitForSeconds(timeSpan);
        Destroy(gameObject);
    }
}
