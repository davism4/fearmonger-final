using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person_Priest : Person {

	//List<Ability> activeSpells;
	private float healTimer=0f, healTimerMax=3f;

	// More support defense. Special ability: Dispel
	protected override void Start () {
		isAdult=true;
		defenseBase=2;
		defenseSupport=2;
		sanityMax=30;
		sanityCurrent=sanityMax;
		base.Start ();
	}

	void Dispel(Ability targetSpell){
		// destroy ability
		// play dispel animation and sound
	}

	protected override void DefendOther (Person other)
	{
		other.AddDefense(defenseSupport);
		if (healTimer>healTimerMax && other.sanityCurrent>other.sanityMax){
			other.sanityCurrent++;
			healTimer=0f;
			text.text="HEALED!";
			isText=true;
		}
	}


	// Update is called once per frame
	protected override void Update () {
		if (healTimer<healTimerMax){
			healTimer+=GameVars.Tick*Time.deltaTime;
		}
		base.Update ();

	}
}
