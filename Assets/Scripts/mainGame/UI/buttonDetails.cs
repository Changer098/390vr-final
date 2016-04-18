using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class buttonDetails : MonoBehaviour {

    [SerializeField]private int ButtonCount = 1;
    [SerializeField]private string weaponName = "";
    [SerializeField]private float refreshTime = 1.0f;
    [SerializeField]private GameObject progressRing;
    [SerializeField]private int ammo = 1;
    XboxKey key;
    private imagesDB imagesDB;
	// Use this for initialization
	void Start () {
        GameObject HUD = GameObject.Find("HUD");
        if (HUD == null) {
            Debug.Log("buttonDetails: Unable to find HUD");
        }
        else imagesDB = HUD.GetComponent<imagesDB>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public GameObject Create(XboxKey key, RectTransform transform) {
        ButtonCount = 1;
        RectTransform original = GetComponent<RectTransform>();
        if (original == null) {
            Debug.Log("Create(): Failed to get RectTransform");
        }
        Image image = GetComponent<Image>();
        if (image == null) {
            Debug.Log("Create(): Failed to get Image Script");
        }
        if (image.sprite == null) {
            Debug.Log("Create(): Image.sprite is null. Halp");
        }
        Start();
        original = transform;
        switch (key) {
            case XboxKey.A:
                if (image.sprite == null) Debug.Log("image.sprite is null");
                if (imagesDB == null) Debug.Log("imagesDB is null");
                image.sprite = imagesDB.ABtn;
                break;
            case XboxKey.B:
                image.sprite = imagesDB.BBtn;
                break;
            case XboxKey.X:
                image.sprite = imagesDB.XBtn;
                break;
            case XboxKey.Y:
                image.sprite = imagesDB.YBtn;
                break;
            case XboxKey.LeftShoulder:
                image.sprite = imagesDB.LeftShoulder;
                break;
            case XboxKey.RightShoulder:
                image.sprite = imagesDB.RightShoulder;
                break;
        }
        this.key = key;
        return this.gameObject;
    }
    public void Create2(XboxKey[] key, RectTransform transform) {
        ButtonCount = 2;
        RectTransform original = GetComponent<RectTransform>();
        Image image = GetComponent<Image>();
        original = transform;
    }
    public void updateInfo(string weaponName, float refreshTime, int ammo) {
        this.weaponName = weaponName;
        this.refreshTime = refreshTime;
        this.ammo = ammo;
        name = weaponName;
    }
    public void reload() {
        StartCoroutine(reloadRoutine());
    }
    IEnumerator reloadRoutine() {
        yield return new WaitForEndOfFrame();
        HUDInfo.resetAmmo(key);
    }
}
