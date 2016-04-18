using UnityEngine;
using System.Collections;

public class moveCamera : MonoBehaviour {

    public GameObject cameraPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = cameraPos.transform.position;
	}
}
