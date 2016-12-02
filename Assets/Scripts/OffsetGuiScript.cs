using UnityEngine;
using System.Collections;

public class OffsetGuiScript : MonoBehaviour {

	public string offset = "";
	void OnGUI() {
		offset = GUI.TextField(new Rect(10, 10, 200, 100), offset, 25);
	}
}
