using UnityEngine;
using System.Collections;

public interface bullet {

    //-1 - abduct, 0 - lazerFire, 1 - special1, 2 - special2, 3 - special3, 4- special4
    int getType();

    //How much force we should fire the bullet with
    float getFireForce();

    //
    //  Kinematic bullets will not recieve collision updates, so we have to transfer collison 
    //
    void OnCollided(Collision coll);
}
