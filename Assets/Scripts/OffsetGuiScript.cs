using UnityEngine;
using System.Collections;

public class OffsetGuiScript : MonoBehaviour {

	public string offset = "0,0";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		offset = GUI.TextField(new Rect(10, 10, 200, 100), offset, 25);
	}
}
