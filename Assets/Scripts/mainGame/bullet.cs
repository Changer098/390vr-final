using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

    //-1 - abduct, 0 - lazerFire, 1 - special1, 2 - special2, 3 - special3, 4- special4
    int bulletType = 0;

    public void updateDetails(int type) {
        bulletType = type;
    }

    public int getType() {
        return bulletType;
    }
}
