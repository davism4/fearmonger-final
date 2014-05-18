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
		speedNormal = UnityEngine.Random.Range (18f,22f);
		base.Start();
	}
}
