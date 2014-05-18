using UnityEngine;
using System.Collections;

public class Person2_Adult : Person2 {
	
	public Person2_Adult(){
		fearDropMax=2;
		moneyDropMax=2;
		admireCooldownMax=25f;
		admireCooldownMin=20f;
		sanityMax=25;
		baseSpeedMin=15f;
		baseSpeedMax=18f;
	}
}