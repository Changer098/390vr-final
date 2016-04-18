using UnityEngine;
using System.Collections;

public class buttonCreateTest : MonoBehaviour {

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
        Debug.Log("Firing B!");
    }
    public void fireX() {
        Debug.Log("Firing X!");
    }
    public void fireY() {
        Debug.Log("Firing Y!");
    }
}
