using UnityEngine;
using PurdueVR.InputManager;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class InputHandler : MonoBehaviour {

    public GameObject pauseCanvas;
    public GameObject endCanvas;
    public GameObject pureBlackObject;
    public Button calibrateBtn;
    public Button quitBtn;
    public Button returnBtn;

    public Button calibrateEndBtn;
    public Button quitEndBtn;
    public Button replayBtn;

    public Color SelectColor;                                   //Selection Color of UI Elements
    public GameObject UFOobject;

    public bool gameIsPaused = false;
    public bool gameIsEnded = false;
    private bool canMove = true;
    private bool canPause = true;
    private bool transitioning = false;
    private UFOHandler ufoHandler;

    private Hashtable colorTable = new Hashtable();         //original colors
    private Hashtable colorTableEnd = new Hashtable();      //original colors for the end Menu
    public int index = 0;
    public int endIndex = 0;
    public bool waiterFader;
    private bool wonGame;

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
    InputAxis RightUp = new InputAxis("RightUp", "", XboxAxis.RightStickY);

    InputAxis LeftTrigger = new InputAxis("LeftTrigger", "", XboxAxis.LeftTrigger);
    InputAxis RightTrigger = new InputAxis("RightTrigger", "", XboxAxis.RightTrigger);
    // Use this for initialization
    void Start() {
        Time.timeScale = 1;
        if (waiterFader) StartCoroutine(screenFadeIn());
        colorTable.Add("calibrateBtn", calibrateBtn.colors.normalColor);
        colorTable.Add("quitBtn", quitBtn.colors.normalColor);
        colorTable.Add("returnBtn", returnBtn.colors.normalColor);
        colorTableEnd.Add("calibrateEndBtn", calibrateEndBtn.colors.normalColor);
        colorTableEnd.Add("quitEndBtn", quitEndBtn.colors.normalColor);
        colorTableEnd.Add("replayBtn", replayBtn.colors.normalColor);

        ufoHandler = UFOobject.GetComponent<UFOHandler>();
        HUDInfo.addTriggers();
    }

    // Update is called once per frame
    void Update() {
        //handle pausing 
        if (gameIsPaused && !transitioning) {
            //handle pause navigation, literally just up and down
            //handle escape via pause buttons
            //Don't use guide button because windows 10
            if (managerMain.GetKeyDown(BackBtn) || managerMain.GetKeyDown(StartBtn)) {
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
        else if (!transitioning && gameIsEnded) {
            //handle navigation
            if (managerMain.GetAxis(LeftUp) < -0.5f && canMove) {
                Debug.Log("Left Stick Up");
                //left thumb stick went up
                switch (endIndex) {
                    case 0:
                        endIndex++;
                        handleColors(endIndex, true);
                        break;
                    case 1:
                        endIndex++;
                        handleColors(endIndex, true);
                        break;
                }
                StartCoroutine(moveWait());
            }
            if (managerMain.GetAxis(LeftUp) > 0.5f && canMove) {
                Debug.Log("Left Stick Down");
                //left thumb stick went down
                switch (endIndex) {
                    case 1:
                        endIndex--;
                        handleColors(endIndex, true);
                        break;
                    case 2:
                        endIndex--;
                        handleColors(endIndex, true);
                        break;
                }
                StartCoroutine(moveWait());
            }
            if (managerMain.GetKeyDown(ABtn)) {
                Debug.Log("Clicked Select Button");
                switch (endIndex) {
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
                        //ReplayBtn
                        StartCoroutine(reload());
                        break;
                }
            }
        }
        else if (!transitioning) {
            if ((managerMain.GetKeyDown(BackBtn) || managerMain.GetKeyDown(StartBtn)) && canPause) {
                Debug.Log("Showing pause");
                showPause();
            }
            if (ufoHandler.canMoveUFO()) {
                //do move UFO
                ufoHandler.updatePosition(managerMain.GetAxis(LeftUp), managerMain.GetAxis(LeftSide), managerMain.GetAxis(RightUp));
            }
            if (managerMain.GetKeyDown(ABtn)) {
                //fire mechanism
                ufoHandler.fireA();
            }
            if (managerMain.GetKey(BBtn)) {
                ufoHandler.fireB();
            }
            if (managerMain.GetKeyDown(XBtn)) {
                ufoHandler.fireX();
            }
            if (managerMain.GetKeyDown(YBtn)) {
                ufoHandler.fireY();
            }
            if (managerMain.GetAxis(LeftTrigger) > 0.5f) {
                ufoHandler.leftTrigger();
            }
            if (managerMain.GetAxis(RightTrigger) > 0.5f) {
                ufoHandler.rightTrigger();
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
    public void endGame(bool won) {
        wonGame = won;
        gameIsEnded = true;
        endCanvas.GetComponent<endMenu>().status(won, HUDInfo.destruction);
        endCanvas.SetActive(true);
        Time.timeScale = 0.01f;
        handleColors(0, false);
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
    void handleColors(int index, bool doNothing) {
        ColorBlock colors;
        switch (endIndex) {
            case 0:
                //quitBtn
                colors = calibrateEndBtn.colors;
                colors.normalColor = (Color)colorTableEnd["calibrateEndBtn"];
                calibrateEndBtn.colors = colors;
                colors = replayBtn.colors;
                colors.normalColor = (Color)colorTableEnd["replayBtn"];
                replayBtn.colors = colors;
                //set quit to selectColor
                colors = quitEndBtn.colors;
                colors.normalColor = SelectColor;
                quitEndBtn.colors = colors;
                break;
            case 1:
                //calibrateBtn                
                colors = quitEndBtn.colors;
                colors.normalColor = (Color)colorTableEnd["quitEndBtn"];
                quitEndBtn.colors = colors;
                colors = replayBtn.colors;
                colors.normalColor = (Color)colorTableEnd["replayBtn"];
                replayBtn.colors = colors;
                //set calibrate to selectColor
                colors = calibrateEndBtn.colors;
                colors.normalColor = SelectColor;
                calibrateEndBtn.colors = colors;
                break;
            case 2:
                //returnBtn
                colors = calibrateEndBtn.colors;
                colors.normalColor = (Color)colorTableEnd["calibrateEndBtn"];
                calibrateEndBtn.colors = colors;
                colors = quitEndBtn.colors;
                colors.normalColor = (Color)colorTableEnd["quitEndBtn"];
                quitEndBtn.colors = colors;
                //set return to selectColor
                colors = replayBtn.colors;
                colors.normalColor = SelectColor;
                replayBtn.colors = colors;
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
    IEnumerator reload() {
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
        SceneManager.LoadScene(2);
    }
    IEnumerator screenFadeIn()
    {
        transitioning = true;
        Renderer render = pureBlackObject.GetComponent<Renderer>();
        pureBlackObject.SetActive(true);

        yield return new WaitForSeconds(30);
        float waitTime = 0.025f;
        float opacity = 1;
        Debug.Log("fading in");
        while (opacity > 0)
        {
            Color c = render.material.color;
            opacity = opacity - ((float)10 / (float)255);
            c.a = opacity;
            render.material.color = c;
            float start = Time.unscaledTime;
            while (Time.unscaledTime < start + waitTime)
            {
                yield return null;
            }
        }

        Debug.Log("Not fading");
        transitioning = false;
        pureBlackObject.SetActive(false);
    }
}
