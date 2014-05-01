using UnityEngine;
using System.Collections;

public class Trap : Furniture {

	private int damage=1;
	Vector2 corner1=Vector2.zero, corner2=Vector2.zero;
	BoxCollider2D box;
	public bool sprung=false;

	protected override void Start(){
		base.Start();
		box = GetComponent<BoxCollider2D>();
	}

	public void Activate(){
		if (!sprung){
			corner1 = (Vector2)transform.position + new Vector2(-box.size.x/2,box.size.y/2);
			corner1 = (Vector2)transform.position + new Vector2(box.size.x/2,-box.size.y/2);
			Collider2D[] colliders = Physics2D.OverlapAreaAll(corner1,corner2,GameVars.interactLayer);
			foreach (Collider2D collider in colliders){
				if (collider.CompareTag ("Person")){
					collider.transform.GetComponent<Person2>().Scare (damage);
				}
			}
			sprung=true;
			// Change sprite to used appearance
		}
	}

	public void Reset(){
		sprung=false;
		// Change sprite to unused appearance
	}
}
