using UnityEngine;
using System.Collections;

public class PillGuyMovement : MonoBehaviour {

    public GameObject myTablet;

	// Use this for initialization
	void Start () {
        	
	}
	
	// Update is called once per frame
	void Update () {
		if (myTablet) {
			this.transform.position = new Vector3 (myTablet.transform.position.x, 1, myTablet.transform.position.z);
			this.transform.rotation = Quaternion.Euler (0, myTablet.transform.eulerAngles.y, 0);
			this.transform.position -= transform.forward * 0.75f;
		} else {
			Destroy (gameObject);
		}
	}
}
