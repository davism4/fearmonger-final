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
		sanityMax=45;
		speedNormal += UnityEngine.Random.Range (-0.4f,0.1f);
		speedFast += UnityEngine.Random.Range (-0.45f,0.2f);
		base.Start();
	}
}
