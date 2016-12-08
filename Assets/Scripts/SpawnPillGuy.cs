using UnityEngine;
using System.Collections;

public class SpawnPillGuy : MonoBehaviour {

    public GameObject pillGuyObject;
	public GameObject tablet;
    GameObject pillGuy;

    // Use this for initialization
    void Start () {
        //pillGuy = Instantiate(pillGuyObject);
		//pillGuy.GetComponent<PillGuyMovement>().myTablet = tablet.transform.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {
        //Destroy(pillGuy);
    }
}
