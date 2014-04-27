using UnityEngine;
using System.Collections;

public class Hazard : MonoBehaviour {

	protected int damage;
	protected float timer=0, duration; // in seconds
	//protected CircleCollider2D ccollider;
	//protected RoomObject currentRoom;
	protected Room room;

	// Update is called once per frame
	protected virtual void Update () {
		if (timer<duration) {
			timer += Time.deltaTime;
		}
		else {
			Finish();
		}
	}

	// Run when duration is up
	protected virtual void Finish() {
		// play some particle effect/animation, maybe?
		Destroy(gameObject);
		Destroy(this);
	}
}
