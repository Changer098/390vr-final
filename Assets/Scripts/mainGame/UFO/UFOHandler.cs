using UnityEngine;
using System.Collections;

public class UFOHandler : MonoBehaviour {

    // Use this for initialization
    public Camera OVRCamera;
    public Vector3 angleRotation;
    public float forceLeft;
    public float forceUp;
    bool canMove = false;
    Rigidbody rigid;
    public GameObject hitTarget;

    private GameObject instantiatedTarget;
	void Start () {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(holdPosition());
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
	
	public void updatePosition(float thumbUp, float thumbLeft) {
        //thumbUp is the Left Thumbstick Vertical, thumbLeft is Left Thumbstick Horizontal
        //handle dampening of input variables
        float dampenedUp = 0, dampenedLeft = 0, threshold = 0.1f;
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
        //debug variables
        forceLeft = dampenedLeft;
        forceUp = dampenedUp;
        //rigid.AddForce(gameObject.transform.forward * dampenedUp);
        //rigid.AddForce(gameObject.transform.right * -1 * dampenedLeft);

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
}
