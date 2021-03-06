﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Ability : MonoBehaviour {
	
	public string Name;
	public string Description;
	private bool locked=true;
	public bool Locked { get { return locked; } set { locked = value; }}


	protected int minFear, useCost, FearDamage;
	public int UseCost { get {return useCost;}}
	public int Damage { get {return FearDamage; }}
	public int MinFear {
		get {return minFear;}
	}
	protected float Duration, cooldownStart; // in seconds
	public int TimeDuration { get { return (int)Duration; }}
	public int TimeCooldown { get { return (int)cooldownStart; }}

	protected GameObject hazard=null, hazardInstance=null;
	protected Game2 game;
	public AudioClip effectSound;
	public Texture2D icon;

	// protected MainGame game

	public float cooldownTimer=0f;

	public bool isCooldown {
		get { return  cooldownTimer>0f;} }
	
	// Animation and sound?

	public bool CanUse(){
		if (!Locked && cooldownTimer<=0f){// && game.energy>=EnergyCost){
			return true;
		} else {
			return false;
		}
	}
	
	public void Unlock()
	{
		Locked = false;
		Debug.Log("Unlocked ability: " + this.Name);
	}

	public string ShowName(){
		/*if (Locked)
			return Name+"\n("+useCost.ToString()+" fear to use)";// round up
		else */
		if (cooldownTimer>=10f)
			return "Ready in "+Mathf.CeilToInt (cooldownTimer).ToString ()+"s";// round up
		else if (cooldownTimer>0f)
			return "Ready in "+Mathf.CeilToInt (cooldownTimer).ToString ()+"s ";// round up
		else
			return Name;
	}

	public int CooldownSeconds(){
		return Mathf.CeilToInt (cooldownTimer);
	}

	// use this function for BETA
	// CanUse() is ALWAYS called before this
	public virtual void UseAbility(RaycastHit2D hit){
		if (CanUse()){
			cooldownTimer = cooldownStart;
			game.fearEnergy -= useCost;
//			Debug.Log("Used ability "+ this.Name +" set with "+FearDamage+" dmg.");
			if (hazard!=null) {
				hazardInstance = GameObject.Instantiate(hazard,new Vector3(hit.point.x,hit.point.y,0f),Quaternion.identity) as GameObject;
				hazardInstance.GetComponent<Hazard>().SetValues(Duration, FearDamage);
				if(effectSound!=null)
					AudioSource.PlayClipAtPoint (effectSound, Camera.main.transform.position);
			}
		}

	}

	// PROTECTED/PRIVATE

	private void Update(){
		//Debug.Log (CooldownSeconds());
		if (game.fearEnergy>=minFear){
			Locked=false;
		} else {
			Locked=true;
		}
		if (isCooldown){
			if (cooldownTimer>0){
				cooldownTimer -= Time.deltaTime;
			}
		}
	}
	
	protected virtual void Start(){
		game = transform.GetComponent<Game2>();
		cooldownTimer=0f;
	}

	protected virtual void EndAbility(){
		//game.currentRoom.ActiveAbilityEffects.Remove (this);
	}
	
	// this function is from ALPHA
	// room = the current room (based on GameManager)
	// args = depends on ability
	/*	public virtual void UseAbility(Game game, Vector2 clickLocation){
		//game.currentRoom.ActiveAbilityEffects.Add (this);
		///game.playerLevel.UseEnergy(EnergyCost);
		// normalize to proper Z-depth
		Debug.Log("Used ability "+ this.Name);
		Vector3 clickLocation3d = new Vector3(clickLocation.x, clickLocation.y, 0);
		GameObject.Instantiate(hazard,clickLocation3d,Quaternion.identity);
		if(effectSound!=null)
			AudioSource.PlayClipAtPoint (effectSound, hazard.transform.position);
	}*/

}
