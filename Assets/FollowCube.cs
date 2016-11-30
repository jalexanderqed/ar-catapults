using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FollowCube : NetworkBehaviour {

    GameObject camera;

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer) return;
        camera = GameObject.Find("SceneCamera");
        this.transform.parent = camera.transform;
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = new Vector3(1, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
