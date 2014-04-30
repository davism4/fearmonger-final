using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game2 : MonoBehaviour {
	/**
	 * bonusLevel = restarts each night, determines $ bonuses from fear
	 * xp = collected fear, restarts each night
	 * xpNextBonusLevel = fear to next "level up"
	 */

	private float nightTimerRealSeconds =0f, GameMinutePerRealSecond; // don't edit this
	public int money=0, fearLevel=0;
	/* Gametime config:
	 * This is a way to configure how long a nighttime round lasts, both
	 * in real time and in the game time. Example: real time round is
	 * 5 minutes, but in the game it is from 6PM to 6AM, so there are
	 * 12 game hours per 5 real minutes. This is used to update the 
	 * digital clock in game.
	 * The default values are below
	 */
	private const int nightDurationRealSeconds = 300; // real seconds per night round
	private const int nightDurationGameMinutes = 720; // 6pm to 6am =  12 hours * 60min/hr

	//LayerMask defaultLayers = LayerMask.NameToLayer("Default");
	//GameObject[] roomGameObjects;
	[HideInInspector] public Room[] rooms;
	[HideInInspector] public List<Room> roomsWithTraps;
	public int currentRoomNumber=0;
	private bool isChangingRooms=true;
	private float dyCam=99f;

	public int days=0;
	GameObject cam;

	// Input
	public Ability[] listAbilities;
	public Ability currentAbility;
	//private RaycastHit2D hit;
	private RaycastHit2D hit;
	private Ray ray;
	
	// PUBLIC FUNCTIONS
	void StartDay(){
		GameVars.IsNight=false;
		days++;
	}
	
	void StartNight(){
		GameVars.IsNight=true;
		foreach (Room r in rooms){
			if (r.enabled)
				r.CheckIn();
		}
	}

	public void Buy(Furniture f){
		if (money >= f.buyCost){
			money -= f.buyCost;
			// get new furniture thing
		} else {
			// Not enough money!
		}
	}

	// digital clock readout of time
	public string DigiClock(){
		int mins = Mathf.FloorToInt(nightTimerRealSeconds*GameMinutePerRealSecond);
		//int realSeconds = Mathf.FloorToInt(nightTimerRealSeconds);
		int minute = mins%60;
		int hour = 6 + mins/60; // start at 6 pm
		string strMinute = minute<10? ("0"+ minute) : (""+minute);
		if (hour < 10 ) // 06:00 PM to 09:59 PM
			return ("0"+hour + ":" + strMinute + " PM");
		else if (hour < 12 ) // 10:00 PM to 11:59 PM
			return (hour + ":" + strMinute + " PM");
		else if (hour == 12) // 12:00 AM to 12:59 AM
			return ("12:" + strMinute + " AM");
		else // 01:00 AM to 05:59 AM
			return ("0"+ (hour - 12) + ":" + strMinute + " AM");
	}

	// PRIVATE FUNCTIONS 
	private void Start () {
		cam = Camera.main.transform.gameObject; // two distinct references
		rooms = new Room[GameObject.FindGameObjectsWithTag ("Room").Length];
		for (int i=1;i<=rooms.Length;i++){
			rooms[i-1] = GameObject.Find ("Room "+i).GetComponent<Room>();
		}
		rooms[0].enabled=true;
		// Maybe the camera wasn't at the right place when game started - that's okay
		Vector3 d3 = rooms[0].transform.position - cam.transform.position;
		cam.transform.position += new Vector3(d3.x,d3.y,0);

		GameVars.IsNight=true;
		GameMinutePerRealSecond = ((float)nightDurationGameMinutes/nightDurationRealSeconds);
		GameVars.WallLeft=rooms[0].XLeft;
		GameVars.WallRight=rooms[0].XRight;
		GameVars.WallLeftSoft=GameVars.WallLeft+2f;
		GameVars.WallRightSoft=GameVars.WallRight-2f;
		//listAbilities = GameObject.FindObjectsOfType<Ability>();
		listAbilities = new Ability[5];
		listAbilities[0] = transform.GetComponent<Ability_Spiders>();
		listAbilities[1] = transform.GetComponent<Ability_Darkness>();
		listAbilities[2] = transform.GetComponent<Ability_Claw>();
		listAbilities[3] = transform.GetComponent<Ability_Monster>();
		listAbilities[4] = transform.GetComponent<Ability_Possess>();

		int a = 1 << LayerMask.NameToLayer("PersonLayer");
		int b = 1 << LayerMask.NameToLayer("FurnitureLayer");
		GameVars.interactLayer = (a | b);
	}

	private void BuyAbility(int index){
		if (listAbilities[index].Locked){
			listAbilities[index].Unlock();
			//money -= listAbilities[index].BuyCost;
		}
	}

	private void OnGUI(){
		for (int i=0;i<listAbilities.Length;i++){
			if(fearLevel < listAbilities[i].minFearCost){ 
				//if (listAbilities[i].BuyCost <= money)
				//	GUI.contentColor=Color.white;
				//else
					GUI.contentColor=Color.gray;
			} else {
				if (listAbilities[i].isCooldown)
					GUI.contentColor=Color.yellow;
				else if (currentAbility==listAbilities[i])
					GUI.contentColor=Color.green;
				else
					GUI.contentColor=Color.white;
			}
			if (GUI.Button (new Rect (i*Screen.width/5, Screen.height-40, Screen.width/5, 40), listAbilities [i].ShowName())) {
				//cursorAppearance.SetSprite (2);
				if(fearLevel >= listAbilities[i].minFearCost){
					currentAbility = listAbilities[i];
				}
			}
			
		}
	}
	
	// Update is called once per frame
	private void Update () {
		// Change this later:
//		//Debug.Log (DigiClock());

		// camera slide effect between rooms
		if (isChangingRooms){
			dyCam = cam.transform.position.y - rooms[currentRoomNumber].transform.position.y;
			if (dyCam>1f)
				cam.transform.position += 20f*Vector3.down*Time.deltaTime; // Camera up/down speed
			else if (dyCam<-1f)
				cam.transform.position += 20f*Vector3.up*Time.deltaTime; // Camera up/down speed
			else if (Mathf.Abs (dyCam)>0.001f)
				cam.transform.position += 20*Vector3.down*dyCam*Time.deltaTime;
			else
				isChangingRooms=false;
		}

		/**
		  * Handle clicking on stuff
		  * Mouse over $ -> collect $
		  * Click on up/down triangle -> change floor
		  * Click on lamp -> switch lamp on/off
		  * Click on ability's icon -> select ability
		  * Click anywhere else at night -> use ability
		  * Click on stuff during daytime?
		  */
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		hit = Physics2D.Raycast(ray.origin,ray.direction);
		if (hit){
			//Debug.Log (hit.collider.gameObject.name);
			if (hit.collider.gameObject.CompareTag ("Money")){
				DestroyObject (hit.collider.gameObject);
				money++;
			} else if (hit.collider.gameObject.CompareTag("FearPickup")){
				DestroyObject (hit.collider.gameObject);
				fearLevel++;
			} else if (hit.collider.gameObject.CompareTag ("Person")){
				Person2 p = hit.collider.gameObject.GetComponent<Person2>();
				p.DisplayHP ();
				//Debug.Log("Person");
			} else if (hit.collider.gameObject.CompareTag ("Furniture")){
				Furniture f = hit.collider.gameObject.GetComponent<Furniture>();
				
			}
			if (Input.GetMouseButtonDown (0)){
				if (currentRoomNumber<rooms.Length-1 && hit.collider.gameObject.CompareTag("Triangle Up")){
					//Debug.Log("Going up");
					currentRoomNumber++;
					isChangingRooms=true;
				} else if (currentRoomNumber > 0 && hit.collider.gameObject.CompareTag("Triangle Down")){
					//Debug.Log("Going down");
					currentRoomNumber--;
					isChangingRooms=true;
				} else if (hit.collider.gameObject.CompareTag ("Lamp")){
					//Debug.Log ("Switched lamp");
					Lamp l = hit.collider.gameObject.GetComponent<Lamp>();
					l.Flip ();
				} else if (false) { // select ability from its icon
					// select that as active ability
					// show info
				} else if (GameVars.IsNight && currentAbility!=null) {
					if (hit.collider.gameObject.CompareTag ("Room") || hit.collider.gameObject.CompareTag ("Furniture")){
						if (currentAbility.CanUse ()){
							currentAbility.Activate (hit.point);

						} else {
							//Debug.Log ("can't use ability!");
						}
					}
				} else { 
					// it's day time
				}
			}
		}

		if (GameVars.IsNight){
			if (nightTimerRealSeconds<nightDurationRealSeconds){
				nightTimerRealSeconds += Time.deltaTime; // here it makes sense to count up, not down
			} else {
				StartDay ();
			}
			// nighttime logic

		} else {
			// Daytime logic
		}
	}
}
