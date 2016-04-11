using UnityEngine;
using PurdueVR.InputManager;
using System.Collections;

public class menuNavigation : MonoBehaviour {

    // Use this for initialization
    int index = -1;
	void Start () {
        index = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    if (managerMain.GetKeyDown(keyBinding.buttons[0]))
        {
            //select button pressed

        }
        if (managerMain.GetAxis(keyBinding.buttons[1] || ()) {
            //

        }
        else if(managerMain.GetAxis(keyBinding.buttons[2]) || ())
        {
            //horizontal axis activated

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
}
