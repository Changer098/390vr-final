using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class endMenu : MonoBehaviour {

    // Use this for initialization
    public Text endText;
    public Text scoreText;

    public void status(bool won, int score) {
        scoreText.text = scoreText.text + score.ToString();
        if (won) {
            endText.text = "You won!";
        }
        else {
            endText.text = "You lost";
        }
    }
}
