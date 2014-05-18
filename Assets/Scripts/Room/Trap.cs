using UnityEngine;
using System.Collections;

public class Trap : Furniture {

	protected Animator anim;

	public int damage=1;
	Vector2 corner1=Vector2.zero, corner2=Vector2.zero;
	BoxCollider2D hurtBox; 
	public bool Used=false;

	protected override void Start(){
		anim = transform.GetComponent<Animator> ();
		if (!anim.enabled)
			anim=null;

		base.Start();
	}


	public void Activate(){
		if (!Used){
			if (this.node!=null){
				hurtBox = this.node.GetComponent<BoxCollider2D>();
				corner1 = (Vector2)transform.position + new Vector2(-hurtBox.size.x/2,hurtBox.size.y/2);
				corner2 = (Vector2)transform.position + new Vector2(hurtBox.size.x/2,-hurtBox.size.y/2);
				Collider2D[] colliders = Physics2D.OverlapAreaAll(corner1,corner2,GameVars.interactLayer);
				foreach (Collider2D collider in colliders){
					if (collider.CompareTag ("Person")){
						collider.transform.GetComponent<Person2>().Scare (damage);
					}
				}
			}
			Used=true;
			// Animate or change sprite to used appearance
		}
		// Individual code here
	}

	public void Reset(){
		Used=false;
		// Change sprite to unused appearance
	}

	// Update is called once per frame
	protected override void Update () {
		if (anim!=null){
			anim.SetBool ("activated", Used);
		}
		base.Update();
	}
}
