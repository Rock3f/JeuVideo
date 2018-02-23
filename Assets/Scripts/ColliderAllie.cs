using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAllie : MonoBehaviour {

	// Use this for initialization
	public Transform bulletPrefab;
    void Start() {
        Transform bullet = Instantiate(bulletPrefab) as Transform;
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}