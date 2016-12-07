using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CastleScript : NetworkBehaviour {

	public int numLeft = 100;

	[SyncVar(hook = "onCrumble")]
	public bool crumbled = false;

	void Start(){
		numLeft = transform.childCount;
		if (isServer)
			return;
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		for (int i = 0; i < players.Length; i++) {
			players [i].GetComponent<FollowCube> ().removeText ();
			players [i].GetComponent<FollowCube> ().removeGuiObj ();
		}
	}

	void Update () {
		if (!isServer)
			return;
		if (numLeft < 12) {
			crumbled = true;
			GameObject serverObj = GameObject.Find ("ServerObj");
			ServerScript server = serverObj.GetComponent<ServerScript> ();
			server.endGame ();
		}
	}

	/*void endGame(){
		Debug.Log ("!!!");
		if (isServer)
			return;
		CmdEndGame ();
	}

	[Command]
	void CmdEndGame(){
		Debug.Log ("Castle says to end!");
		GameObject serverObj = GameObject.Find ("ServerObj");
		ServerScript server = serverObj.GetComponent<ServerScript> ();
		server.endGame ();
	}*/

	public void childCrumbled(){
		numLeft -= 1;
	}

	public void onCrumble(bool val){
		crumbled = val;
		if (crumbled == true) {
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

			Vector3 p1Vect = transform.position - players [0].transform.position;
			Vector3 p2Vect = transform.position - players [1].transform.position;
			if (p1Vect.magnitude > p2Vect.magnitude) {
				players [1].GetComponent<FollowCube> ().lose ();
				players [0].GetComponent<FollowCube> ().win();
			} else {
				players [0].GetComponent<FollowCube> ().lose ();
				players [1].GetComponent<FollowCube> ().win ();
			}
			for (int i = 0; i < players.Length; i++) {
				players [i].GetComponent<FollowCube> ().makeGuiObj ();
			}
			//Destroy (gameObject);
		}
	}
}
