using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class FollowCube : NetworkBehaviour {

	public GameObject myCamObj;
	private Camera myCam;

	private GameObject camera;
	private GameObject serverObj;
	private GPSScript gps;
	//private ServerScript server;
	private int located = 0;

	private bool OffsetProvided = false;

	[SyncVar(hook = "OnGetOffset")]
	public Vector3 offset;


	// Use this for initialization
	void Start () {
		if (!isLocalPlayer) {
			Destroy (myCamObj);
			return;
		}
        camera = GameObject.Find("SceneCamera");
		myCam = myCamObj.GetComponent<Camera> ();
		camera.GetComponent<Camera> ().enabled = false;
		myCam.enabled = true;
		gps = GetComponent<GPSScript> ();

		string offGuiStr = GameObject.Find ("OffsetGui").GetComponent<OffsetGuiScript>().offset;
		Destroy (GameObject.Find ("OffsetGui"));

		string[] strs = offGuiStr.Split (new string[] {","}, StringSplitOptions.None);
		if (strs.Length == 2) {
			OffsetProvided = true;
			offset = new Vector3 (int.Parse (strs [0]), 0, int.Parse (strs [1]));
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) return;
		if (located < 2) {
			float lon = gps.getLongitude ();
			float lat = gps.getLatitude ();
			if (OffsetProvided) {
				lon = offset.z;
				lat = offset.x;
			}
			//Make sure server knows where it is and attempt to localize
			if (located == 0) {
				if (gps.getReady ()) {
					CmdLocate (lon,lat,true,OffsetProvided);
					located = 1;
				}
			} 
			//If not still localized, keep trying
			if (located == 1) {
				if (gps.getOffset().y == -1) {
					CmdLocate (lon,lat,false,OffsetProvided);
				} else {
					located = 2;
				}
			}
		}
	}

	void LateUpdate(){
		if (!isLocalPlayer) return;
		if (camera != null) {
			transform.position = camera.transform.position + gps.getOffset ();
			transform.rotation = camera.transform.rotation;
		}
	}

	[Command]
	void CmdLocate(float longit,float latit,bool provideLoc,bool offProv){
		GameObject serverObj = GameObject.Find ("ServerObj");
		ServerScript server = serverObj.GetComponent<ServerScript> ();
		if(provideLoc) server.setLoc (longit, latit);
		offset = server.Offset (longit, latit,offProv);
	}

	void OnGetOffset(Vector3 off){
		if (!isLocalPlayer)
			return;
		offset = off;
		gps.setOffset (off);
	}
}
