﻿using UnityEngine;
using System.Collections;

public class SpawnPillGuy : MonoBehaviour {

    public GameObject pillGuyObject;

	// Use this for initialization
	void Start () {
        GameObject pillGuy = Instantiate(pillGuyObject);
        pillGuy.GetComponent<PillGuyMovement>().myTablet = this.transform.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
