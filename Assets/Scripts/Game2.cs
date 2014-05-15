using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

public class Game2 : MonoBehaviour {

	// Stuff we can leave open in the editor
	public int  money=0, fearEnergy=0;
	public bool START_AT_NIGHT=false;

	// don't edit these values:
	private float nightTimerRealSeconds =0f, GameMinutePerRealSecond;
	public float enemyGenCooldown=0f, roomCheckCountdown=0f;
	public const float enemyGenCooldownMax=20f, roomCheckCountdownMax=2f;
	public const int fearEnergyMax=100;
	private int days=0, currentRoomNumber=0, roomsOpen=0;
	public int Day{get { return days; }}
	public int CurrentRoomNumber { get { return currentRoomNumber; }}
	public int RoomsOpen {get {return roomsOpen; }}
	private const int nightDurationRealSecondsMax = 300; // real seconds per night round
	private const int nightDurationGameMinutes = 720; // 6pm to 6am =  12 hours * 60min/hr
	[HideInInspector] public Room[] rooms;
	private Room roomWithPriest, roomWithThug;
	private GameObject priestObject, thugObject;

	private List<Room> tempRoomList = new List<Room>(); // this is used for checking in priests and thugs
	private bool roomsHaveTraps=false, roomsHaveFurns=false, isChangingRooms=true;
	private float dyCam=99f;
	GameObject cam;
	private RaycastHit2D hit;
	private Ray ray;
	private Ability[] listAbilities;
	private Ability currentAbility;
	static string tooltip;
	//private int count; // what was this supposed to do?



	public AudioClip bgmDay;
	public AudioClip bgmNight;
	public AudioSource audioSource;

	// VARIABLES FOR DAYTIME INPUT AND UI GO DOWN HERE

	// Values used in UI
	private float percentNight;
	private Furniture[] furnitureTypes; // Viewable in daytime GUI menu - don't change
	private GameObject[] furniturePhysicalTypes; // use to instantiate from buy menu - don't change
	private int currentFurnitureIndex=-1;


	// =============================================== //
	// ================== INTERFACE ================== //
	// =============================================== //
	Texture2D fearBarTexture, fearBarTextureBlack, nightProgressTexture;
	Texture2D[] abilityIcons;
	float fearBarHeight=10f;
	
	private void RegisterHitDaytime(RaycastHit2D hit){
		if (Input.GetMouseButtonDown (0)){
			Debug.Log ("Hit: "+hit.collider.gameObject.name);
			Node n = hit.collider.gameObject.GetComponent<Node>();
			if (hit.collider.CompareTag ("Furniture")){
				Sell (hit.collider.gameObject);
			} else if (GameVars.IsPlacingFurniture && currentFurnitureIndex >= 0){
				if (hit.collider.CompareTag ("Node")){
					if (Buy (furnitureTypes[currentFurnitureIndex],hit)){
						n.Add (furniturePhysicalTypes[currentFurnitureIndex]);
					}
				}
				currentFurnitureIndex=-1;
			}
		}
	}
	
	[ExecuteInEditMode]
	private void OnGUI(){
		if (Time.timeScale<=0) return;
		if (GameVars.IsNight){

			//Debug.Log (((float)fearEnergy)/fearEnergyMax);
			GUI.DrawTexture (new Rect(0,Screen.height-fearBarHeight,Screen.width,fearBarHeight),fearBarTextureBlack,ScaleMode.StretchToFill);
			GUI.DrawTexture (new Rect(0,Screen.height-fearBarHeight,((float)Screen.width*fearEnergy)/fearEnergyMax,fearBarHeight),fearBarTexture,ScaleMode.StretchToFill);
			//count = 0;
			for (int i=0;i<listAbilities.Length;i++){
				if(fearEnergy < listAbilities[i].minFear){ 
					GUI.contentColor=Color.gray;
				} else {
					if (listAbilities[i].isCooldown)
						GUI.contentColor=Color.yellow;
					else if (currentAbility==listAbilities[i])
						GUI.contentColor=Color.green;
					else
						GUI.contentColor=Color.white;
				}

				if (GUI.Button (new Rect (i*Screen.width/listAbilities.Length, Screen.height-40-fearBarHeight,
				                          Screen.width/listAbilities.Length, 40), new GUIContent(listAbilities [i].ShowName(),
				                          listAbilities[i].ShowName() + ": " + listAbilities[i].Description
				                          + " Costs " + listAbilities[i].minFear))) {
					//cursorAppearance.SetSprite (2);
					if(fearEnergy >= listAbilities[i].minFear){
						currentAbility = listAbilities[i];
					}
				}
				GUI.Label(new Rect(i*Screen.width/listAbilities.Length, Screen.height-60-fearBarHeight,
				                	Screen.width/listAbilities.Length, 60), GUI.tooltip);
				GUI.tooltip = null;

			}
		} else {
			// daytime GUI
			int index=0;
//			Debug.Log(currentFurnitureIndex);
			for (int row=2; row>=1; row--) {
				for (int x=0;x<furnitureTypes.Length/2;x++){
					if (currentFurnitureIndex == index){
						GUI.contentColor=Color.green;
					} else if (money < furnitureTypes[index].buyCost){
						GUI.contentColor=Color.gray;
					} else {
						GUI.contentColor=Color.white;
					}
					if (GUI.Button (new Rect(x*Screen.width/(furnitureTypes.Length/2),
					                         Screen.height-row*30,
					                         Screen.width/(furnitureTypes.Length/2),
					                         30), new GUIContent(furnitureTypes[index].DisplayName, 
					                          furnitureTypes[index].DisplayName + ": " + furnitureTypes[index].description
					                    	   + " " + furnitureTypes[index].buyCost + " money"))){
						if (money >= furnitureTypes[index].buyCost) {
							Debug.Log("Placing furniture: "+furnitureTypes[index].name);
							currentFurnitureIndex = index;
							GameVars.IsPlacingFurniture=true;
						}
					}
					GUI.Label (new Rect(x*Screen.width/(furnitureTypes.Length/2),
					                    630,
					                    Screen.width/(furnitureTypes.Length/2),
					                    40), GUI.tooltip);
					//GUI.tooltip = null;
					index++;
				}
			}
		}
	}

	private void BuyRoom(){
		if (roomsOpen < rooms.Length){
			rooms[roomsOpen].Buy ();
			roomsOpen++;
		}
		Debug.Log ("Rooms unlocked = "+roomsOpen);
	}

	private bool Buy(Furniture f, RaycastHit2D hit){
		if (money >= f.buyCost){
			money -= f.buyCost;
			return true;
			//hit.collider.GetComponent<Room>().AddFurniture (f);
			// get new furniture thing
			// place it somewhere
		} else {
			return false;
			// Not enough money!
		}
	}
	private void Sell(GameObject obFurn){
		money += obFurn.GetComponent<Furniture>().buyCost;
		obFurn.GetComponent<Furniture>().node.Clear ();
	}
	


	// Update is called once per frame
	private void Update () {
		if (Input.GetKeyDown("m"))
			BuyRoom ();
		if (Day==1){
			if (START_AT_NIGHT){
				START_AT_NIGHT = false; // only run once
				StartNight ();
			} else {
				StartDay ();
			}
		}
		if (Input.GetKeyDown("b")){
			if (GameVars.IsNight)
				StartDay ();
			else
				StartNight ();
		}
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
		if (GameVars.IsNight){
			if (nightTimerRealSeconds<nightDurationRealSecondsMax){
				nightTimerRealSeconds += Time.deltaTime; // here it makes sense to count up, not down
				percentNight = (((float)nightTimerRealSeconds)/nightDurationRealSecondsMax);
			} else {
				StartDay ();
			}
			if (roomsHaveFurns || roomsHaveTraps){
				if (enemyGenCooldown > 0f){
					enemyGenCooldown-=Time.deltaTime;
				} else {
					if (roomsHaveTraps && UnityEngine.Random.value<0.5f){
						if(roomWithPriest==null)
							CheckInPriest ();
					}
					else if (roomsHaveFurns && roomWithThug==null)
						CheckInThug();
				}
			}
		} else {
			// DAYTIME GAME LOGIC
		}
		// CLICKING ON STUFF
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		hit = Physics2D.Raycast(ray.origin,ray.direction);
		if (hit){
			RegisterHit(hit);
		}
	}

	// don't change
	public string DigiClock(){
		int mins = Mathf.FloorToInt(nightTimerRealSeconds*GameMinutePerRealSecond);
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

	// =============================================== //
	// ==================== ENEMIES ================== //
	// =============================================== //

	// called from Room when the Thug or Priest exits it
	public void CheckOutEnemy(Person2 p, bool stay){
		bool isThug=false;
		if (p is Person2_Priest){
			isThug=false;
			roomWithPriest=null;
		} else {
			isThug=true;
			roomWithThug=null;
		}
		if (!stay){
			if (isThug)
				(p as Person2_Thug).Reset ();
			else
				(p as Person2_Priest).Reset ();
			enemyGenCooldown=2.5f*enemyGenCooldownMax;
		} else
			enemyGenCooldown=enemyGenCooldownMax;
	}

	// Find a room with at least 1 trap, and put the priest in it
	void CheckInPriest(){
		tempRoomList.Clear ();
		foreach (Room r in rooms){
			if (r.TrapCount()>0){
				tempRoomList.Add (r);
			}
		}
		if (tempRoomList.Count>0){
			int roomIndex=UnityEngine.Random.Range (0,tempRoomList.Count-1);
			roomWithPriest = tempRoomList[roomIndex];
			roomWithPriest.AddEnemy(priestObject);
			enemyGenCooldown = enemyGenCooldownMax;
			Debug.Log("WARNING - PRIEST HAS ENTERED INTO "+roomWithPriest.name);
		}
	}

	// Find a room with at least 1 furniture, and put the thug in it
	void CheckInThug(){
		tempRoomList.Clear ();
		foreach (Room r in rooms){
			if (r.NonTrapFurnitureCount()>0){
				tempRoomList.Add (r);
			}
		}
		if (tempRoomList.Count>0){
			int roomIndex=UnityEngine.Random.Range (0,tempRoomList.Count-1);
			roomWithThug = tempRoomList[roomIndex];
			roomWithThug.AddEnemy(thugObject);
			enemyGenCooldown = enemyGenCooldownMax;
			Debug.Log("WARNING - THUG HAS ENTERED INTO "+roomWithThug.name);
		}
	}

	// =============================================== //
	// ================= GAME SETUP ================== //
	// =============================================== //
	
	private void Start() {

		cam = Camera.main.transform.gameObject; // two distinct references?
		// Set up rooms
		rooms = new Room[GameObject.FindGameObjectsWithTag ("Room").Length];
		for (int i=0;i<rooms.Length;i++){
			rooms[i] = GameObject.Find ("Room "+(i+1)).GetComponent<Room>();
			rooms[i].Cost = 1000 + 500*i;
			if (!rooms[i].HASLOADED){
				wait (0.1f);
			}
			if (rooms[i].open){
				BuyRoom();
			}
		}
		rooms[0].open=true;
		// Maybe the camera wasn't at the right place when game started - that's okay
		Vector3 d3 = rooms[0].transform.position - cam.transform.position;
		cam.transform.position += new Vector3(d3.x,d3.y,0);

		GameMinutePerRealSecond = ((float)nightDurationGameMinutes/nightDurationRealSecondsMax);
		GameVars.WallLeft=rooms[0].XLeft;
		GameVars.WallRight=rooms[0].XRight;
		GameVars.WallLeftSoft=GameVars.WallLeft+2f;
		GameVars.WallRightSoft=GameVars.WallRight-2f;
		fearBarTexture = Resources.Load<Texture2D>("Sprites/gui/fearprogress-purple");
		fearBarTextureBlack = Resources.Load<Texture2D>("Sprites/gui/fearprogress-black");
		nightProgressTexture = Resources.Load<Texture2D>("Sprites/gui/nightskyTexture");
		abilityIcons = Resources.LoadAll<Texture2D>("Sprites/gui/abilityicons");
		//GameVars.hpBarRed = 
		//GameVars.hpBarGreen = Resources.Load<Texture2D>("Sprites/gui/hpBarGreen");
		listAbilities = new Ability[5]; // these are attached to the Main Game transform
		listAbilities[0] = transform.GetComponent<Ability_Ghost>();
		listAbilities[1] = transform.GetComponent<Ability_Repair>();
		listAbilities[2] = transform.GetComponent<Ability_Claw>();
		listAbilities[3] = transform.GetComponent<Ability_Monster>();
		listAbilities[4] = transform.GetComponent<Ability_Possess>();
		// Load all of the resources once - everything else you can just Instantiate()
		furniturePhysicalTypes = Resources.LoadAll<GameObject>("Prefabs/Furniture");
		furnitureTypes = new Furniture[furniturePhysicalTypes.Length];

		for (int i=0; i<furniturePhysicalTypes.Length; i++){
			furnitureTypes[i]=furniturePhysicalTypes[i].GetComponent<Furniture>();
		}
		priestObject = Resources.Load<GameObject>("Prefabs/Person/Priest");
		thugObject = Resources.Load<GameObject> ("Prefabs/Person/Thug");
		//GameVars.pickupCoin=Resources.Load<GameObject>("Prefabs/PickupCoin");
		//GameVars.pickupFear=Resources.Load<GameObject>("Prefabs/PickupFear");
		int alayer = 1 << LayerMask.NameToLayer("PersonLayer");
		int blayer = 1 << LayerMask.NameToLayer("FurnitureLayer");
		GameVars.interactLayer = (alayer | blayer);
		audioSource = Camera.main.transform.Find ("Sound").GetComponent<AudioSource> ();
	}

	private IEnumerator wait(float time){
		yield return new WaitForSeconds(time);
	}

	private void StartDay(){
		Debug.Log ("Starting day...");
		bgmDay = Resources.Load<AudioClip> ("Sounds/bgm_final_day");
		audioSource.Stop ();
		audioSource.loop = true;
		audioSource.volume = 1;
		audioSource.clip = bgmDay;
		audioSource.Play ();
		GameVars.IsNight=false;
		days++;
		foreach (Room r in rooms){
			r.CheckOut ();
			r.DisplayGrid (true);
		}
	}
	
	public void StartNight(){
		GameVars.IsNight=true;
		Debug.Log ("Starting night..");
		bgmNight = Resources.Load<AudioClip> ("Sounds/bgm_final_night");
		audioSource.Stop ();
		audioSource.loop = true;
		audioSource.volume = 1;
		audioSource.clip = bgmNight;
		audioSource.Play ();
		foreach (Room r in rooms){
			r.DisplayGrid (false);
		}
		foreach (Room r in rooms){

//			Debug.Log (name + " is open: "+r.open);
			if (r.open){
				r.CheckIn();
			}
			if (!roomsHaveFurns)
				roomsHaveFurns = (bool)(r.NonTrapFurnitureCount()>0);
			if (!roomsHaveTraps)
				roomsHaveTraps = (bool)(r.TrapCount()>0);
		}
		// No non-trap furniture = no thug tonight
		// No traps = no priest tonight
		Debug.Log ("Checking for traps..."+roomsHaveTraps);
		Debug.Log ("Checking for furniture..."+roomsHaveFurns);
		if (roomsHaveFurns || roomsHaveTraps)
			enemyGenCooldown=enemyGenCooldownMax;
	}
	
	private void RegisterHit(RaycastHit2D hit){
//		Debug.Log (hit.collider.gameObject.name);
		if (hit.collider.gameObject.CompareTag ("Money")){
			DestroyObject (hit.collider.gameObject);
			money += 10;
		} else if (hit.collider.gameObject.CompareTag("FearPickup")){
			DestroyObject (hit.collider.gameObject);
			fearEnergy++;
		} else if (hit.collider.gameObject.CompareTag ("Person")){
			Person2 p = hit.collider.gameObject.GetComponent<Person2>();
			p.DisplayHP ();
		} else if (hit.collider.gameObject.CompareTag ("Furniture")){
			Furniture f = hit.collider.gameObject.GetComponent<Furniture>();
			f.DisplayHP();
		}
		if (Input.GetMouseButtonDown (0)){
			Debug.Log ("Clicked on "+hit.collider.name);
			if (currentRoomNumber<roomsOpen && hit.collider.gameObject.CompareTag("Triangle Up")){
				currentRoomNumber++;
				isChangingRooms=true;
			} else if (currentRoomNumber > 0 && hit.collider.gameObject.CompareTag("Triangle Down")){
				currentRoomNumber--;
				isChangingRooms=true;
			}
		}
		if (!GameVars.IsNight) {
			RegisterHitDaytime (hit);
		} else if (Input.GetMouseButtonDown (0)){
			if (currentAbility==listAbilities[4] && listAbilities[4].CanUse () && hit.collider.gameObject.CompareTag ("Person")){
				currentAbility.UseAbility (hit);
				//currentAbility=null; 
			} else if (hit.collider.gameObject.CompareTag ("Furniture")){
				Furniture f = hit.collider.gameObject.GetComponent<Furniture>();
				if (currentAbility==listAbilities[1] && listAbilities[1].CanUse()){
					//Debug.Log (hit.collider.name);
					currentAbility.UseAbility (hit);
				} else if (f is Lamp){
					Lamp l = hit.collider.gameObject.GetComponent<Lamp>();
					if (l.Durability>0)
						l.Flip ();
				} else if (f is Trap && !(f as Trap).Used) {
					Debug.Log("Activating");
					(f as Trap).Activate ();
				}
			} else if (currentAbility!=null && currentAbility!=listAbilities[1] && currentAbility!=listAbilities[4]) {
				if (hit.collider.gameObject.CompareTag ("Room") || hit.collider.gameObject.CompareTag ("Node") ||
				    hit.collider.gameObject.CompareTag ("Furniture")){
					if (currentAbility.CanUse ()){
						currentAbility.UseAbility (hit);
						//currentAbility=null;
					}
				}
			}
		}

	}



}
