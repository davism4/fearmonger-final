using UnityEngine;
using System.Collections;

public class Person2_Child : Person2 {

	protected override void Start(){
		fearDropMin=2;
		fearDropMax=4;
		moneyDropMin=0;
		moneyDropMax=1;
		admireCooldownMin=20f;
		admireCooldownMax=30f;
		sanityMax=30;
		speedNormal += UnityEngine.Random.Range (0f,0.2f);
		speedFast += UnityEngine.Random.Range (-0.01f,0.15f);
		base.Start();
	}
}
