using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour {

	/** Placeable = permanent dropped into the grid layout. People interact with them.
	 * width, height = # blocks for object 2D dimensions
	 * admireValue = max of how much $ people drop when admiring
	 * buyCost = how much $ the player needs for a new piece
	 */
	public int width=1, height=1;
	//public int admireValue=0;
	public int buyCost=0;
	public float dropRate=0.0f;

	protected virtual void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			Person2 p = other.transform.GetComponent<Person2>();
			p.Interact (this);
		}
	}

	protected virtual void Start(){
	}

	protected virtual void Update(){
	}
	
	public void Delete(){
	
	}
}
