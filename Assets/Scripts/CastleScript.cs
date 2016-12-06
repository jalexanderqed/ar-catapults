using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CastleScript : NetworkBehaviour {

	void Update () {
		if (!isServer)
			return;
		int children = transform.childCount;
		if (children < 8) {
			GameObject serverObj = GameObject.Find ("ServerObj");
			ServerScript server = serverObj.GetComponent<ServerScript> ();
			server.endGame ();
		}
	}
	void OnDestroy(){
		if (isServer)
			return;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			players [i].GetComponent<FollowCube> ().makeGuiObj ();
		}
	}
}
