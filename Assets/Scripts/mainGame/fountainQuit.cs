using UnityEngine;
using System.Collections;

public class fountainQuit : MonoBehaviour {

    public ParticleSystem fountainParticles;
	public void quitParticles() {
        fountainParticles.Stop();
    }
}
