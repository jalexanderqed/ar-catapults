using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerScript : NetworkBehaviour {

	[SyncVar]
	int playerNum;

	[SyncVar]
	float longitude;

	[SyncVar]
	float latitude;

	[SyncVar]
	bool choseLocation;

	private GPSScript gps;


	//[Command]
	public Vector3 Offset(float longi,float latit){
		//if (!isServer)
			//return new Vector3 (0, -2, 0);
		if (!choseLocation)
			return new Vector3 (0,-1,0); //not ready yet
		return new Vector3(longitude - longi,0,latitude - latit);
	}

	[Command]
	public void CmdSetLoc(float longi, float latit){
		if (!isServer)
			return;
		if (!choseLocation) {
			longitude = longi;
			latitude = latit;
			choseLocation = true;
		}
	}

	// Use this for initialization
	void Start () {
		gps = GetComponent<GPSScript> ();
	}

	void Update(){
		if (isServer && !choseLocation) {
			if (gps.getReady ()) {
				longitude = gps.getLongitude ();
				latitude = gps.getLatitude ();
				choseLocation = true;
			}
		}
	}

}
