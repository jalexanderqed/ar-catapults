using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FollowCube : NetworkBehaviour {

	public GameObject myCamObj;
	private Camera myCam;

	private GameObject camera;
	private GameObject serverObj;
	private GPSScript gps;
	//private ServerScript server;
	private int located = 0;

	[SyncVar(hook = "OnGetOffset")]
	public Vector3 offset;


	// Use this for initialization
	void Start () {
        if (!isLocalPlayer) return;
        camera = GameObject.Find("SceneCamera");
		myCam = myCamObj.GetComponent<Camera> ();
		camera.GetComponent<Camera> ().enabled = false;
		myCam.enabled = true;
		gps = GetComponent<GPSScript> ();
        //this.transform.parent = camera.transform;
        //this.transform.localPosition = new Vector3(0, 0, 0);
        //this.transform.localRotation = Quaternion.identity;
        //this.transform.localScale = new Vector3(1, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) return;
		//Make sure server knows where it is and attempt to localize
		if (located == 0) {
			if (gps.getReady ()) {
				CmdLocate (gps.getLongitude(), gps.getLatitude(),true);
				located = 1;
			}
		} 
		//If not still localized, keep trying
		if (located == 1) {
			if (gps.getOffset().y == -1) {
				CmdLocate (gps.getLongitude(), gps.getLatitude(),false);
			} else {
				located = 2;
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
	void CmdLocate(float longit,float latit,bool provideLoc){
		GameObject serverObj = GameObject.Find ("ServerObj");
		ServerScript server = serverObj.GetComponent<ServerScript> ();
		if(provideLoc) server.setLoc (longit, latit);
		offset = server.Offset (longit, latit);
	}

	void OnGetOffset(Vector3 off){
		offset = off;
		gps.setOffset (off);
	}
}
