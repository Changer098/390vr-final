using UnityEngine;
using System.Collections;

public class buildDestruct : MonoBehaviour {
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.layer == 11) {
            bullet bulletScript = col.gameObject.GetComponent<bullet>();
            if (bulletScript != null) {
                bulletScript.OnCollided(col);
                HUDInfo.UpdateDestruction(1);
            }
        }
    }
}
