using UnityEngine;
using System.Collections;

public class AudioDB : MonoBehaviour {

    public AudioSource _destroyBuilding;
    public AudioSource _destroyApartment;
    public AudioSource _destroyTree;
    public AudioSource _destroyCar;

    public static AudioSource destroyBuilding;
    public static AudioSource destroyApartment;
    public static AudioSource destroyTree;
    public static AudioSource destroyCar;
	// Use this for initialization
	void Start () {
        destroyBuilding = _destroyBuilding;
        destroyApartment = _destroyApartment;
        destroyTree = _destroyTree;
        destroyCar = _destroyCar;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
