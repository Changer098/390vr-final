using UnityEngine;
using PurdueVR.InputManager;
using System.Collections;

public class buttonCreateTestController : MonoBehaviour {

    public bool Amade = false;
    public bool Bmade = false;
    public bool Xmade = false;
    public bool Ymade = false;

    InputKey ABtn = new InputKey("A Btn", KeyCode.A, XboxKey.A);
    InputKey BBtn = new InputKey("B Btn", KeyCode.B, XboxKey.B);
    InputKey XBtn = new InputKey("X Btn", KeyCode.X, XboxKey.X);
    InputKey YBtn = new InputKey("Y Btn", KeyCode.Y, XboxKey.Y);
    void Start() {
        //11 elements in list
        //Select - Xbox.A_buttons[0]
        keyBinding.addButton(new InputKey("B Btn", KeyCode.B, XboxKey.B));  //buttons[11]
        keyBinding.addButton(new InputKey("Y Btn", KeyCode.Y, XboxKey.Y));  //buttons[12]
        keyBinding.addButton(new InputKey("X Btn", KeyCode.X, XboxKey.X));  //buttons[13]
    }
    void Update() {
        if (managerMain.GetKeyDown(ABtn)) {
            // A Btn
            Debug.Log("Pressed A");
            if (!Amade) {
                createABtn();
                Amade = true;
            }
            else {
                fireA();
            }
        }
        if (managerMain.GetKeyDown(BBtn)) {
            Debug.Log("Pressed B");
            //B Btn
            if (!Bmade) {
                createBBtn();
                Bmade = true;
            }
            else {
                fireB();
            }
        }
        if (managerMain.GetKeyDown(XBtn)) {
            Debug.Log("Pressed X");
            //X Btn
            if (!Xmade) {
                createXBtn();
                Xmade = true;
            }
            else {
                fireX();
            }
        }
        if (managerMain.GetKeyDown(YBtn)) {
            Debug.Log("Pressed Y");
            //Y Btn
            if (!Ymade) {
                createYBtn();
                Ymade = true;
            }
            else {
                fireY();
            }
        }
    }

    public void createABtn() {
        HUDInfo.AddButton(XboxKey.A, "testWeapn", 1, 5);
    }
    public void createBBtn() {
        HUDInfo.AddButton(XboxKey.B, "testB", 0.5f, 2);
    }
    public void createXBtn() {
        HUDInfo.AddButton(XboxKey.X, "testX", 0.5f, 3);
    }
    public void createYBtn() {
        HUDInfo.AddButton(XboxKey.Y, "testY", 0.75f, 1);
    }
    public void fireA() {
        if (HUDInfo.getAble(XboxKey.A)) {
            Debug.Log("Firing A!");
            int ammoCount = HUDInfo.getAmmo(XboxKey.A);
            if (ammoCount > 0) {
                Debug.Log("Ammo: " + ammoCount);
                ammoCount = ammoCount - 1;
                Debug.Log("setAmmo(): " + HUDInfo.setAmmo(XboxKey.A, ammoCount));
                if (HUDInfo.getAmmo(XboxKey.A) == 0) {
                    HUDInfo.callReload(XboxKey.A);
                }
            }
            else {
                Debug.Log("Cannot fire, getAmmo <= 0");
            }
        }

    }
    public void fireB() {
        if (HUDInfo.getAble(XboxKey.B)) {
            Debug.Log("Firing B!");
            int ammoCount = HUDInfo.getAmmo(XboxKey.B);
            if (ammoCount > 0) {
                Debug.Log("Ammo: " + ammoCount);
                ammoCount = ammoCount - 1;
                Debug.Log("setAmmo(): " + HUDInfo.setAmmo(XboxKey.B, ammoCount));
                if (HUDInfo.getAmmo(XboxKey.B) == 0) {
                    HUDInfo.callReload(XboxKey.B);
                }
            }
            else {
                Debug.Log("Cannot fire, getAmmo <= 0");
            }
        }
    }
    public void fireX() {
        if (HUDInfo.getAble(XboxKey.X)) {
            Debug.Log("Firing X!");
            int ammoCount = HUDInfo.getAmmo(XboxKey.X);
            if (ammoCount > 0) {
                Debug.Log("Ammo: " + ammoCount);
                ammoCount = ammoCount - 1;
                Debug.Log("setAmmo(): " + HUDInfo.setAmmo(XboxKey.X, ammoCount));
                if (HUDInfo.getAmmo(XboxKey.X) == 0) {
                    HUDInfo.callReload(XboxKey.X);
                }
            }
            else {
                Debug.Log("Cannot fire, getAmmo <= 0");
            }
        }
    }
    public void fireY() {
        if (HUDInfo.getAble(XboxKey.Y)) {
            Debug.Log("Firing Y!");
            int ammoCount = HUDInfo.getAmmo(XboxKey.Y);
            if (ammoCount > 0) {
                Debug.Log("Ammo: " + ammoCount);
                ammoCount = ammoCount - 1;
                Debug.Log("setAmmo(): " + HUDInfo.setAmmo(XboxKey.Y, ammoCount));
                if (HUDInfo.getAmmo(XboxKey.Y) == 0) {
                    HUDInfo.callReload(XboxKey.Y);
                }
            }
            else {
                Debug.Log("Cannot fire, getAmmo <= 0");
            }
        }
    }
}
