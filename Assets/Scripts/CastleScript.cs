using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CastleScript : NetworkBehaviour {

	void Update () {
		if (!isServer)
			return;
		int children = transform.childCount;
		if (children < 5) {
			GameObject serverObj = GameObject.Find ("ServerObj");
			ServerScript server = serverObj.GetComponent<ServerScript> ();
			server.endGame ();
		}
	}
}
