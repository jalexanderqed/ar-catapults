using UnityEngine;
using System.Collections;

// Responsible for syncing castle blocks over the network
public class CastleBlockScript : MonoBehaviour {

	void Update () {
		if (transform.GetChild(0).position.y < -2)
			Destroy (gameObject);
	}
}

//Ive networked up to CastleBlock (5)