using UnityEngine;
using System.Collections;

public class buildDestruct : MonoBehaviour {

    // Use this for initialization
    public float radius = 0;
    public float force = 0;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void hugeExplosion() {
        Debug.Log("huge ass explosion");
        Rigidbody rigid = GetComponent<Rigidbody>();
        Renderer render = GetComponent<Renderer>();
        //rigid.AddExplosionForce(force, render.bounds.center, radius);
    }

    /*void OnCollisionEnter(Collision col) {
        if (col.gameObject.layer == 11) {
            bullet bulletScript = col.gameObject.GetComponent<bullet>();
            if (bulletScript != null) {
                bulletScript.OnCollided(col);
                Rigidbody rigid = GetComponent<Rigidbody>();
                Debug.Log("Exploding");
                rigid.AddExplosionForce(2, col.transform.position, 5);
            }
        }
    }*/
}
