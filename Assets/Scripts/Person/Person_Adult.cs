using UnityEngine;
using System.Collections;

public class Person_Adult : Person {

	// Nothing special.
	protected override void Start () {
		isAdult=true;
		defenseBase=1;
		defenseSupport=1;
		sanityMax=20;
		sanityCurrent=sanityMax;
		base.Start ();
	}
}
