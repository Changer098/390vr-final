using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDinterface : MonoBehaviour {
    public Vector2 startPosition;
    public float paddingY;
    public GameObject HUDCanvas;
    public GameObject btnTemplate;
    public Image healthProgress;
    public Image destructProgress;
    public int citizenWorth;
    //public GameObject UFO;
    // Update is called once per frame
    void Awake() {
        HUDInfo.buttonList = new ArrayList(6);
        HUDInfo.startPosition = startPosition;
        HUDInfo.paddingY = paddingY;
        HUDInfo.HUDCanvas = HUDCanvas;
        HUDInfo.btnTemplate = btnTemplate;
        HUDInfo.healthProgress = healthProgress;
        HUDInfo.destructProgress = destructProgress;
        GameObject UFO = GameObject.Find("UFO");
        HUDInfo.ufoHandler = UFO.GetComponent<UFOHandler>();
        HUDInfo.citizenWorth = citizenWorth;
    }
}
