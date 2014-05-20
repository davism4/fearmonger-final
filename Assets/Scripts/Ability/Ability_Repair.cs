using UnityEngine;
using System.Collections;

public class Ability_Repair : Ability {

	// Ability #2

	public Ability_Repair () {
		Name="Repair";
		FearDamage = 5;
		Description = "Haunted repair tools that fix damaged furniture.";
		Duration = 3;
		minFear=20;
		useCost=10;
		cooldownStart = 9;
	}
	
	protected override void Start(){
		hazard=Resources.Load<GameObject>("Prefabs/Hazards/Repair");
		effectSound=Resources.Load<AudioClip>("Sounds/fixing");
		base.Start();
	}
	
	public override void UseAbility (RaycastHit2D hit)
	{
		if (hit.collider.CompareTag ("Furniture")){
		base.UseAbility (hit);
		hazardInstance.GetComponent<Hazard_Repair>().SetTarget(hit.collider.gameObject);
		}
	}
}
