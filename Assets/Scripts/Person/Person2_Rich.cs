using UnityEngine;
using System.Collections;

public class Person2_Rich : Person2 {

	protected override void Start () {
		fearDropMin=0;
		fearDropMax=3;
		moneyDropMin=1;
		moneyDropMax=3;
		admireCooldownMin=12f;
		admireCooldownMax=20f;
		speedNormal += UnityEngine.Random.Range (-0.1f,0.05f);
		speedFast += UnityEngine.Random.Range (-0.05f,0.125f);
		base.Start ();
	}

}
