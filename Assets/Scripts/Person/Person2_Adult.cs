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
		speedNormal = UnityEngine.Random.Range (12f,17f);
		base.Start();
	}
}