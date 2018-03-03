using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeBar : MonoBehaviour {

	public float hp = 5;
	public float maxHp = 5;

	public Transform foregroundBar;
	public Transform backgroundBar;

	public GameObject player;
	// Use this for initialization
	void Start () {
		maxHp = player.GetComponent<fight>().maxHp;
	}
	
	// Update is called once per frame
	void Update () {
		hp = player.GetComponent<fight>().hp;
		backgroundBar.localScale = new Vector3(maxHp, 1 , 1);
		foregroundBar.localScale = new Vector3(Mathf.Clamp(hp, 0 , maxHp), 1, 1);

	}
}
