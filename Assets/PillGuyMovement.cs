using UnityEngine;
using System.Collections;

public class PillGuyMovement : MonoBehaviour {

    public GameObject myTablet;

	// Use this for initialization
	void Start () {
        	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(myTablet.transform.position.x, 2, myTablet.transform.position.z);
	}
}
