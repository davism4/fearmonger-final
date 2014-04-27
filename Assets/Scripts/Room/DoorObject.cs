using UnityEngine;
using System.Collections;

public class DoorObject : MonoBehaviour {

	//BoxCollider2D box;
	//RoomObject myRoom;
	Person p;

	// Use this for initialization
	void Start () {
		//box=transform.GetComponent<BoxCollider2D>();
		//myRoom= transform.GetComponent<RoomObject>();
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Person")){
//			Debug.Log("A person approaches the door");
			p = other.transform.GetComponent<Person>();
			if (p.Sanity<=0){
				p.Leave ();
			}
		}
	}

}
