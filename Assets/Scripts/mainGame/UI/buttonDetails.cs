using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class buttonDetails : MonoBehaviour {

    [SerializeField]
    private int ButtonCount = 1;
    [SerializeField]
    private string weaponName = "";
    [SerializeField]
    private float refreshTime = 1.0f;
    [SerializeField]
    private GameObject progressRing;
    [SerializeField]
    private int ammo = 1;
    [SerializeField]
    private int currAmmo = 0;
    private Image progressImage;
    XboxKey key;
    XboxAxis axis;
    private imagesDB imagesDB;
    private bool isButton;
    // Use this for initialization
    void Start() {
        GameObject HUD = GameObject.Find("HUD");
        if (HUD == null) {
            Debug.Log("buttonDetails: Unable to find HUD");
        }
        else imagesDB = HUD.GetComponent<imagesDB>();
    }

    // Update is called once per frame
    void Update() {

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
    public void updateInfo(string weaponName, float refreshTime, int ammo, bool isButton) {
        this.weaponName = weaponName;
        this.refreshTime = refreshTime;
        this.ammo = ammo;
        this.isButton = isButton;
        name = weaponName;
        currAmmo = ammo;
        progressImage = progressRing.GetComponent<Image>();
    }
    public void updateAxis(XboxAxis axis) {
        this.axis = axis;
    }
    public void reload() {
        StartCoroutine(reloadRoutine());
    }
    IEnumerator reloadRoutine() {
        Color original = GetComponent<Image>().color;
        Color faded = new Color(original.r, original.g, original.b, 0.5f);
        GetComponent<Image>().color = faded;
        float currProgress = progressImage.fillAmount;
        float addAmmount = refreshTime * 0.05f;
        Debug.Log("reloadRoutine(): addAmount = " + addAmmount);
        while (progressImage.fillAmount < 1) {
            progressImage.fillAmount = progressImage.fillAmount + addAmmount;
            yield return new WaitForSeconds(0.05f);
        }
        if (isButton) {
            int tmp = HUDInfo.resetAmmo(key);
            if (tmp == -1) {
                Debug.Log("reload(): unable to reset for button");
            }
            else {
                Debug.Log("tmp = " + tmp);
                GetComponent<Image>().color = original;
            }
        }
        else {
            int tmp = HUDInfo.resetAmmo(axis);
            if (tmp == -1) {
                Debug.Log("reload(): unable to reset for trigger");
            }
            else {
                Debug.Log("tmp = " + tmp);
                GetComponent<Image>().color = original;
            }
        }
        //GetComponent<Image>().color = original;
    }
    public void updateAmmoAmmount(int currAmmo) {
        this.currAmmo = currAmmo;
        float progress = (float)currAmmo / (float)ammo;
        progressImage.fillAmount = progress;
    }
}
