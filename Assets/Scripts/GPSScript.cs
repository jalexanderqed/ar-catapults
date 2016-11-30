using UnityEngine;
using System.Collections;

public class GPSScript : MonoBehaviour {

	public GameObject text;
	private TextMesh txt;
	private bool locEnabled = true;
	private Compass compass;

	public float longitude;
	public float latitude;
	public float heading;
	public bool ready = false;

	IEnumerator Wait(int secs){
		yield return new WaitForSeconds(secs);
	}

	void Start () {
		txt = text.GetComponent<TextMesh>();
		if (!Input.location.isEnabledByUser) {
			txt.text = "Please enable location services!";
			locEnabled = false;
		} else {
			Input.location.Start (5f,1f);
		}
		compass = new Compass ();
		compass.enabled = true;
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
					+ "\nCompass: " + compass.trueHeading;
				latitude = Input.location.lastData.latitude;
				longitude = Input.location.lastData.longitude;
				heading = compass.trueHeading;
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
}
