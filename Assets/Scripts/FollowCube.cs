using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FollowCube : NetworkBehaviour {

    GameObject camera;

	private GameObject serverObj;
	private int located = 0;
	private GPSScript gps;
	private ServerScript server;

	private Vector3 offset;



	// Use this for initialization
	void Start () {
        if (!isLocalPlayer) return;
		serverObj = GameObject.Find ("UnetManager");
		server = serverObj.GetComponent<ServerScript> ();
        camera = GameObject.Find("SceneCamera");
		gps = GetComponent<GPSScript> ();
        this.transform.parent = camera.transform;
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = new Vector3(1, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) return;
		if (located == 0) {
			if (gps.getReady ()) {
				server.CmdSetLoc (gps.getLongitude(), gps.getLatitude());
				located = 1;
			}
		} 
		if (located == 1) {
			offset = server.Offset (gps.getLongitude (), gps.getLatitude ());
			if (offset.y == 0)
				located = 2;
		}
	}
}
