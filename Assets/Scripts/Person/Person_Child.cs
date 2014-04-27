using UnityEngine;
using System.Collections;

public class Person_Child : Person
{
	// Nothing special. Weakest victim.
	protected override void Start () {
		isAdult=false;
		defenseBase=0;
		defenseSupport=1;
		sanityMax=15;
		sanityCurrent=sanityMax;
		base.Start ();
	}
}
