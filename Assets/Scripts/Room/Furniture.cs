using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour {

	/** Placeable = permanent dropped into the grid layout. People interact with them.
	 * width, height = # blocks for object 2D dimensions
	 * admireValue = max of how much $ people drop when admiring
	 * buyCost = how much $ the player needs for a new piece
	 */
	//public int admireValue=0;
	private bool isShowingDurability=false;
	private float showCountdown=0.75f;
	private const float showCountdownMax=0.75f;

	public int buyCost=0;
	public int durability=99;

	// Called during daytime furniture placement
	public void Delete(){
		
	}

	public void DisplayHP(){
		isShowingDurability=true;
		showCountdown=showCountdownMax;
	//	text.text=sanityPercent.ToString ()+"%";
	}

	protected virtual void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			Person2 p = other.transform.GetComponent<Person2>();
			p.Interact (this);
		}
	}

	protected virtual void Start(){
	}

	protected virtual void Update(){
		if (isShowingDurability){
			if(showCountdown>0f){
				showCountdown -= Time.deltaTime;
			} else {
				showCountdown = showCountdownMax;
				isShowingDurability=false;
			}
		}
	}

	public void Break(){
		// animation
		DestroyImmediate (gameObject);
	}


}
