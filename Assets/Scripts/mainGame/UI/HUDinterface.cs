using UnityEngine;
using System.Collections;

public class HUDinterface : MonoBehaviour {
    public Vector2 startPosition;
    public float paddingY;
    public GameObject HUDCanvas;
    public GameObject btnTemplate;
    // Update is called once per frame
    void Start() {
        HUDInfo.startPosition = startPosition;
        HUDInfo.paddingY = paddingY;
        HUDInfo.HUDCanvas = HUDCanvas;
        HUDInfo.btnTemplate = btnTemplate;
    }
}
