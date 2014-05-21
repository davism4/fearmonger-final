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
	
	public Person2_Thug () {
		moneyDropMax=1;
		//fearDropMax=2;
		admireCooldownMin=20f;
		admireCooldownMax=50f;
		sanityMax=25;
		baseSpeedMin=12f;
		baseSpeedMax=14f;
		destroyCooldown=destroyCooldownMax;
	}
	
	protected override void Start(){
		radius = 2*GetComponent<BoxCollider2D>().size.x;
		base.Start();
	}
	
	public override void Interact(Furniture f){
		if (!f.IsTrap){
			//if (UnityEngine.Random.value<(1.0f)/room.NonTrapFurnitureCount ())
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
				if ((target.transform.position - transform.position).magnitude>radius){
					target=null;
				} else {	
					IS_FACING_RIGHT = (target.transform.position.x > transform.position.x);
					if (destroyCooldown>0f){
						destroyCooldown -= Time.deltaTime;
					} else {
						destroyCooldown=destroyCooldownMax;
						Attack (target);
					}
				}
			}
			if (anim!=null){
				anim.SetBool ("attacking", IS_ATTACKING);
			}
			
			isBusy = IS_ATTACKING;
			base.UpdateNormal ();
		}
	}
	
	public override void Exit(bool forced){
		/*if (!forced && GameVars.IsNight && sanity>0f){
			room.RemoveEnemy (gameObject,true);
		} else {
			room.RemoveEnemy (gameObject,false);
			base.Exit(forced);
		}*/
		room.game.CheckOutThug();
		base.Exit (forced);
	}
	
	public override void Scare(int s){
		// reset target trap
		target=null;
		destroyCooldown=destroyCooldownMax;
		base.Scare (s);
	}
	
	private void Attack(Furniture t){
		destroyCooldown=destroyCooldownMax;
		//		Debug.Log((t.transform.position - transform.position).magnitude);
		if (t.Damage(2)){
			target=null;
		}
	}
	
}

