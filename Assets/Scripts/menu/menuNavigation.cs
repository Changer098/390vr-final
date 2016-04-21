using UnityEngine;
using PurdueVR.InputManager;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class menuNavigation : MonoBehaviour {

    // Use this for initialization
    int index = -1;
    //int secondaryIndex = -1;
    public Canvas mainCanvas;
    //public Canvas settingsCanvas;
    
    //mainCanvas elements
    public Button playBtn;
    public Button quitBtn;
    public Button calibrateBtn;

    //settingsCanvas elements
    public Button backBtn;
    public Toggle controllerToggle;
    public Toggle VRToggle;

    public Color SelectColor;                                   //Selection Color of UI Elements
    public GameObject pureBlackObject;
    public GameObject OVRCameraRig;

    private bool transitioning = false;                         //whether or not the screen is transitioning to another canvas or fading out. Disallows input
    bool canMove = true;                                        //Used by moveWait() to prevent multiple controller inputs

    private Hashtable colorTable = new Hashtable();  //original colors
	void Start () {
        index = 0;
        //secondaryIndex = -1;

        //Create a hashtable of the original UIElement Colors. We change the color of the Element when it's active, else we set it to its original color
        colorTable.Add("playBtn", playBtn.colors.normalColor);
        colorTable.Add("quitBtn", quitBtn.colors.normalColor);
        colorTable.Add("calibrateBtn", calibrateBtn.colors.normalColor);
        colorTable.Add("backBtn", backBtn.colors.normalColor);
        colorTable.Add("controllerToggle", controllerToggle.colors.normalColor);
        colorTable.Add("VRToggle", VRToggle.colors.normalColor);

        //DontDestroyOnLoad(OVRCameraRig);
        //DontDestroyOnLoad(pureBlackObject);
        selectObject();        
	}
	
	// Update is called once per frame
	void Update () {
        if (!transitioning) {
            if (managerMain.GetKeyDown(keyBinding.buttons[0])) {
                //select button pressed
                if (index != -1) {
                    //mainCanvas
                    switch (index) {
                        case 0:
                            //quitBtn
                            Debug.Log("Pressed Quit Btn");
                            Application.Quit();
                            break;
                        case 1:
                            //calibrateBtn
                            Debug.Log("Pressed Calibrate Btn");
                            UnityEngine.VR.InputTracking.Recenter();
                            break;
                        case 2:
                            //playBtn
                            Debug.Log("Pressed Play Btn");
                            StartCoroutine(screenFadeOutAndLoad());
                            break;
                    }
                }
            }
            //Don't call get axis without controller
            if ((managerMain.currentInput.isXbox && managerMain.GetAxis(keyBinding.buttons[1]) != 0) || (!managerMain.currentInput.isXbox && GetAxisKeys(true) != 0)) {
                //vertical axis activated
                    //in the mainCanvas
                    if (managerMain.currentInput.isXbox) {
                        switch (index) {
                            case 0:
                                if (managerMain.GetAxis(keyBinding.buttons[1]) < -0.4 && canMove) {
                                    //quitBtn, should navigate to playBtn
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
                                //playBtn, should navigate to quitBtn
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
                                if (GetAxisKeys(true) < 0) {
                                    //quitBtn, should navigate to playBtn
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
                                //playBtn, should navigate to quitBtn
                                index = 0;
                                    selectObject();
                                }
                                break;
                        }
                    }

            }
            else if ((managerMain.currentInput.isXbox && managerMain.GetAxis(keyBinding.buttons[2]) != 0) || (!managerMain.currentInput.isXbox && GetAxisKeys(false) != 0)) {
                //horizontal axis activated
                    if (managerMain.currentInput.isXbox) {
                        //opposite direction
                        switch (index) {
                            case 0:
                                if (managerMain.GetAxis(keyBinding.buttons[2]) == 1 && canMove) {
                                    //quitBtn, should navigate to calibrateBtn
                                    index = 1;
                                    selectObject();
                                    StartCoroutine(moveWait());
                                }
                                break;
                            case 1:
                                if (managerMain.GetAxis(keyBinding.buttons[2]) == -1 && canMove) {
                                    //calibrateBtn, should navigate to quitBtn
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
                                    //quitBtn, should navigate to calibrateBtn
                                    index = 1;
                                    selectObject();
                                }
                                break;
                            case 1:
                                if (GetAxisKeys(false) > 0) {
                                    //calibrateBtn, should navigate to quitBtn
                                    index = 0;
                                    selectObject();
                                }
                                break;
                        }
                    }
            }
        }
	}

    void selectObject() {
            //modify index colors
            ColorBlock colors;
            switch(index) {
                case 0:             //quit
                    colors = quitBtn.colors;
                    colors.normalColor = SelectColor;
                quitBtn.colors = colors;
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
                quitBtn.colors = colors;
                    colors = quitBtn.colors;
                    colors.normalColor = (Color)colorTable["quitBtn"];
                quitBtn.colors = colors;
                    break;
                case 2:             //play
                    colors = playBtn.colors;
                    colors.normalColor = SelectColor;
                    playBtn.colors = colors;
                    colors = calibrateBtn.colors;
                    colors.normalColor = (Color)colorTable["calibrateBtn"];
                    calibrateBtn.colors = colors;
                    colors = quitBtn.colors;
                    colors.normalColor = (Color)colorTable["quitBtn"];
                quitBtn.colors = colors;
                    break;
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
    //Disallows controller input for 0.35 seconds. Helps prevent multiple input events, like holding a key for an extended period of time
    IEnumerator moveWait() {
        canMove = false;
        yield return new WaitForSeconds(0.35f);
        canMove = true;
    }
    //Fades out the screen, loads the loading scene, and then switches to said scene
    IEnumerator screenFadeOutAndLoad() {

        Renderer render = pureBlackObject.GetComponent<Renderer>();
        pureBlackObject.SetActive(true);

        float waitTime = 0.025f;
        float opacity = 0;
        while (opacity < 1) {
            Color c = render.material.color;
            opacity = opacity + ((float)10 / (float)255);
            c.a = opacity;
            render.material.color = c;
            yield return new WaitForSeconds(waitTime);
        }
        Debug.Log("screenFade finished");
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

}
