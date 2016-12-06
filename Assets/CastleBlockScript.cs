using UnityEngine;
using System.Collections;

// Responsible for syncing castle blocks over the network
public class CastleBlockScript : MonoBehaviour {

	private bool destroyed = false;

	void Update () {
		if (transform.GetChild (0).position.y < -2) {
			if (!destroyed) {
				destroyed = true;
				transform.parent.GetComponent<CastleScript> ().childCrumbled ();
			}
		}
	}
}

//Ive networked up to CastleBlock (5)