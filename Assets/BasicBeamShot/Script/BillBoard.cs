using UnityEngine;
using System.Collections;

public class BillBoard : MonoBehaviour
{
	private Camera LookAtCam;
	
	public float RandomRotate = 0;
	private float RndRotate;
	Transform this_t_;
	
	void Awake() {
		this_t_ = this.transform;
		RndRotate = Random.value*RandomRotate;
	}
	
	void Update() {
		LookAtCam = Camera.main;
		if ( LookAtCam == null ) return;
		Transform cam_t = LookAtCam.transform;
		
		Vector3 vec = cam_t.position - this_t_.position;
        Vector3 scale = new Vector3();
        scale.x = this_t_.localScale.x / 2;
        scale.y = this_t_.localScale.y / 2;
        scale.z = this_t_.localScale.z / 2;

        this_t_.localScale.Set(scale.x, scale.y, scale.z);

		vec.x = vec.z = 0.0f;
		this_t_.LookAt(cam_t.position - vec); 
		this_t_.Rotate(Vector3.forward,RndRotate);
	}
}