using UnityEngine;
using System.Collections;

public class moveCamera : MonoBehaviour {

    public GameObject cameraPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (moveCamera.isAlive) transform.position = cameraPos.transform.position;
	}
    public static bool isAlive = true;
}
