using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ServerScript : NetworkBehaviour {

	[SyncVar(hook = "OnChangePlayerNum")]
	public int playerNum;

	[SyncVar(hook = "OnChangeLongitude")]
	public float longitude;

	[SyncVar(hook = "OnChangeLatitude")]
	public float latitude;

	[SyncVar(hook = "OnChooseLocation")]
	public bool choseLocation;

	private GPSScript gps;


	//[Command]
	public Vector3 Offset(float longi,float latit){
		//if (!isServer)
			//return new Vector3 (0, -2, 0);
		if (!choseLocation)
			return new Vector3 (0,-1,0); //not ready yet
		return new Vector3(longitude - longi,0,latitude - latit);
	}
		
	public void setLoc(float longi, float latit){
		if (!isServer)
			return;
		if (!choseLocation) {
			longitude = longi;
			latitude = latit;
			choseLocation = true;
			Debug.Log ("Chose location!");
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

	void OnChangePlayerNum(int num){
		playerNum = num;
	}

	void OnChangeLongitude(float longit){
		longitude = longit;
	}

	void OnChangeLatitude(float latit){
		latitude = latit;
	}

	void OnChooseLocation(bool chose){
		choseLocation = chose;
	}

}
