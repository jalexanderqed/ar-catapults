using UnityEngine;
using System.Collections;

public class GPSScript : MonoBehaviour {

	public GameObject text;
	private TextMesh txt;
	private bool locEnabled = true;

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

	}
	
	// Update is called once per frame
	void Update () {
		
		if (locEnabled) {
			if (Input.location.status == LocationServiceStatus.Initializing) {
				txt.text = "Initializing";
			} else if (Input.location.status == LocationServiceStatus.Failed) {
				txt.text = "Unable to determine device location";
				return;
			} else {
				txt.text = "Latitude: " + Input.location.lastData.latitude + "\nLongitude: " + Input.location.lastData.longitude + "\nAltitude: " + Input.location.lastData.altitude + "\nAccuracy: " + Input.location.lastData.horizontalAccuracy + "\nTime: " + Input.location.lastData.timestamp;
			}
		}
	}
}
