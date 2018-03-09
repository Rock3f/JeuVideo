using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatePnjBackGround : MonoBehaviour {

	private animationSprite ac;
	public float frameDuration = 0.2f;
	private float accumulateur = 0;
	// Use this for initialization
	void Start () {
		ac = GetComponent<animationSprite> ();
	}
	
	// Update is called once per frame
	void Update () {
		ac.ChangeAnimation("normal", true);
	}
}
