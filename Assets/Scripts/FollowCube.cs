using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class FollowCube : NetworkBehaviour {

	public GameObject myCamObj;
	public GameObject tablet;
	public GameObject myCatapult;
	public GameObject netProj;
	public GameObject netProjObj;

	public GameObject guiObj;

	private GameObject myGuiObj;
	private Camera myCam;
	private CatapultScript catScr;
	private GameObject camera;
	private GameObject serverObj;
	private GPSScript gps;
	private MultiTrackingScript trackScript;
	//private ServerScript server;
	private int located = 0;

	private bool useCompass = true;

	private bool OffsetProvided = false;

	[SyncVar(hook = "OnGetOffset")]
	public Vector3 offset;

    private float compassStartTime;
    private bool markerFound = false;
    private bool compassRotated = false;
    private int numCompassSamples = 0;
    private float compassSampleSum = 0;


	// Use this for initialization
	void Start () {
		catScr = myCatapult.GetComponent<CatapultScript> ();
		//transform.parent = GameObject.Find ("SceneCenter").transform;
		if (!isLocalPlayer) {
			catScr.amLocal = false;
			Destroy (myCamObj);
			return;
		}
		makeGuiObj ();
		trackScript = GameObject.Find ("SceneCenter").GetComponent<MultiTrackingScript> ();
		catScr.setLocalProperties ();
        camera = GameObject.Find("SceneCamera");
		myCam = myCamObj.GetComponent<Camera> ();
		camera.GetComponent<Camera> ().enabled = false;
		myCam.enabled = true;
		gps = GetComponent<GPSScript> ();

		//Spawn a catapult
		//myCatapult = Instantiate (catapult);
		//myCatapult.transform.parent = transform;
		//myCatapult.transform.localPosition = Vector3.zero;
		//myCatapult.transform.localRotation = Quaternion.identity;
		catScr.camera = myCam;

		string offGuiStr = GameObject.Find ("OffsetGui").GetComponent<OffsetGuiScript>().offset;
		Destroy (GameObject.Find ("OffsetGui"));

		string[] strs = offGuiStr.Split (new string[] {","}, StringSplitOptions.None);
		if (strs.Length == 2) {
			OffsetProvided = true;
			offset = new Vector3 (int.Parse (strs [0]), 0, int.Parse (strs [1]));
		}
        compassStartTime = Time.time;
	}

	public void makeGuiObj(){
		if (!isLocalPlayer)
			return;
		if (myGuiObj == null) {
			myGuiObj = Instantiate (guiObj);
			myGuiObj.GetComponent<GameStartGuiScript> ().player = this.gameObject;
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
			tablet.transform.localPosition = camera.transform.position;// + gps.getOffset ();
			tablet.transform.localRotation = camera.transform.rotation;
		}
		//myCatapult.GetComponent<CatapultScript> ()
		myCatapult.transform.LookAt (tablet.transform.position);
		myCatapult.transform.rotation = Quaternion.Euler (0, myCatapult.transform.rotation.eulerAngles.y, 0);
		if (catScr.getLaunched ()) {
			CmdSpawnNetProj (catScr.getPos (), catScr.getVel ());
		}

        GameObject mainTrack = trackScript.GetBestTracked();
        if (mainTrack != null && useCompass) {
            if (!markerFound)
            {
                markerFound = true;
                compassStartTime = Time.time;
            }
			
			if ((Time.time - compassStartTime) < 2) {
                float frontAngle = tablet.transform.rotation.eulerAngles.y;
                float heading = gps.getHeading();
                float diff = frontAngle - heading;
                compassSampleSum += diff;
                numCompassSamples++;
			}
            else if(!compassRotated)
            {
                compassRotated = true;
                mainTrack.transform.RotateAround(mainTrack.transform.position, Vector3.up, -1 * (compassSampleSum / numCompassSamples));
                //transform.RotateAround(transform.position, Vector3.up, (compassSampleSum / numCompassSamples));
            }
		}
	}

	[Command]
	void CmdLocate(float longit,float latit,bool provideLoc,bool offProv){
		GameObject serverObj = GameObject.Find ("ServerObj");
		ServerScript server = serverObj.GetComponent<ServerScript> ();
		if(provideLoc) server.setLoc (longit, latit);
		offset = server.Offset (longit, latit,offProv);
	}

	[Command]
	public void CmdSpawnNetProj(Vector3 pos,Vector3 vel){
		netProj = Instantiate (netProjObj);
		netProj.transform.position = pos;
		netProj.GetComponent<Rigidbody> ().velocity = vel;

		NetworkServer.Spawn (netProj);
	}

	[Command]
	void CmdStartGame(){
		
		GameObject serverObj = GameObject.Find ("ServerObj");
		ServerScript server = serverObj.GetComponent<ServerScript> ();
		server.startGame ();
	}

	void OnGetOffset(Vector3 off){
		if (!isLocalPlayer)
			return;
		offset = off;
		gps.setOffset (off);
		transform.position = gps.offset;
		//set rotation
		//transform.rotation = Quaternion.Euler(0,heading + 90,0);
	}

	public void startGame(){
		if (!isLocalPlayer)
			return;
		CmdStartGame ();
	}
}
