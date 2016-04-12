using UnityEngine;
using System.Collections;

public class CamerasSwitcher : MonoBehaviour {

    public GameObject ActualCameraRig;
    private GameObject TransferObjects;             //GameObject of the transferObjects script

	// Use this for initialization
	void Awake () {
        TransferObjects = GameObject.Find("LevelTransferObject");
        transferObjects tObject = TransferObjects.GetComponent<transferObjects>();
        GameObject otherCamera = tObject.getCamera();
        Destroy(otherCamera);
        tObject.Destroy();
        Debug.Log("Switched Cameras successfully");
	}
}
