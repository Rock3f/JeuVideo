using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltBar : MonoBehaviour {

	// Use this for initialization
	public float hit = 0;
	public float maxHit = 10;

	public Transform foregroundBar;
	public Transform backgroundBar;
	public GameObject player;
	// Use this for initialization
	void Start () {
		maxHit = player.GetComponent<fight>().maxCombo;
	}
	
	// Update is called once per frame
	void Update () {
		hit = player.GetComponent<fight>().combo;
		backgroundBar.localScale = new Vector3(maxHit, 1 , 1);
		foregroundBar.localScale = new Vector3(Mathf.Clamp(hit, 0 , maxHit), 1, 1);

	}
}
