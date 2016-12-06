using UnityEngine;
using System.Collections;

public class GPSScript : MonoBehaviour {

	public GameObject text;
	private TextMesh txt;
	private bool locEnabled = true;
	//private Compass comp;
	private Compass newCom;

	public float longitude;
	public float latitude;
	public float heading;
	public bool ready = false;
	public Vector3 offset;

	private int i = 0;

	void Start () {
		txt = text.GetComponent<TextMesh>();
		if (!Input.location.isEnabledByUser) {
			txt.text = "Please enable location services!";
			locEnabled = false;
		} else {
			Input.compass.enabled = true;
			newCom = new Compass ();
			newCom.enabled = true;
			Input.location.Start (5f,1f);
			Input.compass.enabled = true; 
			newCom.enabled = true;
			while (!newCom.enabled) {
			}
			while (!Input.compass.enabled) {
			}
			//comp = new Compass ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (locEnabled) {
			if (Input.location.status == LocationServiceStatus.Initializing) {
				txt.text = "Initializing";
			} else if (Input.location.status == LocationServiceStatus.Failed) {
				txt.text = "Unable to determine device location";
				return;
			} else if (Input.location.status == LocationServiceStatus.Running) {
                txt.text = "Latitude: " + Input.location.lastData.latitude + "\nLongitude: " + Input.location.lastData.longitude + "\nAltitude: " + Input.location.lastData.altitude + "\nAccuracy: " + Input.location.lastData.horizontalAccuracy + "\nTime: " + Input.location.lastData.timestamp
					+ "\nCompass: " + newCom.trueHeading + "\n Unity Offset: " + newCom.magneticHeading + "," + newCom.rawVector.x + "\ni: " + Input.compass.trueHeading;
				latitude = Input.location.lastData.latitude;
				longitude = Input.location.lastData.longitude;

				heading = newCom.trueHeading;
				i+=1;
				if(!ready) ready = true;
			}
		}
	}

	public float getLongitude(){
		return longitude;
	}

	public float getLatitude(){
		return latitude;
	}

	public float getHeading(){
		return heading;
	}

	public bool getReady(){
		return ready;
	}

	public void setOffset(Vector3 off){
		offset = off;
	}

	public Vector3 getOffset(){
		return offset;
	}
}
