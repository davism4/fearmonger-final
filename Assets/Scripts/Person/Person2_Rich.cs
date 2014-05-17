﻿using UnityEngine;
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
		speedNormal *= UnityEngine.Random.Range (0.9f,1.1f);
		base.Start();
	}
}
