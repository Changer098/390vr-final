using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class loadingScene : MonoBehaviour {

    AsyncOperation loader;
    bool settingScene = false;
    bool changingColor = false;
    bool isDone = false;
    Random rand;
    [SerializeField]Text loading;

	void Start () {
        rand = new Random();
        loader = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        loader.allowSceneActivation = false;
        StartCoroutine(colorChange());
        StartCoroutine(progress());
	}
    void Update() {
        if (loader.isDone) {
            //do fade out
            if (!settingScene) {
                Debug.Log("Scene Management has finished loading the scene!");
                settingScene = true;
                StartCoroutine(makeActive());
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
        StartCoroutine(makeActive());
    }
    IEnumerator makeActive() {
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
        //Set loaded scene as active scene
        Debug.Log("makeActive(): finished");
        loader.allowSceneActivation = true;
        //SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
    }
    IEnumerator progress() {
        while (!isDone) {
            Debug.Log("progress: " + loader.progress);
            yield return new WaitForSeconds(1);
        }
    }
}
