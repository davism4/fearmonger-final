using UnityEngine;
using System.Collections;

public class Person2_Thug : Person2 {

	private float destroyCooldown;
	private const float destroyCooldownMax=1.5f;
	private Furniture target=null;
	private float radius;

	public bool IS_ATTACKING {
		get { return (target!=null); }
	}
	
	// Use this for initialization
	public Person2_Thug () {
		moneyDropMax=1;
		fearDropMax=2;
		admireCooldownMin=15f;
		admireCooldownMax=25f;
		sanityMax=30;
		baseSpeedMin=11f;
		baseSpeedMax=14f;
		destroyCooldown=destroyCooldownMax;
	}
	
	public override void Interact(Furniture f){
	//	Debug.Log ("Interact with "+f.name);
		if (!f.IsTrap){
			//if (UnityEngine.Random.value<(1.0f)/room.NonTrapFurnitureCount ())
//			Debug.Log ("Targeting the "+f.name);
				target=f;
		} else {
			base.Interact (f);
		}
	}
	
	public void Reset(){
		sanity=sanityMax;
		destroyCooldown=destroyCooldownMax;
		target=null;
		isHurt=false;
	}
	
	protected override void UpdateNormal(){
		if (room!=null){
			if (room.NonTrapFurnitureCount ()<1){
				isLeaving=true;
			} else if (target!=null){
				IS_FACING_RIGHT = (target.transform.position.x > transform.position.x);
				if (destroyCooldown>0f){
					destroyCooldown -= Time.deltaTime;
				} else {
					destroyCooldown=destroyCooldownMax;
					Attack (target);
				}
			}
			isBusy = IS_ATTACKING;
			if (anim!=null){
				anim.SetBool ("attacking", IS_ATTACKING);
			}
			base.UpdateNormal ();
		}
	}

	public override void Exit(bool forced){
		room.game.CheckOutThug();
		/*if (!forced && GameVars.IsNight && sanity>0f){
			room.RemoveEnemy (gameObject,true);
		} else {
			room.RemoveEnemy (gameObject,false);
			base.Exit(forced);
		}*/
		base.Exit (forced);
	}

	protected override void Start(){
		radius = GetComponent<BoxCollider2D>().size.x;
		base.Start();
	}

	public override void Scare(int s){
		// reset target trap
		target=null;
		destroyCooldown=destroyCooldownMax;
		base.Scare (s);
	}
	
	private void Attack(Furniture t){
		destroyCooldown=destroyCooldownMax;
		if ((t.transform.position - transform.position).magnitude>radius)
			target = null;
		else if (t.Damage(2)){
			target=null;
		}
	}
}
