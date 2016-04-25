using UnityEngine;
using System.Collections;

public class endMenuTest : MonoBehaviour {

    public bool gameLost;
    public bool gameWon;
    private bool alreadySet = false;
    public GameObject InputManager; 
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        if (gameLost && !alreadySet) {
            InputManager.GetComponent<InputHandler>().endGame(false);
            gameLost = false;
        }
        else if (gameWon && !alreadySet) {
            InputManager.GetComponent<InputHandler>().endGame(true);
            gameWon = false;
        }
        else if (alreadySet) {
            gameLost = false;
            gameWon = false;
        }
	}
}
