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
		speedNormal += UnityEngine.Random.Range (-0.01f,0.05f);
		speedFast += UnityEngine.Random.Range (-0.02f,0.05f);
		base.Start();
	}


	
}