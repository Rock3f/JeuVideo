using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatePnjBackGround : MonoBehaviour {

	private animationSprite ac;
	// Use this for initialization
	void Start () {
		ac = GetComponent<animationSprite> ();
	}
	
	// Update is called once per frame
	void Update () {
		ac.ChangeAnimation("normal", true);
	}
}
