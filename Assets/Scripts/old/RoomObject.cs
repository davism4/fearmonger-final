// DEPRECATED

using UnityEngine;
using UnityEditor; // Contains the PrefabUtility class.
using System.Collections;
using System;
using System.Collections.Generic;

public class RoomObject : MonoBehaviour {

	/*======== VARIABLES ========*/
	/*private string roomNumber;
	public string RoomName { get {return ("Room "+roomNumber);}}
	public Person[] occupants;
	//public List<Ability> ActiveAbilityEffects = new List<Ability>();
	[HideInInspector] public Game game; // accessible to occupants
	protected PlayerLevel playerLevel;
	
	// Physical
	[HideInInspector] public Vector3 CameraPosition, ExitLocation;//, EntryLocation; 
	private Vector3 bed1StartPos, bed2StartPos, bed3StartPos, lamp1StartPos, lamp2StartPos;
	private Vector3 bed1angle, bed2angle, bed3angle, lamp1angle, lamp2angle;
	private Transform bed1, bed2, bed3, lamp1, lamp2;
	public LampObject Lamp1, Lamp2;

	// AI stuff
	public AudioClip doorOpenSound, doorCloseSound;
	public bool isOccupied=false, isUnlocked=false;
	private float stayTimer=0, stayTimerMax; // stay duration, max is reset randomly according to # of occupants
	private float vacantTimer=0, vacantTimerMax; // time in between vacant rooms, max is reset randomly
	
	public int LightsOn, LampsOn=2; // accessible to lamps and occupants
	private int LightsOnMax;
	public int numberOccupants=0;
	private GameObject[] peoplePrefabTypes=new GameObject[7];
	private Vector3[] spawnPositions;

	public string MapName(){
		if (!isUnlocked){
			return "Locked";
		} else if (!isOccupied) {
			return roomNumber;//"Vacant";
		} else {
			return roomNumber;
		}
	}

	/*======== ROOM MANAGEMENT ========*/
	/*
	private void ResetFurniture()
	{
		Debug.Log("Checking in to "+RoomName);
		bed1.position = bed1StartPos; bed1.eulerAngles=bed1angle;
		bed2.position = bed2StartPos; bed2.eulerAngles=bed2angle;
		bed3.position = bed3StartPos; bed3.eulerAngles=bed3angle;
		lamp1.position = lamp1StartPos; lamp1.eulerAngles=lamp1angle;
		lamp2.position = lamp2StartPos; lamp2.eulerAngles=lamp2angle;
		LampsOn = 2;
	}
	
	// Use this for initialization
	private void Start () {
		game=GameObject.Find("GameManager").GetComponent<Game>();
		ExitLocation = transform.FindChild ("Door").position;
		spawnPositions = new Vector3[]{
			transform.FindChild("Spawn 1").position,
			transform.FindChild("Spawn 2").position,
			transform.FindChild("Spawn 3").position
		};
		// Normalize to proper Z-depth
		spawnPositions[0] = new Vector3(spawnPositions[0].x, spawnPositions[0].y, 0);
		spawnPositions[1] = new Vector3(spawnPositions[1].x, spawnPositions[1].y, 0);
		spawnPositions[2] = new Vector3(spawnPositions[2].x, spawnPositions[2].y, 0);
		bed1 = transform.FindChild("Bed 1"); bed1angle=bed1.eulerAngles;
		bed2 = transform.FindChild("Bed 2"); bed2angle=bed2.eulerAngles;
		bed3 = transform.FindChild("Bed 3"); bed3angle=bed3.eulerAngles;
		lamp1 = transform.FindChild("Lamp 1"); lamp1angle=lamp1.eulerAngles;
		lamp2 = transform.FindChild("Lamp 2"); lamp2angle=lamp2.eulerAngles;
		Lamp1 = lamp1.GetComponent<LampObject>();
		Lamp2 = lamp2.GetComponent<LampObject>();
		bed1StartPos = bed1.position;
		bed2StartPos = bed2.position;
		bed3StartPos = bed3.position;
		//EntryLocation = transform.FindChild("Entry").position;
		lamp1StartPos = lamp1.position;
		lamp2StartPos = lamp2.position;
		CameraPosition = this.transform.FindChild("CameraPosition").position;
		roomNumber = transform.name.Split(' ')[1];
		//peoplePrefabTypes = Resources.LoadAll<GameObject>("Prefabs/Person");
		//print (peoplePrefabTypes.Length);
		peoplePrefabTypes[0]=Resources.Load<GameObject>("Prefabs/Person/Boy");
		peoplePrefabTypes[1]=Resources.Load<GameObject>("Prefabs/Person/Girl");
		peoplePrefabTypes[2]=Resources.Load<GameObject>("Prefabs/Person/Man");
		peoplePrefabTypes[3]=Resources.Load<GameObject>("Prefabs/Person/Lady");
		peoplePrefabTypes[4]=Resources.Load<GameObject>("Prefabs/Person/LightMan");
		peoplePrefabTypes[5]=Resources.Load<GameObject>("Prefabs/Person/LightLady");
		peoplePrefabTypes[6]=Resources.Load<GameObject>("Prefabs/Person/Priest");
	}

	private GameObject[] getNewCombo() {
		int i = UnityEngine.Random.Range(0,200)%(PersonLists.Combinations.Count);
		int[] array = PersonLists.Combinations[i];
		GameObject[] t = new GameObject[array.Length];
		for(int j=0; j<array.Length; j++){
			if (array[j]>-1) {
				t[j] = peoplePrefabTypes[array[j]];
			}
			else t[j]=null;
		}
		return t;
	}

	public void Unlock(float fInitial) {
		isUnlocked = true;
		vacantTimerMax = fInitial;
	}

	public void CheckIn(){
		// Prepare the room
		ResetFurniture ();
		isOccupied = true;
		LightsOnMax=2;
		GameObject temp;
		stayTimer = 0; vacantTimer = 0;
		stayTimerMax = UnityEngine.Random.Range(42f+numberOccupants*4,78f+numberOccupants*4);

		GameObject[] combo = getNewCombo();
		numberOccupants = combo.Length;
		occupants = new Person[numberOccupants];
		if (doorOpenSound!=null)
			AudioSource.PlayClipAtPoint (doorOpenSound, ExitLocation);

		for (int i=0; i<combo.Length; i++) {
			//Vector3 spawn = 

			temp = PrefabUtility.InstantiatePrefab (combo[i]) as GameObject;
			temp.transform.position = new Vector3(ExitLocation.x,ExitLocation.y,0);
			//temp = PrefabUtility.InstantiatePrefab(combo[i],spawn,Quaternion.identity) as Transform;
			occupants[i] = temp.GetComponent<Person>();
			occupants[i].SetDestination(spawnPositions[i]);
			if (occupants[i] is Person_Candle){
				LightsOnMax++;
			}
		}
		// this is called separately so occupants can set their roommates
		foreach (Person p in occupants){
			p.SetRoom(this);
		}
		game.NumOccupiedRooms++;
		//Debug.Log("Checking in "+numberOccupants+" people into room "+roomName);
		game.WriteText(numberOccupants+" potential victims checked into "+RoomName+".");
	}

	public float GetDuration()
	{
		return stayTimerMax - stayTimer;
	}

	public void CheckOut(){
		isOccupied=false;
		vacantTimer=0; stayTimer = 0;
		vacantTimerMax = UnityEngine.Random.Range(10f,15f); // delay to next check-in
		for (int i=numberOccupants-1; i>=0; i--) {
			if (occupants[i]!=null) {
				occupants[i].Leave ();
			}
		}
		numberOccupants=0;
		game.NumOccupiedRooms--;
		Lamp1.TurnOff ();
		Lamp2.TurnOff ();
//		Debug.Log("Checked out from room "+roomName +" lamps on ="+LampsOn);
		//if (doorCloseSound!=null)
		//	AudioSource.PlayClipAtPoint (doorCloseSound, transform.position);
		game.WriteText (RoomName+" has become vacant.");
	}

	public void TurnLightOn(){
//		print ("Turning light on");
		if (LampsOn<LightsOnMax){
			LampsOn++;
			LightsOn++;
		}
	}

	public void TurnLightOff(){
//		print ("Turning light off.");
		if (LampsOn>0){
			LightsOn--;
			LampsOn--;
		}
	}
	
	// Update is called once per frame
	private void Update () {
		// don't check out the room while the player is still looking at it
		if (isUnlocked) {
			if (isOccupied) {
				if (stayTimer<stayTimerMax) {
					stayTimer += GameVars.Tick*Time.deltaTime;
				}
				else if (numberOccupants>0){
					game.WriteText (RoomName+"'s occupants are checking out.");
					foreach(Person p in occupants){
						p.GoToDoor();
					}
				}
				else if (numberOccupants<1){// if (game.currentRoom!=this){
					CheckOut();
				}
			}
			else { // vacant
				if (vacantTimer<vacantTimerMax) {
					vacantTimer += GameVars.Tick*Time.deltaTime;
				}
				else {
					CheckIn ();
				}
			}
		}
	}*/
}
