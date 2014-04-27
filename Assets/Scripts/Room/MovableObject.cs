using UnityEngine;
using System.Collections;


public class MovableObject : MonoBehaviour {

	public int Weight = 0;
	public float sizex;
	public float sizey;
	public bool IsBeingHeld = false;
	public bool Intersect = false;
	BoxCollider2D box;

	// Use this for initialization
	void Start () {
		box = (BoxCollider2D)GetComponent<BoxCollider2D>();
		sizex = box.size.x/2;
		sizey = box.size.y/2;
		
	}

	// Update is called once per frame
	void Update () {
		if (IsBeingHeld) {
			box.isTrigger = true;
		}
		else {
			box.isTrigger = false;
		}
		if (Intersect) {
			transform.GetComponent<SpriteRenderer>().color = Color.black;
		}
		else {
			transform.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}
}
