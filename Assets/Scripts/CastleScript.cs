using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CastleScript : NetworkBehaviour {

	private int numLeft = 100;

	void Start(){
		numLeft = transform.childCount;
	}

	void Update () {
		if (!isServer)
			return;
		if (numLeft < 12) {
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

	public void childCrumbled(){
		numLeft -= 1;
	}
}
