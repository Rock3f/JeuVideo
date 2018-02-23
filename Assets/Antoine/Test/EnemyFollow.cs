using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour {

	public float speed;

	public Transform Player1;

	public Transform Player2;

	private Transform target;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		//target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		if (Vector3.Distance(Player1.GetComponent<Transform>().position, transform.position) > Vector3.Distance(Player2.GetComponent<Transform>().position, transform.position)){
			target = Player2.GetComponent<Transform>();
		}
		else{
			target = Player1.GetComponent<Transform>();
		}
		transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x +1, target.position.y, target.position.z), speed * Time.deltaTime);
		
	}
}
