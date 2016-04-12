﻿using UnityEngine;
using PurdueVR.InputManager;
using UnityEngine.UI;
using System.Collections;

public class menuNavigation : MonoBehaviour {

    // Use this for initialization
    public int index = -1;
    int secondaryIndex = -1;
    public Canvas mainCanvas;
    public Canvas settingsCanvas;
    int mainElementsCount = 0;
    int settingsElementsCount = 0;
    bool canMove = true;
    public float TmpMove = 0;
    //mainCanvas elements
    public Button playBtn;
    public Button settingsBtn;
    public Button calibrateBtn;

    public Button backBtn;
    public Toggle controllerToggle;
    public Toggle VRToggle;
    public Color SelectColor;

    private Hashtable colorTable = new Hashtable();  //original colors
	void Start () {
        index = 0;
        secondaryIndex = -1;

        colorTable.Add("playBtn", playBtn.colors.normalColor);
        colorTable.Add("settingsBtn", settingsBtn.colors.normalColor);
        colorTable.Add("calibrateBtn", calibrateBtn.colors.normalColor);
        colorTable.Add("backBtn", backBtn.colors.normalColor);
        colorTable.Add("controllerToggle", controllerToggle.colors.normalColor);
        colorTable.Add("VRToggle", VRToggle.colors.normalColor);

        selectObject();
	}
	
	// Update is called once per frame
	void Update () {
        TmpMove = managerMain.GetAxis(keyBinding.buttons[2]);
        if (managerMain.GetKeyDown(keyBinding.buttons[0]))
        {
            //select button pressed
            if (index != -1) {
                //mainCanvas
                switch (index) {
                    case 0:
                        //settingsBtn
                        break;
                    case 1:
                        //calibrateBtn
                        break;
                    case 2:
                        //playBtn
                        break;
                }
            }
            else {
                //settingsCanvas

            }
        }
        //Don't call get axis without controller
        if ((managerMain.currentInput.isXbox && managerMain.GetAxis(keyBinding.buttons[1]) != 0) || (!managerMain.currentInput.isXbox && GetAxisKeys(true) != 0)) {
            //vertical axis activated
            if (index != -1) {
                //in the mainCanvas
                if (managerMain.currentInput.isXbox) {
                    switch(index) {
                        case 0:
                            if (managerMain.GetAxis(keyBinding.buttons[1]) < -0.4 && canMove) {
                                //settingsBtn, should navigate to playBtn
                                index = 2;
                                selectObject();
                                StartCoroutine(moveWait());
                            }
                            break;
                        case 1:
                            if (managerMain.GetAxis(keyBinding.buttons[1]) < -0.4 && canMove) {
                                //calibrateBtn, should navigate to playBtn
                                index = 2;
                                selectObject();
                                StartCoroutine(moveWait());
                            }
                            break;
                        case 2:
                            if (managerMain.GetAxis(keyBinding.buttons[1]) > 0.4 && canMove) {
                                //playBtn, should navigate to settingsBtn
                                index = 0;
                                selectObject();
                                StartCoroutine(moveWait());
                            }
                            break;
                    }                 
                }
                else {
                    switch(index) {
                        case 0:
                            if (GetAxisKeys(true) < 0) {
                                //settingsBtn, should navigate to playBtn
                                index = 2;
                                selectObject();
                            }
                            break;
                        case 1:
                            if (GetAxisKeys(true) < 0) {
                                //calibrateBtn, should navigate to playBtn
                                index = 2;
                                selectObject();
                            }
                            break;
                        case 2:
                            if (GetAxisKeys(true) > 0) {
                                //playBtn, should navigate to settingsBtn
                                index = 0;
                                selectObject();
                            }
                            break;
                    }
                }
            }
            else {
                //in the settingsCanvas
                if (managerMain.currentInput.isXbox) {

                }
                else {

                }
            }
        }
        else if((managerMain.currentInput.isXbox && managerMain.GetAxis(keyBinding.buttons[2]) != 0) || (!managerMain.currentInput.isXbox && GetAxisKeys(false) != 0))
        {
            //horizontal axis activated
            if (index != -1) {
                if (managerMain.currentInput.isXbox) {
                    //opposite direction
                    switch (index) {
                        case 0:
                            if (managerMain.GetAxis(keyBinding.buttons[2]) == 1 && canMove) {
                                //settingsBtn, should navigate to calibrateBtn
                                index = 1;
                                selectObject();
                                StartCoroutine(moveWait());
                            }
                            break;
                        case 1:
                            if (managerMain.GetAxis(keyBinding.buttons[2]) == -1 && canMove) {
                                //calibrateBtn, should navigate to settingsBtn
                                index = 0;
                                selectObject();
                                StartCoroutine(moveWait());
                            }
                            break;
                    }
                }
                else {
                    switch (index) {
                        case 0:
                            if (GetAxisKeys(false) < 0) {
                                //settingsBtn, should navigate to calibrateBtn
                                index = 1;
                                selectObject();
                            }
                            break;
                        case 1:
                            if (GetAxisKeys(false) > 0) {
                                //calibrateBtn, should navigate to settingsBtn
                                index = 0;
                                selectObject();
                            }
                            break;
                    }
                }
            }
            else {
                if (managerMain.currentInput.isXbox) {

                }
                else {

                }
            }
        }
	}

    void selectObject() {
        if (index == -1) {
            //modify secondaryIndex colors

        }
        if (secondaryIndex == -1) {
            //modify index colors
            ColorBlock colors;
            switch(index) {
                case 0:             //settings
                    colors = settingsBtn.colors;
                    colors.normalColor = SelectColor;
                    settingsBtn.colors = colors;
                    colors = playBtn.colors;
                    colors.normalColor = (Color)colorTable["playBtn"];
                    playBtn.colors = colors;
                    colors = calibrateBtn.colors;
                    colors.normalColor = (Color)colorTable["calibrateBtn"];
                    calibrateBtn.colors = colors;
                    break;
                case 1:             //calibrate
                    colors = calibrateBtn.colors;
                    colors.normalColor = SelectColor;
                    calibrateBtn.colors = colors;
                    colors = playBtn.colors;
                    colors.normalColor = (Color)colorTable["playBtn"];
                    settingsBtn.colors = colors;
                    colors = settingsBtn.colors;
                    colors.normalColor = (Color)colorTable["settingsBtn"];
                    settingsBtn.colors = colors;
                    break;
                case 2:             //play
                    colors = playBtn.colors;
                    colors.normalColor = SelectColor;
                    playBtn.colors = colors;
                    colors = calibrateBtn.colors;
                    colors.normalColor = (Color)colorTable["calibrateBtn"];
                    calibrateBtn.colors = colors;
                    colors = settingsBtn.colors;
                    colors.normalColor = (Color)colorTable["settingsBtn"];
                    settingsBtn.colors = colors;
                    break;
            }
        }
    }
    float GetAxisKeys(bool isVertical)
    {
        if (isVertical)
        {
            //get for w/s and up/down
            float value = 0;
            bool upActive = false;
            //w
            if (managerMain.GetKeyDown(keyBinding.buttons[3])) {
                value = value + 1;
                upActive = true;
            }
            //s
            if (managerMain.GetKeyDown(keyBinding.buttons[4]))
            {
                value = value - 1;
                upActive = true;
            }
            //up
            if (managerMain.GetKeyDown(keyBinding.buttons[7]) && !upActive)
            {
                value = value + 1;
            }
            //down
            if (managerMain.GetKeyDown(keyBinding.buttons[8]) && !upActive)
            {
                value = value - 1;
            }
            return value;
        }
        else
        {
            //get for a/d and left/right
            float value = 0;
            bool upActive = false;
            //a
            if (managerMain.GetKeyDown(keyBinding.buttons[5]))
            {
                value = value + 1;
                upActive = true;
            }
            //d
            if (managerMain.GetKeyDown(keyBinding.buttons[6]))
            {
                value = value - 1;
                upActive = true;
            }
            //left
            if (managerMain.GetKeyDown(keyBinding.buttons[9]) && !upActive)
            {
                value = value + 1;
            }
            //right
            if (managerMain.GetKeyDown(keyBinding.buttons[10]) && !upActive)
            {
                value = value - 1;
            }
            return value;
        }
    }
    IEnumerator moveWait() {
        canMove = false;
        yield return new WaitForSeconds(0.35f);
        canMove = true;
    }
}