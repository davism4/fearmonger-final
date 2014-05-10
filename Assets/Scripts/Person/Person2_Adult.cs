using UnityEngine;
using System.Collections;

public class Person2_Adult : Person2 {
	
	protected override void Start(){
		fearDropMin=1;
		fearDropMax=2;
		moneyDropMin=1;
		moneyDropMax=2;
		admireCooldownMax=25f;
		admireCooldownMin=20f;
		sanityMax=45;
		speedNormal += UnityEngine.Random.Range (-0.35f,0.1f);
		speedFast += UnityEngine.Random.Range (-0.4f,0.2f);
		base.Start();
	}
}