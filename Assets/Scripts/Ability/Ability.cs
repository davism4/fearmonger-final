using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Ability : MonoBehaviour {
	
	[HideInInspector] public string Name, Description;
	[HideInInspector] public bool Locked=true;
	//public int MinLevel; // we might not use this
	[HideInInspector] public int BuyCost, EnergyCost, FearDamage;
	protected float Duration; // in seconds
	protected GameObject hazard=null;
	protected MainGame game;
	public AudioClip effectSound;

	// protected MainGame game
	public bool isCooldown=false;
	private float cooldownTimer;
	protected float cooldownStart; // counts down 

	// Animation and sound?

	public bool CanUse(){
		if (!Locked && !isCooldown){// && game.energy>=EnergyCost){
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
		if (Locked)
			return Name+"\n($"+BuyCost.ToString()+" to buy)";// round up
		else if (isCooldown)
			return Name+"\n(Ready in "+Mathf.CeilToInt (cooldownTimer).ToString ()+"s)";// round up
		else
			return Name;
	}

	public int CooldownSeconds(){
		return Mathf.CeilToInt (cooldownTimer);
	}

	// this function is from ALPHA
	// room = the current room (based on GameManager)
	// args = depends on ability
	public virtual void UseAbility(Game game, Vector2 clickLocation){
		//game.currentRoom.ActiveAbilityEffects.Add (this);
		game.playerLevel.UseEnergy(EnergyCost);
		// normalize to proper Z-depth
		Debug.Log("Used ability "+ this.Name);
		Vector3 clickLocation3d = new Vector3(clickLocation.x, clickLocation.y, 0);
		GameObject.Instantiate(hazard,clickLocation3d,Quaternion.identity);
		if(effectSound!=null)
			AudioSource.PlayClipAtPoint (effectSound, hazard.transform.position);
	}

	// use this function for BETA
	// CanUse() is ALWAYS called before this
	public virtual void Activate(Vector2 point){
		cooldownTimer = cooldownStart;
		isCooldown=true;
		Debug.Log("Used ability "+ this.Name);
		if (hazard!=null) {
			GameObject.Instantiate(hazard,new Vector3(point.x,point.y,-5),Quaternion.identity);
			if(effectSound!=null)
				AudioSource.PlayClipAtPoint (effectSound, hazard.transform.position);
		}
	}

	// PROTECTED/PRIVATE

	private void Update(){
		//Debug.Log (CooldownSeconds());
		if (isCooldown){
			if (cooldownTimer>0){
				cooldownTimer -= Time.deltaTime;
			} else {
				cooldownTimer=cooldownStart;
				isCooldown=false;
			}
		}
	}
	
	protected virtual void Start(){
		game = transform.GetComponent<MainGame>();
		cooldownTimer=cooldownStart;
	}

	protected virtual void EndAbility(){
		//game.currentRoom.ActiveAbilityEffects.Remove (this);
	}
	
}
