using UnityEngine;
using System.Collections;

public class BeamCollision : MonoBehaviour {

    public GameObject HitEffect = null;
    public bool Reflect = false;

	private BeamLine BL;
	private bool bHit = false;
	private BeamParam BP;


    // Use this for initialization
    void Start () {
		BL = (BeamLine)this.gameObject.transform.FindChild("BeamLine").GetComponent<BeamLine>();
		BP = this.transform.root.gameObject.GetComponent<BeamParam>();
	}
	
	// Update is called once per frame
	void Update () {
		//RayCollision
		RaycastHit hit;
        //10000000011 in base 10
        int layerMask = 1027;
        if (HitEffect != null && !bHit && Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            GameObject hitobj = hit.collider.gameObject;
			if(hit.distance < BL.GetNowLength())
		    {
				BL.StopLength(hit.distance);
				bHit = true;

                Quaternion Angle;
                //Reflect to Normal
                if (Reflect)
                {
                    Angle = Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal));
                }
                else
                {
                    Angle = Quaternion.AngleAxis(180.0f, transform.up) * this.transform.rotation;
                }
                GameObject obj = (GameObject)Instantiate(HitEffect,this.transform.position+this.transform.forward*hit.distance,Angle);
				obj.GetComponent<BeamParam>().SetBeamParam(BP);
				obj.transform.localScale = this.transform.localScale;

                //generate collision data
                if (hit.collider.gameObject.layer == 10) {
                    Debug.Log("Hit building");
                    hit.collider.gameObject.GetComponent<building>().CollisionFaker(this.GetComponent<lazerBullet>());
                }
                else {
                    Debug.Log("Hit " + hit.collider.name + ", on layer: " + hit.collider.gameObject.layer);
                }
			}
			//print("find" + hit.collider.gameObject.name);
		}
	}
}
