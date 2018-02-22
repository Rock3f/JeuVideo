using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltBar : MonoBehaviour {

	// Use this for initialization
	public float hit = 0;
	public float maxHit = 10;

	public Transform foregroundBar;
	public Transform backgroundBar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		backgroundBar.localScale = new Vector3(maxHit, 1 , 1);
		foregroundBar.localScale = new Vector3(Mathf.Clamp(hit, 0 , maxHit), 1, 1);

	}
}
