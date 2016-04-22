using UnityEngine;
using System.Collections;
using System;

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
    //public GameObject bullet;
    public GameObject singleBeam;               //the prefab of the first beam we instantiate (Right Trigger)
    public GameObject constBeam;                //the prefab of the constantly on beam (B Btn)
    public BeamParam ourBeamParam;              //The param of the first instantiated beam (Right Trigger)
    public BeamParam secondBeamParam;           //The param of the second instantiated beam (A Btn)
    public BeamParam constBeamParam;            //The param of the third instantiated, first constant beam (B Btn)
    public GameObject fireLocation;             //The location for the first beam and a beam on A btn
    public GameObject fireLocation2;            //The location of the second beam created on A btn
    public float distanceThreshold;             //Value that determines how high the UFO can go

    private GameObject instantiatedTarget;
    private hitTarget targetScript;
    RaycastHit rayHit;

    bool unlockedA = false;
    bool canFireA = true;
    bool unlockedB = false;
    bool canFireB = true;
    bool unlockedX = false;
    bool canFireX = true;
    bool unlockedY = false;
    bool canFireY = true;

    bool canFireRight = true;

    //public Vector3 PrintingIsDumb;

    //Abduct Rays values
    private bool keepAbductAlive = false;
    //Const Beam value
    private bool keepConstBeamAlive = false;

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
        float dampenedUp = 0, dampenedLeft = 0, dampenedRight = 0, threshold = 0.1f;
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
            if (leftThumbVertical < threshold) dampenedRight = 0;
            else dampenedRight = leftThumbVertical;
        }
        else if (leftThumbVertical < 0) {
            if (leftThumbVertical > (-1 * threshold)) dampenedRight = 0;
            else dampenedRight = leftThumbVertical;
        }
        //debug variables
        forceLeft = dampenedLeft;
        forceUp = dampenedUp;
        rigid.AddForce(gameObject.transform.up * dampenedUp * multiplier);
        Debug.DrawLine(gameObject.transform.forward, gameObject.transform.forward * 10);
        rigid.AddForce(gameObject.transform.right * -1 * dampenedLeft * multiplier);

        //Add Vertical force
        if (gameObject.transform.position.y < distanceThreshold) {
            rigid.AddForce(Vector3.up * rigid.mass * dampenedRight);
        }
        else if (dampenedRight < 0) {
            rigid.AddForce(Vector3.up * rigid.mass * dampenedRight);
        }

        //Cast Target Object
        Ray rayfire = new Ray(OVRCamera.transform.position, OVRCamera.transform.forward);
        //10000000011 in base 10
        int mask = 1027;
        if (Physics.Raycast(rayfire, out rayHit, 100, mask, QueryTriggerInteraction.Ignore)) {
            if (rayHit.collider.gameObject.layer == 8) {
                Debug.Log("Hit UFO");
            }
            else {
                if (targetScript == null) {
                    targetScript = hitTarget.GetComponent<hitTarget>();
                    targetScript.colorUpdate(rayHit);
                }
                else {
                    targetScript.colorUpdate(rayHit);
                }
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
                HUDInfo.AddButton(XboxKey.A, "weaponA", 0.75f, 10);
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
        if (HUDInfo.getAble(XboxKey.A) && canFireA) {
            Debug.Log("Firing A!");
            int ammoCount = HUDInfo.getAmmo(XboxKey.A);
            if (ammoCount > 0) {
                GameObject s1 = (GameObject)Instantiate(singleBeam, fireLocation.transform.position, fireLocation.transform.rotation);
                s1.transform.LookAt(rayHit.point);
                s1.GetComponent<BeamParam>().SetBeamParam(ourBeamParam);
                s1.AddComponent<lazerBullet>().setType(1);

                StartCoroutine(fireBeamTwoAndWait(fireLocation2.transform.position, fireLocation2.transform.rotation));

                ammoCount = HUDInfo.setAmmo(XboxKey.A, ammoCount - 2);
                if (HUDInfo.getAmmo(XboxKey.A) == 0) {
                    HUDInfo.callReload(XboxKey.A);
                }
                //StartCoroutine(waitA());
            }
            else {
                Debug.Log("Cannot fire, getAmmo <= 0");
            }
        }
    }
    public void fireB() {
        if (HUDInfo.getAble(XboxKey.B) && canFireB) {
            Debug.Log("Firing B!");
            int ammoCount = HUDInfo.getAmmo(XboxKey.B);

            if (ammoCount > 0) {
                ammoCount = HUDInfo.setAmmo(XboxAxis.LeftTrigger, ammoCount - 1);
                keepConstBeamAlive = true;
            }
        }
    }
    public void fireX() {

    }
    public void fireY() {

    }
    public void rightTrigger() {
        if (HUDInfo.getAble(XboxAxis.RightTrigger) && canFireRight) {
            Debug.Log("Firing Right Trigger!");
            int ammoCount = HUDInfo.getAmmo(XboxAxis.RightTrigger);
            
            if (ammoCount > 0) {
                //Debug.Log("Ammo: " + ammoCount);
                ammoCount = HUDInfo.setAmmo(XboxAxis.RightTrigger, ammoCount - 1);
                //Debug.Log("setAmmo(): " + ammoCount);
                //Do fire
                /*GameObject instantiated = (GameObject)Instantiate(bullet, fireLocation.transform.position, Quaternion.identity);
                Rigidbody instantiatedRigid = instantiated.GetComponent<Rigidbody>();
                //THANKS TO WEI
                instantiatedRigid.AddForce((rayHit.point- instantiated.transform.position).normalized * (float)instantiated.GetComponent<bullet>().getFireForce());*/
                GameObject s1 = (GameObject)Instantiate(singleBeam, fireLocation.transform.position, fireLocation.transform.rotation);
                s1.transform.LookAt(rayHit.point);
                s1.GetComponent<BeamParam>().SetBeamParam(ourBeamParam);
                s1.AddComponent<lazerBullet>().setType(0);


                //instantiatedRigid.AddForceAtPosition(rayHit.point * instantiated.GetComponent<bullet>().getFireForce(), transform.forward);
                // PrintingIsDumb = 

                //wait Fire
                StartCoroutine(waitRight());

                if (HUDInfo.getAmmo(XboxAxis.RightTrigger) <= 0 || ammoCount <= 0) {
                    Debug.Log("calling reload");
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
                //Debug.Log("Ammo: " + ammoCount);
                ammoCount = HUDInfo.setAmmo(XboxAxis.LeftTrigger, ammoCount - 1);
                //Debug.Log("setAmmo(): " + ammoCount);
                //Do fire
                keepAbductAlive = true;

                if (HUDInfo.getAmmo(XboxAxis.LeftTrigger) == 0) {
                    HUDInfo.callReload(XboxAxis.LeftTrigger);
                }
            }
            else {
                Debug.Log("Cannot fire, getAmmo <= 0, is " + HUDInfo.getAmmo(XboxAxis.LeftTrigger));
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
    IEnumerator constBeamer() {
        bool justSet = false;
        GameObject beam = null;
        while (true) {
            if (keepConstBeamAlive) {
                //do instantiation and such
                if (beam == null) {
                    beam = (GameObject)Instantiate(constBeam, fireLocation.transform.position, fireLocation.transform.rotation);
                    beam.transform.LookAt(rayHit.point);
                    beam.GetComponent<BeamParam>().SetBeamParam(constBeamParam);
                    lazerBullet lazBull = beam.AddComponent<lazerBullet>();
                    lazBull.setType(2);
                }
                else {
                    beam.SetActive(true);
                }
                yield return new WaitForSeconds(0.1f);
                keepConstBeamAlive = false;
                justSet = false;
            }
            else if (!keepConstBeamAlive && !justSet) {
                //turn off and such
                if (beam != null) {
                    beam.SetActive(false);
                }
                yield return new WaitForSeconds(0.1f);
            }
            else if (!keepConstBeamAlive && justSet) {
                yield return new WaitForSeconds(0.25f);
                justSet = false;
            }
        }
    }

    //wait a little bit so you don't waste all your ammo
    IEnumerator waitRight() {
        canFireRight = false;
        float waitTime = 0.5f;
        float start = Time.unscaledTime;
        /*while (Time.unscaledTime < start + waitTime) {
            yield return null;
        }*/
        while (Time.unscaledTime < start + waitTime) {
            yield return null;
        }
            
        canFireRight = true;
    }
    IEnumerator fireBeamTwoAndWait(Vector3 position, Quaternion rotation) {
        canFireA = false;
        //wait 10 frames before firing
        for (int i = 0; i < 10; i++) {
            yield return new WaitForEndOfFrame();
        }
        GameObject s2 = (GameObject)Instantiate(singleBeam, position, rotation);
        s2.transform.LookAt(rayHit.point);
        s2.GetComponent<BeamParam>().SetBeamParam(secondBeamParam);
        s2.AddComponent<lazerBullet>().setType(1);


        float waitTime = 1f;
        float start = Time.unscaledTime;
        while (Time.unscaledTime < start + waitTime) {
            yield return null;
        }
        canFireA = true;
    }
}
