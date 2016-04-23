using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDInfo : MonoBehaviour {
    public static Vector2 startPosition;
    public static float paddingY;
    public static GameObject HUDCanvas;
    public static GameObject btnTemplate;
    public static Image destructProgress;
    public static Image healthProgress;
    public static UFOHandler ufoHandler;

    public static int destruction = 0;
    public static float health = 1;
    public static int citizenWorth = 1;
    private struct UIbutton {
        public XboxKey key;
        public int keyCount;
        public GameObject Gamebutton;
        public int ammo;
        public int fullAmmo;            //the full capacity of the ammo
        public bool isAble;
    }
    private struct UITrigger {
        public XboxAxis axis;
        public GameObject Gamebutton;
        public int ammo;
        public int fullAmmo;            //the full capacity of the ammo
        public bool isAble;
    }
    public static void addTriggers() {
        AddTrigger(XboxAxis.RightTrigger, "laserFire", 1f, 10, HUDCanvas.transform.Find("Right Trigger").gameObject);
        AddTrigger(XboxAxis.LeftTrigger, "abduct", 0.5f, 300, HUDCanvas.transform.Find("Left Trigger").gameObject);
    }
    public static ArrayList buttonList = new ArrayList(6);

    public static void AddTrigger(XboxAxis axis, string weaponName, float refreshTime, int ammo, GameObject gameObj) {
        //Set btnData and add it to the list
        UITrigger btnData;
        btnData.axis = axis;
        btnData.Gamebutton = gameObj;
        btnData.ammo = ammo;
        btnData.isAble = true;
        btnData.fullAmmo = ammo;
        buttonList.Add(btnData);
        gameObj.GetComponent<buttonDetails>().updateInfo(weaponName, refreshTime, ammo, false);
        gameObj.GetComponent<buttonDetails>().updateAxis(axis);
        Debug.Log("Added Trigger");
    }
    //single Key method for adding a button
    public static void AddButton(XboxKey key, string weaponName, float refreshTime, int ammo) {
        //this method assumes the caller knows what they're doing
        //will add duplicates
        GameObject temp = Instantiate(btnTemplate);
        temp.transform.SetParent(HUDCanvas.transform, false);
        buttonDetails dets = temp.GetComponent<buttonDetails>();
        RectTransform Rtransform = btnTemplate.GetComponent<RectTransform>();

        dets.updateInfo(weaponName, refreshTime, ammo, true);
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
        anchored.x = startPosition.x;
        newTransform.anchoredPosition = anchored;

        //Set btnData and add it to the list
        UIbutton btnData;
        btnData.key = key;
        btnData.keyCount = 1;
        btnData.Gamebutton = button;
        btnData.ammo = ammo;
        btnData.isAble = true;
        btnData.fullAmmo = ammo;
        buttonList.Add(btnData);
    }

    public static bool getAble(XboxKey key) {
        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i] is UIbutton) {
                UIbutton tmp = (UIbutton)buttonList[i];
                if (tmp.key == key)
                    return tmp.isAble;
            }
        }
        return false;
    }
    public static bool getAble(XboxAxis axis) {
        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i] is UITrigger) {
                UITrigger tmp = (UITrigger)buttonList[i];
                if (tmp.axis == axis) 
                    return tmp.isAble;
                }
            }
        return false;
    }
    public static int setAmmo(XboxKey key, int ammo) {
        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i] is UIbutton) {
                UIbutton tmp = (UIbutton)buttonList[i];
                if (tmp.key == key) {
                    tmp.ammo = ammo;
                    buttonList[i] = tmp;
                    callUpdateAmmoAmount(i, ammo);
                    return tmp.ammo;
                }
            }            
        }
        return -1;
    }
    public static int setAmmo(XboxAxis axis, int ammo) {
        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i] is UITrigger) {
                UITrigger tmp = (UITrigger)buttonList[i];
                if (tmp.axis == axis) {
                    tmp.ammo = ammo;
                    buttonList[i] = tmp;
                    callUpdateAmmoAmount(i, ammo);
                    return tmp.ammo;
                }
            }
        }
        return -1;
    }
    public static int getAmmo(XboxKey key) {
        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i] is UIbutton) {
                UIbutton tmp = (UIbutton)buttonList[i];
                if (tmp.key == key) {
                    return tmp.ammo;
                }
            }           
        }
        return -1;
    }
    public static int getAmmo(XboxAxis axis) {
        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i] is UITrigger) {
                UITrigger tmp = (UITrigger)buttonList[i];
                if (tmp.axis == axis) {
                    return tmp.ammo;
                }
            }           
        }
        return -1;
    }

    public static int resetAmmo(XboxKey key) {
        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i] is UIbutton) {
                UIbutton tmp = (UIbutton)buttonList[i];
                if (tmp.key == key) {
                    tmp.ammo = tmp.fullAmmo;
                    buttonList[i] = tmp;
                    callUpdateAmmoAmount(i, tmp.fullAmmo);
                    return tmp.ammo;
                }
            }          
        }
        return -1;
    }
    public static int resetAmmo(XboxAxis axis) {
        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i] is UITrigger) {
                Debug.Log("iterating");
                UITrigger tmp = (UITrigger)buttonList[i];
                if (tmp.axis == axis) {
                    tmp.ammo = tmp.fullAmmo;
                    buttonList[i] = tmp;
                    callUpdateAmmoAmount(i, tmp.fullAmmo);
                    Debug.Log("reset ammo with " + tmp.ammo);
                    return tmp.ammo;
                }
                else Debug.Log("tmp : " + tmp.axis.ToString() + "requested: " + axis.ToString());
            }       
        }
        return -1;
    }
    public static void callUpdateAmmoAmount(int index, int ammount) {
        if (buttonList[index] is UIbutton) {
            UIbutton tmp = (UIbutton)buttonList[index];
            buttonDetails dets = tmp.Gamebutton.GetComponent<buttonDetails>();
            dets.updateAmmoAmmount(ammount);
        }
        else if (buttonList[index] is UITrigger) {
            UITrigger tmp = (UITrigger)buttonList[index];
            buttonDetails dets = tmp.Gamebutton.GetComponent<buttonDetails>();
            dets.updateAmmoAmmount(ammount);
        }
    }
    public static void callReload(XboxKey key) {
        Debug.Log("Reloading!");
        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i] is UIbutton) {
                UIbutton tmp = (UIbutton)buttonList[i];
                if (tmp.key == key) {
                    buttonDetails dets = tmp.Gamebutton.GetComponent<buttonDetails>();
                    dets.reload();
                    break;
                }
            }
        }
    }
    public static void callReload(XboxAxis axis) {
        Debug.Log("Reloading!");
        for (int i = 0; i < buttonList.Count; i++) {
            if (buttonList[i] is UITrigger) {
                UITrigger tmp = (UITrigger)buttonList[i];
                if (tmp.axis == axis) {
                    buttonDetails dets = tmp.Gamebutton.GetComponent<buttonDetails>();
                    dets.reload();
                    break;
                }
            }
        }
    }

    public static void UpdateDestruction(int damage) {
        int upgradeAmount;
        switch(ufoHandler.getUpgradeLevel()) {
            case 1:
                upgradeAmount = 100;
                break;
            case 2:
                upgradeAmount = 1000;
                break;
            case 3:
                upgradeAmount = 10000;
                break;
            case 4:
                upgradeAmount = 50000;
                break;
            default:
                upgradeAmount = 100000;
                break;
        }
        destruction = destruction + damage;
        float destructionProgAmnt = (float)destruction / (float)upgradeAmount;
        if (ufoHandler.destructionUpdate(destructionProgAmnt)) {
            destructProgress.fillAmount = 0;
        }
        else {
            destructionProgAmnt = Mathf.Clamp(destructionProgAmnt, 0, 1);
            destructProgress.fillAmount = destructionProgAmnt;
        }
    }
    public static void UpdateHealth(float damage) {
        health = health - damage;
        health = Mathf.Clamp01(health);
        if (ufoHandler.healthUpdate(health)) {

        }
        healthProgress.fillAmount = health;
    }
}
