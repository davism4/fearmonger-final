using UnityEngine;
using System.Collections;

public class HotelRoof : MonoBehaviour {

	public void Initialize(Room[] r){
		for (int i=0; i<r.Length; i++){
			if (!r[i].open){
				transform.position = new Vector3(transform.position.x,r[i].transform.position.y,
				                                 transform.position.z);
				return;
			}
		}
		
	}

	public void MoveToFloor(Room r){
		transform.position = new Vector3(transform.position.x,
		                                r.transform.position.y + 7.95f,
		                                 transform.position.z);
	}
}
