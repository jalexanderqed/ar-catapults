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

	public int testing;

	public float toRadians(float degs){
		return ((Mathf.PI / 180f) * degs);
	}

	//[Command]
	public Vector3 Offset(float longi,float latit,bool offsetProvided){
		if (!choseLocation)
			return new Vector3 (0,-1,0); //not ready yet
		float dLong = longi - longitude;
		float dLati = latit - latitude;

		if (offsetProvided) {
			return new Vector3 (latit,0,longi);
		} else {
			return new Vector3 (dLati * 111111f, 0f, dLong * 111111f * Mathf.Cos (latitude));
		}

		/*float rDLong = toRadians (dLong);
		float rDLati = toRadians (dLati);

		float rLatit = toRadians (latit);
		float rLatitude = toRadians (latitude);

		float a = Mathf.Pow(Mathf.Sin(rDLati/2f),2) + Mathf.Pow(Mathf.Sin(rDLong/2f)) * Mathf.Cos(rLatitude) * Mathf.Cos(rLatit); 
		float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1-a)); 
		float dist = c * 6371 * 1000;
		//Should be a vector in meters
		return  new Vector3(dLati,0,dLong).normalized * dist;*/
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
