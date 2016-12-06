using UnityEngine;
using System.Collections;

public class GameStartGuiScript : MonoBehaviour {

	public GameObject player;
	// Use this for initialization
	void OnGUI() {
		if (GUI.Button (new Rect (40, 40, 200, 200), "Start Game!")) {
			player.GetComponent<FollowCube> ().startGame ();
			Destroy (gameObject);
		}
	}
}
