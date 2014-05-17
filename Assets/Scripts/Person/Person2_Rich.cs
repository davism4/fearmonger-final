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
		speedNormal += UnityEngine.Random.Range (-2f,1f);
		speedFast += UnityEngine.Random.Range (-2.5f,2.5f);
		base.Start();
	}
}
