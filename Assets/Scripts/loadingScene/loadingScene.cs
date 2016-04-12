using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class loadingScene : MonoBehaviour {

    AsyncOperation loader;
    GameObject OVRCameraRig;
    GameObject pureBlackObject;
    bool transitioning = false;                                                     //change scenes
    bool fading = false;
    bool changingColor = false;
    bool isDone = false;
    Random rand;
    [SerializeField]Text loading;
    [SerializeField]GameObject TransferObjects;

	void Start () {
        Debug.Log("Loaded scene");
        OVRCameraRig = GameObject.Find("OVRCameraRig");                             //find the rig from the last scene, we need to undo the screen fade
        pureBlackObject = GameObject.Find("blackBox");
        StartCoroutine(unFade());
        rand = new Random();
        loader = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
        loader.allowSceneActivation = false;
        StartCoroutine(colorChange());
        transferObjects tObjects = TransferObjects.GetComponent<transferObjects>();                 //get the transferObjects script from the TransferObjects gameObject
        tObjects.setCamera(OVRCameraRig);
	}
    void Update() {
        if (isDone) {
            //do fade out
            if (!transitioning && !fading) {
                Debug.Log("Scene Management has finished loading the scene!");
                transitioning = true;
                StartCoroutine(dissapearText());
                //dissapearText automatically triggers FadeAndLoad which sets the loaded scene as active
            }
        }
        if (loader.progress == 0.9f && !isDone) {
            //for some reason, loader gets stuck at 0.9, we must manually tell it to finish
            Debug.Log("is finished");
            isDone = true;
        }
    }
    IEnumerator colorChange() {
        int iterations = 0;
        while (!isDone) {
            changingColor = true;
            if (iterations == 0) {
                Color c = loading.color;
                c.a = Random.Range(0.5f, 1);
                loading.color = c;
                iterations++;
            }
            else {
                if (iterations == 5) {
                    iterations = 0;
                    continue;
                }
                else {
                    iterations++;
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        changingColor = false;
    }
    IEnumerator dissapearText() {
        //fade out text color
        float value = loading.color.a;
        Color c = loading.color;
        float waitInterval = 0.01f;
        while (value >= 0) {
            value = value - 0.025f;
            c.a = value;
            loading.color = c;
            yield return new WaitForSeconds(waitInterval);
        }
        c.a = Mathf.Clamp(value, 0, 1);
        Debug.Log("makeActive(): finished");
        StartCoroutine(FadeAndLoad());
    }
    IEnumerator unFade() {
        fading = true;
        Renderer render = pureBlackObject.GetComponent<Renderer>();
        pureBlackObject.SetActive(true);

        float waitTime = 0.025f;
        float opacity = 1;
        while (opacity > 0) {
            Color c = render.material.color;
            opacity = opacity - ((float)10 / (float)255);
            c.a = opacity;
            render.material.color = c;
            yield return new WaitForSeconds(waitTime);
        }

        fading = false;
        pureBlackObject.SetActive(false);
        Debug.Log("screenFade finished");
    }
    IEnumerator FadeAndLoad() {
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
        Debug.Log("screenFade finished, set game as active scene");
        //loader will not actually finish and that's fine, so we just set it automatically activate.
        loader.allowSceneActivation = true;
    }
}
