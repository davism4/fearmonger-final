using UnityEngine;
using System.Collections;

public class Person2_Child : Person2 {

	public Person2_Child() {
		fearDropMax=4;
		moneyDropMax=1;
		admireCooldownMin=15f;
		admireCooldownMax=20f;
		sanityMax=40;
		baseSpeedMin=19f;
		baseSpeedMax=22f;
	}

	protected override void Start(){
		screamSound=Resources.Load<AudioClip>("Sounds/scream_child_female_1");
		base.Start ();
	}
}
