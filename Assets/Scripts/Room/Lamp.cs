﻿using UnityEngine;
using System.Collections;

public class Lamp: Furniture {

	protected Light lite;

	protected override void Start(){
		lite = transform.GetChild (0).light;
		base.Start ();
	}

	public bool isOn {
		get {return lite.enabled; }
	}

	public void Off(){
		lite.enabled = false;
	}

	public virtual void Flip(){
		if (isOn){
			lite.enabled=false;
		} else {
			lite.enabled=true;
		}
	}

	public virtual void Flip(Person2 p){
		Flip ();
	}

}
