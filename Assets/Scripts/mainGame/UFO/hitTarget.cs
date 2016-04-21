using UnityEngine;
using System.Collections;

public class hitTarget : MonoBehaviour {

    public Color noHitColor;
    public Color HitColor;
    public int hitLayer = -1;
    Renderer rend;
	// Use this for initialization
	void Start () {
        rend = gameObject.GetComponent<Renderer>();
        rend.material.color = noHitColor;
	}
	
	public void colorUpdate(RaycastHit hit) {
        transform.position = hit.point;
        hitLayer = hit.collider.gameObject.layer;
        if (hit.collider != null && hit.collider.gameObject.layer == 11) {
            rend.material.color = HitColor;
        }
        else {
            hitLayer = hit.collider.gameObject.layer;
            rend.material.color = noHitColor;
        }
    }
}
