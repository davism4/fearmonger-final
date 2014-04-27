using UnityEngine;
using System.Collections;

public class LampObject : MovableObject {

	[HideInInspector] public bool IsOn=false;
	private Light lightSource;
	protected RoomObject room;
	// By convention, timers go from zero to max, then reset to zero
	private float flickerTimer = 0f, flickerTimerMax = 0.1f;
	private int flickersRemaining=0;
	private float cryTimer=0f, cryTimerMax=1f;

	// Use this for initialization
	private void Start () {
		//IsOn = true;
		lightSource = (Light)transform.GetComponent("Light");
		room = transform.parent.GetComponent<RoomObject>();
	}

	public void StartFlicker(int flickers)
	{
		flickersRemaining = flickers;
		flickerTimer = 0;
	}

	public void Switch()
	{
		if (IsOn) TurnOff();
		else TurnOn();
	}

	public void TurnOn()
	{
		IsOn = true;
		lightSource.enabled = true;
		room.TurnLightOn();
		/*
		foreach (PersonObject p in room.members) {
			if (p!=null && p.currentSanity<p.MaxSanity)
			{
				p.currentSanity++;
			}
		}
		*/
	}

	public void AssignToPerson(){
		float dist=0f, minDist=999f;
		Person assignee=null;
		foreach(Person p in room.occupants){
			if (p!=null && p.isAdult && !p.hasTargetLamp){
				dist = ((Vector2)transform.position-(Vector2)p.transform.position).magnitude;
				if (dist<minDist){
					minDist=dist;
					assignee=p;
				}
			}
		}
		if (assignee!=null){
			assignee.AssignLamp(this);
		}
	}

	public void TurnOff()
	{
		IsOn = false;
		lightSource.enabled = false;
		room.TurnLightOff();
		AssignToPerson();
	}

	// Update is called once per frame
	private void Update () {
		if (!IsOn){
			if(cryTimer<cryTimerMax){
				cryTimer += GameVars.Tick*Time.deltaTime;
			} else {
				cryTimer=0f;
				AssignToPerson ();
			}
		}
		if (flickersRemaining>0) {
			if (flickerTimer<flickerTimerMax)
			{
				flickerTimer += GameVars.Tick*Time.deltaTime;
			}
			else {
				flickerTimer =0;
				flickersRemaining--;
				if (IsOn) TurnOff ();
				else TurnOn();
			}
		}
	}
}
