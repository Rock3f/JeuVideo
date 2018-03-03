using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAllie : MonoBehaviour {

	// Use this for initialization
	public Transform player2;
    void Start() {
        Physics2D.IgnoreCollision(player2.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}