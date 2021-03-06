﻿using UnityEngine;
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
