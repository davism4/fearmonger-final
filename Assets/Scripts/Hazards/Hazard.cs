using UnityEngine;
using System.Collections;

public class Hazard : MonoBehaviour {

	[HideInInspector] public int damage;
	protected float lifetime, duration; // in seconds
	//protected CircleCollider2D ccollider;
	//protected RoomObject currentRoom;
	[HideInInspector] protected Room room;

	public void SetValues(float Duration, int Damage){
		duration = Duration;
		damage = Damage;
	}

	protected virtual void Start(){
		lifetime = duration;
	}

	// Update is called once per frame
	protected virtual void Update () {
		if (lifetime>0) {
			lifetime -= Time.deltaTime;
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
