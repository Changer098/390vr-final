using UnityEngine;
using PurdueVR.InputManager;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class InputHandler : MonoBehaviour {

    public GameObject pauseCanvas;
    public GameObject pureBlackObject;
    public Button calibrateBtn;
    public Button quitBtn;
    public Button returnBtn;
    public Color SelectColor;                                   //Selection Color of UI Elements
    public GameObject UFOobject;

    public bool gameIsPaused = false;
    private bool canMove = true;
    private bool canPause = true;
    private bool transitioning = false;
    private UFOHandler ufoHandler;

    private Hashtable colorTable = new Hashtable();  //original colors
    private int index = 0;

    //Input mapping
    InputKey ABtn = new InputKey("A Btn", KeyCode.A, XboxKey.A);
    InputKey BBtn = new InputKey("B Btn", KeyCode.B, XboxKey.B);
    InputKey XBtn = new InputKey("X Btn", KeyCode.X, XboxKey.X);
    InputKey YBtn = new InputKey("Y Btn", KeyCode.Y, XboxKey.Y);
    InputKey GuideBtn = new InputKey("Guide Btn", KeyCode.Escape, XboxKey.Guide);
    InputKey BackBtn = new InputKey("Back Btn", KeyCode.Escape, XboxKey.Back);
    InputKey StartBtn = new InputKey("Start Btn", KeyCode.Escape, XboxKey.Start);

    InputAxis LeftUp = new InputAxis("LeftUp", "Mouse Y", XboxAxis.LeftStickY);
    InputAxis LeftSide = new InputAxis("LeftSide", "Mouse X", XboxAxis.LeftStickX);
    // Use this for initialization
    void Start() {
        SceneManager.UnloadScene(1);
        colorTable.Add("calibrateBtn", calibrateBtn.colors.normalColor);
        colorTable.Add("quitBtn", quitBtn.colors.normalColor);
        colorTable.Add("returnBtn", returnBtn.colors.normalColor);
        ufoHandler = UFOobject.GetComponent<UFOHandler>();
    }

    // Update is called once per frame
    void Update() {
        //handle pausing 
        if (gameIsPaused && !transitioning) {
            //handle pause navigation, literally just up and down
            //handle escape via pause buttons
            if (managerMain.GetKeyDown(GuideBtn) || managerMain.GetKeyDown(BackBtn) || managerMain.GetKeyDown(StartBtn)) {
                Debug.Log("pressed pause button");
                hidePause();
            }
            //handle navigation
            if (managerMain.GetAxis(LeftUp) < -0.5f && canMove) {
                Debug.Log("Left Stick Up");
                //left thumb stick went up
                switch (index) {
                    case 0:
                        index++;
                        handleColors(index);
                        break;
                    case 1:
                        index++;
                        handleColors(index);
                        break;
                }
                StartCoroutine(moveWait());
            }
            if (managerMain.GetAxis (LeftUp) > 0.5f && canMove) {
                Debug.Log("Left Stick Down");
                //left thumb stick went down
                switch (index) {
                    case 1:
                        index--;
                        handleColors(index);
                        break;
                    case 2:
                        index--;
                        handleColors(index);
                        break;
                }
                StartCoroutine(moveWait());
            }
            if (managerMain.GetKeyDown(ABtn)) {
                Debug.Log("Clicked Select Button");
                switch(index) {
                    case 0:
                        //quitbtn
                        fromMainGame.isFromMainGame = true;
                        StartCoroutine(screenFadeOutAndLoad());
                        break;
                    case 1:
                        //calibrateBtn
                        UnityEngine.VR.InputTracking.Recenter();
                        break;
                    case 2:
                        //returnBtn
                        hidePause();
                        break;
                }
            }
        }
        else if (!transitioning) {
            if ((managerMain.GetKeyDown(GuideBtn) || managerMain.GetKeyDown(BackBtn) || managerMain.GetKeyDown(StartBtn)) && canPause) {
                Debug.Log("Showing pause");
                showPause();
            }
            if (ufoHandler.canMoveUFO()) {
                //do move UFO
                ufoHandler.updatePosition(managerMain.GetAxis(LeftUp), managerMain.GetAxis(LeftSide));
            }
        }
    }

    void hidePause() {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
        StartCoroutine(pauseWait());
    }
    void showPause() {
        gameIsPaused = true;
        index = 0;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0.01f;
        handleColors(0);
    }
    void handleColors(int index) {
        ColorBlock colors;
        switch (index) {
            case 0:
                //quitBtn
                colors = calibrateBtn.colors;
                colors.normalColor = (Color)colorTable["calibrateBtn"];
                calibrateBtn.colors = colors;
                colors = returnBtn.colors;
                colors.normalColor = (Color)colorTable["returnBtn"];
                returnBtn.colors = colors;
                //set quit to selectColor
                colors = quitBtn.colors;
                colors.normalColor = SelectColor;
                quitBtn.colors = colors;
                break;
            case 1:
                //calibrateBtn                
                colors = quitBtn.colors;
                colors.normalColor = (Color)colorTable["quitBtn"];
                quitBtn.colors = colors;
                colors = returnBtn.colors;
                colors.normalColor = (Color)colorTable["returnBtn"];
                returnBtn.colors = colors;
                //set calibrate to selectColor
                colors = calibrateBtn.colors;
                colors.normalColor = SelectColor;
                calibrateBtn.colors = colors;
                break;
            case 2:
                //returnBtn
                colors = calibrateBtn.colors;
                colors.normalColor = (Color)colorTable["calibrateBtn"];
                calibrateBtn.colors = colors;
                colors = returnBtn.colors;
                colors = quitBtn.colors;
                colors.normalColor = (Color)colorTable["quitBtn"];
                quitBtn.colors = colors;
                colors = returnBtn.colors;
                //set return to selectColor
                colors = returnBtn.colors;
                colors.normalColor = SelectColor;
                returnBtn.colors = colors;
                break;
        }
    }
    IEnumerator moveWait() {
        canMove = false;
        //yield return new WaitForSeconds(0.35f);
        //
        // Would just like to take the time to highlight how stupid Unity's waitForSecond event is.
        //
        float start = Time.unscaledTime;
        while (Time.unscaledTime < start + 0.35f) {
            yield return null;
        }
        canMove = true;
    }
    //So we don't unpause and then pause
    IEnumerator pauseWait() {
        canPause = false;
        yield return new WaitForSeconds(0.1f);
        canPause = true;
    }
    IEnumerator screenFadeOutAndLoad() {
        //begin loading loader level
        AsyncOperation loader;
        loader = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        loader.allowSceneActivation = false;

        Renderer render = pureBlackObject.GetComponent<Renderer>();
        pureBlackObject.SetActive(true);

        float waitTime = 0.025f;
        float opacity = 0;
        while (opacity < 1) {
            Color c = render.material.color;
            opacity = opacity + ((float)10 / (float)255);
            c.a = opacity;
            render.material.color = c;
            float start = Time.unscaledTime;
            while (Time.unscaledTime < start + waitTime) {
                yield return null;
            }
        }
        Debug.Log("screenFade finished");
        //loader will not actually finish and that's fine, so we just set it automatically activate.
        Time.timeScale = 1;
        loader.allowSceneActivation = true;
        Debug.Log("load progress: " + loader.progress);
    }
}
