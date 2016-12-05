using UnityEngine;
using System.Collections;

public class NetProjectileScript : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		if(transform.position.y < 0){
			Destroy (gameObject);
		}
	}
}
