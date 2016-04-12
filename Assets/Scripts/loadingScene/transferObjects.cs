using UnityEngine;
using System.Collections;

public class transferObjects : MonoBehaviour {

    private GameObject OVRCameraRig;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
    public void setCamera(GameObject foo) {
        OVRCameraRig = foo;
        Debug.Log("Set Camera for transfer");
    }
    public GameObject getCamera() {
        return OVRCameraRig;
    }
    public void Destroy() {
        Destroy(gameObject);
    }
}
