using UnityEngine;
using System.Collections;

public class Person2_Thug : Person2 {

	private float destroyCooldown;
	private const float destroyCooldownMax=1.5f;
	bool isDestroying=false;
	
	
	// Use this for initialization
	protected override void Start () {
		moneyDropMin=1;
		moneyDropMax=2;
		fearDropMin=1;
		fearDropMax=2;
		admireCooldownMin=20f;
		admireCooldownMax=25f;
		speedNormal += UnityEngine.Random.Range (-0.2f,0.02f);
		speedFast += UnityEngine.Random.Range (-0.2f,0f);
		destroyCooldown=destroyCooldownMax;
		base.Start ();
	}
	
	public override void Interact(Furniture f){
		if (!(f is Trap)){
			if (UnityEngine.Random.value<(1.0f)/room.furnitureList.Count){
				isDestroying=true;
			} 
		} else {
			base.Interact (f);
		}
	}
	
	protected override void UpdateNormal(){
		if (room!=null){
			if (room.furnitureList.Count<1){
				isLeaving=true;
			} else if (isDestroying){
				if (destroyCooldown>0f){
					destroyCooldown -= Time.deltaTime;
				} else {
					destroyCooldown=destroyCooldownMax;
				}
			}
			base.UpdateNormal ();
		}
	}
	
	public override void Scare(int s){
		// reset target trap
		isDestroying=false;
		destroyCooldown=destroyCooldownMax;
		base.Scare (s);
	}
	
	private void HarmFurniture(Furniture t){
		destroyCooldown=destroyCooldownMax;
		t.durability--;
		if (t.durability<=0){
			t.Break ();
			isDestroying=false;
		}
	}
}
