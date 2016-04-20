using UnityEngine;
using System.Collections;

public class UFOHandler : MonoBehaviour {

    // Use this for initialization
    public Camera OVRCamera;
    public Vector3 angleRotation;
    public float forceLeft;
    public float forceUp;
    public float multiplier = 2.5f;
    bool canMove = false;
    Rigidbody rigid;
    public GameObject hitTarget;
    public GameObject abductRays;

    private GameObject instantiatedTarget;

    bool unlockedA = false;
    bool unlockedB = false;
    bool unlockedX = false;
    bool unlockedY = false;

    //Abduct Rays values
    private bool keepAbductAlive = false;
	void Start () {
        moveCamera.isAlive = true;
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(holdPosition());
        StartCoroutine(abducter());
	}
    void FixedUpdate() {
        //gameObject.transform.rotation = OVRCamera.transform.rotation;
        angleRotation = gameObject.transform.rotation.eulerAngles;
        angleRotation.y = OVRCamera.transform.rotation.eulerAngles.y;
        //gameObject.transform.rotation.eulerAngles.Set(angleRotation.x, angleRotation.y, angleRotation.z);
        //gameObject.transform.rotation = Quaternion.Euler(angleRotation);
        //apply force to maintain UFO position
        rigid.AddForce(Vector3.up * rigid.mass * 9.81f);
    }
	
	public void updatePosition(float thumbUp, float thumbLeft, float leftThumbVertical) {
        //thumbUp is the Left Thumbstick Vertical, thumbLeft is Left Thumbstick Horizontal
        //handle dampening of input variables
        float dampenedUp = 0, dampenedLeft = 0, dampenedUpLeft = 0, threshold = 0.1f;
        if (thumbUp > 0) {
            if (thumbUp < threshold) dampenedUp = 0;
            else dampenedUp = thumbUp;
        }
        else if (thumbUp < 0) {
            if (thumbUp > (-1 * threshold)) dampenedUp = 0;
            else dampenedUp = thumbUp;
        }
        if (thumbLeft > 0) {
            if (thumbLeft < threshold) dampenedLeft = 0;
            else dampenedLeft = thumbLeft;
        }
        else if (thumbLeft < 0) {
            if (thumbLeft > (-1 * threshold)) dampenedLeft = 0;
            else dampenedLeft = thumbLeft;
        }
        if (leftThumbVertical > 0) {
            if (leftThumbVertical < threshold) dampenedUpLeft = 0;
            else dampenedUpLeft = leftThumbVertical;
        }
        else if (leftThumbVertical < 0) {
            if (leftThumbVertical > (-1 * threshold)) dampenedUpLeft = 0;
            else dampenedUpLeft = leftThumbVertical;
        }
        //debug variables
        forceLeft = dampenedLeft;
        forceUp = dampenedUp;
        rigid.AddForce(gameObject.transform.forward * -1 * dampenedUp * multiplier);
        rigid.AddForce(gameObject.transform.right * -1 * dampenedLeft * multiplier);

        //Add Vertical force
        rigid.AddForce(Vector3.up * rigid.mass * dampenedUpLeft);

        //Cast Target Object
        Ray rayfire = new Ray(OVRCamera.transform.position, OVRCamera.transform.forward);
        RaycastHit rayHit;
        //10000000011 in base 10
        int mask = 1027;
        if (Physics.Raycast(rayfire, out rayHit, 15, mask, QueryTriggerInteraction.Ignore)) {
            if (rayHit.collider.gameObject.layer == 8) {
                Debug.Log("Hit UFO");
            }
            if (hitTarget == null) {
                Instantiate(hitTarget, rayHit.point, Quaternion.identity);
            }
            else {
                hitTarget.transform.position = rayHit.point;
            }
        }
    }
    public bool canMoveUFO() {
        //do checks if intro animation has completed
        return canMove;
    }
    public void destroyUFO() {
        //handle destruction of UFO
        moveCamera.isAlive = false;
    }
    public bool destructionUpdate(float value) {
        //If destruction is enough for an upgrade, upgrade weapon and return true. Else return false
        if (!unlockedA) {
            if (value >= 1) {
                unlockedA = true;
                HUDInfo.AddButton(XboxKey.A, "weaponA", 0.75f, 20);
                return true;
            }
            return false;
        }
        else if (!unlockedB) {
            if (value >= 1) {
                unlockedB = true;
                HUDInfo.AddButton(XboxKey.B, "weaponB", 0.75f, 10);
                return true;
            }
            return false;
        }
        else if (!unlockedX) {
            if (value >= 1) {
                unlockedX = true;
                HUDInfo.AddButton(XboxKey.X, "weaponX", 0.75f, 1);
                return true;
            }
            return false;
        }
        else if (!unlockedY) {
            if (value >= 1) {
                unlockedY = true;
                HUDInfo.AddButton(XboxKey.Y, "weaponY", 0.75f, 5);
                return true;
            }
            return false;
        }
        else {
            //can't upgrade any more, everything is unlocked. return false
            return false;
        }
    }
    public bool healthUpdate(float value) {
        //If health is 0, destroyUFO and return true. Else false
        if (value <= 0) {
            destroyUFO();
            return true;
        }
        return false;
    }
    public int getUpgradeLevel() {
        if (!unlockedA) return 1;
        else if (!unlockedB) return 2;
        else if (!unlockedX) return 3;
        else if (!unlockedY) return 4;
        else return 5;
    }

    //hold the position while axis values reset
    IEnumerator holdPosition() {
        Vector3 position = rigid.position;
        float start = Time.unscaledTime;
        while (Time.unscaledTime < start + 0.5f) {
            rigid.position = position;
            yield return null;
        }
        canMove = true;
        Debug.Log("rigidBody is free");
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

    }
    public void fireX() {

    }
    public void fireY() {

    }
    public void rightTrigger() {
        if (HUDInfo.getAble(XboxAxis.RightTrigger)) {
            Debug.Log("Firing Right Trigger!");
            int ammoCount = HUDInfo.getAmmo(XboxAxis.RightTrigger);
            
            if (ammoCount > 0) {
                Debug.Log("Ammo: " + ammoCount);
                ammoCount = HUDInfo.setAmmo(XboxAxis.RightTrigger, ammoCount - 1);
                Debug.Log("setAmmo(): " + ammoCount);
                //Do fire


                if (HUDInfo.getAmmo(XboxAxis.RightTrigger) == 0) {
                    HUDInfo.callReload(XboxAxis.RightTrigger);
                }
            }
            else {
                Debug.Log("Cannot fire, getAmmo <= 0, is " + ammoCount);
            }
        }
    }
    public void leftTrigger() {
        if (HUDInfo.getAble(XboxAxis.LeftTrigger)) {
            Debug.Log("Firing LeftTrigger!");
            int ammoCount = HUDInfo.getAmmo(XboxAxis.LeftTrigger);

            if (ammoCount > 0) {
                Debug.Log("Ammo: " + ammoCount);
                ammoCount = HUDInfo.setAmmo(XboxAxis.LeftTrigger, ammoCount - 1);
                Debug.Log("setAmmo(): " + ammoCount);
                //Do fire
                keepAbductAlive = true;

                if (HUDInfo.getAmmo(XboxAxis.LeftTrigger) == 0) {
                    HUDInfo.callReload(XboxAxis.LeftTrigger);
                }
            }
            else {
                Debug.Log("Cannot fire, getAmmo <= 0, is " + ammoCount);
            }
        }
    }
    IEnumerator abducter() {
        bool justSet = false;
        while (true) {
            if (keepAbductAlive) {
                abductRays.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                keepAbductAlive = false;
                justSet = true;
            }
            else if (!keepAbductAlive && !justSet) {
                abductRays.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
            else if (!keepAbductAlive && justSet) {
                yield return new WaitForSeconds(0.25f);
                justSet = false;
            }
        }
    }
}
