using UnityEngine;
using System.Collections;

// Let someone click on the lamp -> 3 damage
// Click on the lamp yourself -> 2 damage

public class Lamp_Scary : Lamp {
	
	public int damage=4;

	public Lamp_Scary(){
		IsTrap=true;
	}

	protected override void Start(){
		IsTrap=true;
		base.Start();
	}

	public override void Flip(Person2 p){
		Flip ();
		p.Scare (damage);
	}

}
