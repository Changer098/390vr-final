using UnityEngine;
using System.Collections;

public class HUDInfo : MonoBehaviour {
    public static Vector2 startPosition;
    public static float paddingY;
    public static GameObject HUDCanvas;
    public static GameObject btnTemplate;
    private struct UIbutton {
        public XboxKey[] key;
        public int keyCount;
        public GameObject Gamebutton;
        public int ammo;
        public int fullAmmo;            //the full capacity of the ammo
        public bool isAble;
    }
    public static ArrayList buttonList = new ArrayList(6);
    //single Key method for adding a button
	public static void AddButton(XboxKey key, string weaponName, float refreshTime, int ammo) {
        //this method assumes the caller knows what they're doing
        //will add duplicates
        GameObject temp = Instantiate(btnTemplate);
        temp.transform.SetParent(HUDCanvas.transform, false);
        buttonDetails dets = temp.GetComponent<buttonDetails>();
        RectTransform Rtransform = btnTemplate.GetComponent<RectTransform>();

        dets.updateInfo(weaponName, refreshTime, ammo);
        GameObject button = dets.Create(key, Rtransform);
        //change position by size and add padding
        RectTransform newTransform = button.GetComponent<RectTransform>();
        Vector2 anchored = newTransform.anchoredPosition;
        float posY;
        if (buttonList.Count == 0) {
            posY = startPosition.y;
        }
        else posY = startPosition.y - (paddingY * buttonList.Count) - (Rtransform.rect.height * buttonList.Count);
        Debug.Log("posY: " + posY);
        anchored.y = posY;
        newTransform.anchoredPosition = anchored;

        //Set btnData and add it to the list
        UIbutton btnData;
        btnData.key = new XboxKey[1];
        btnData.key[0] = key;
        btnData.keyCount = 1;
        btnData.Gamebutton = button;
        btnData.ammo = ammo;
        btnData.isAble = true;
        btnData.fullAmmo = ammo;
        buttonList.Add(btnData);
    }

    //Overloaded method for creating one with multiple buttons 
    public static void AddButton(XboxKey[] key, int keyCount, string weaponName, float refreshTime) {

    }
    public static bool getAble(XboxKey key) {
        foreach (UIbutton btn in buttonList) {
            if (btn.key[0] == key) {
                return btn.isAble;
            }
        }
        return false;
    }
    public static int setAmmo(XboxKey key, int ammo) {
        for (int i = 0; i < buttonList.Count; i++) {
            UIbutton tmp = (UIbutton)buttonList[i];
            if (tmp.key[0] == key) {
                tmp.ammo = ammo;
                buttonList[i] = tmp;
                return tmp.ammo;
            }
        }
        return -1;
    }
    public static int getAmmo(XboxKey key) {
        for (int i = 0; i < buttonList.Count; i++) {
            UIbutton tmp = (UIbutton)buttonList[i];
            if (tmp.key[0] == key) {
                return tmp.ammo;
            }
        }
        return -1;
    }

    public static int resetAmmo(XboxKey key) {
        for (int i = 0; i < buttonList.Count; i++) {
            UIbutton tmp = (UIbutton)buttonList[i];
            if (tmp.key[0] == key) {
                tmp.ammo = tmp.fullAmmo;
                buttonList[i] = tmp;
                return tmp.ammo;
            }
        }
        return -1;
    }
    public static void callReload(XboxKey key) {
        Debug.Log("Reloading!");
        foreach (UIbutton btn in buttonList) {
            if (btn.key[0] == key) {
                buttonDetails dets = btn.Gamebutton.GetComponent<buttonDetails>();
                dets.reload();
            }
        }
    }
}
