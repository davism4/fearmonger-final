using UnityEngine;
using System.Collections;

public class Person2_Priest : Person2 {

	private float destroyCooldown;
	private const float destroyCooldownMax=1.5f;
	Trap target=null;

	public bool IS_ATTACKING {
		get { return destroyCooldown > 0f; }
	}

	public void Animate(){
		//
	}

	protected override void Start () {
		moneyDropMin=0;
		moneyDropMax=0;
		fearDropMin=0;
		fearDropMax=2;
		admireCooldownMin=15f;
		admireCooldownMax=25f;
		sanityMax=60;
		speedNormal += UnityEngine.Random.Range (-0.6f,0.08f);
		speedFast += UnityEngine.Random.Range (-0.5f,0f);
		destroyCooldown=destroyCooldownMax;
		base.Start ();
	}
	
	public override void Interact(Furniture f){
		if (f is Trap){
			if (UnityEngine.Random.value<(1.0f)/room.TrapCount ())
				target=(f as Trap);
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
			if (room.TrapCount ()<1){
				isLeaving=true;
			} else if (target!=null){
				if (destroyCooldown>0f){
					destroyCooldown -= Time.deltaTime;
				} else {
					destroyCooldown=destroyCooldownMax;
					Attack (target);
				}
			}

			if (anim!=null){
				anim.SetBool ("attacking", IS_ATTACKING);
			}

			base.UpdateNormal ();
		}
	}

	public override void Exit(bool forced){
		if (!forced && GameVars.IsNight && sanity>0f){

		} else {
			base.Exit(forced);
		}
	}

	public override void Scare(int s){
		// reset target trap
		target=null;
		destroyCooldown=destroyCooldownMax;
		base.Scare (s);
	}

	private void Attack(Trap t){
		destroyCooldown=destroyCooldownMax;
		if (t.Damage(1)){
			t.Break ();
			target=null;
		}
	}

}

