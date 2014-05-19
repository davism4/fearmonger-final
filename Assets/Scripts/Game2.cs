using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game2 : MonoBehaviour {

	// Stuff we can leave open in the editor
	public int  money=0, fearEnergy=0;
	public bool START_AT_NIGHT=false;
	public float BASE_CAM_SPEED = 25f;

	// don't edit these values:
	private float nightTimerRealSeconds =0f, GameMinutePerRealSecond;
	public float enemyGenCooldown=0f, roomCheckCountdown=0f;
	private const float enemyGenCooldownMax=20f, roomCheckCountdownMax=2f;
	private const int fearEnergyMax=100;
	private int days=0, currentRoomNumber=0;
	public int Day{get { return days; }}
	public int CurrentRoomNumber { get { return currentRoomNumber; }}
	public int RoomsOpen=0;
	private const int nightDurationRealSecondsMax = 180; // real seconds per night round
	private const int nightDurationGameMinutes = 720; // 6pm to 6am =  12 hours * 60min/hr
	public Room[] rooms;
	private Room roomWithPriest, roomWithThug;
	private GameObject priestObject, thugObject;
	private Padlock padlock;
	private List<Room> tempRoomList = new List<Room>(); // this is used for checking in priests and thugs
	private bool roomsHaveTraps=false, roomsHaveFurns=false, isChangingRooms=true;
	private Vector3 dvCam=Vector3.zero;//private float dyCam=99f;
	private GameObject cam;
	private RaycastHit2D hit;
	private Ray ray;
	private Ability[] listAbilities;
	private Ability currentAbility;
	static string tooltip;
	public GUIStyle style;
	public Font mytype;

	//private int count; // what was this supposed to do?

	private Sound sound; // which sound is this referring to? It is referenced in Start() 
	private AudioClip collectSound;
	private AudioClip collectFear;
	private AudioClip clickSound;
	private AudioClip lampSwitch;

	// VARIABLES FOR DAYTIME INPUT AND UI GO DOWN HERE

	// Values used in UI
	
	private Furniture[] furnitureTypes; // Viewable in daytime GUI menu - don't change
	private GameObject[] furniturePhysicalTypes; // use to instantiate from buy menu - don't change
	private int currentFurnitureIndex=-1;
	public float percentNight {
		get { return (((float)nightTimerRealSeconds)/nightDurationRealSecondsMax); }}

	// =============================================== //
	// ================== INTERFACE ================== //
	// =============================================== //
	private Texture2D fearBarTexture, fearBarTextureBlack;
	private Texture2D[] abilityIcons;
	private float fearBarHeight=10f;
	
	private void RegisterHitDaytime(RaycastHit2D hit){
		if (Input.GetMouseButtonDown (0)){
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
		GUI.skin.font = mytype;
		if (Time.timeScale<=0) return;
		if (GameVars.IsNight){

			//Debug.Log (((float)fearEnergy)/fearEnergyMax);
			GUI.DrawTexture (new Rect(0,Screen.height-40-fearBarHeight,Screen.width,40+fearBarHeight),fearBarTextureBlack,ScaleMode.StretchToFill);
			GUI.DrawTexture (new Rect(0,Screen.height-40-fearBarHeight,((float)Screen.width*fearEnergy)/fearEnergyMax,40+fearBarHeight),fearBarTexture,ScaleMode.ScaleAndCrop);

			//count = 0;
			for (int i=0;i<listAbilities.Length;i++){
				GUI.backgroundColor = Color.magenta;
				GUI.contentColor = Color.green;
				if(fearEnergy < listAbilities[i].minFear){ 
					GUI.contentColor=Color.gray;
				} else {
					if (listAbilities[i].isCooldown)
						GUI.contentColor=Color.yellow;
					else if (currentAbility==listAbilities[i])
						GUI.backgroundColor=Color.green;
				}

				if (GUI.Button (new Rect (i*Screen.width/listAbilities.Length, Screen.height-40-fearBarHeight,
				                          Screen.width/listAbilities.Length, 40), new GUIContent(listAbilities[i].ShowName(),
				                          listAbilities[i].ShowName() + ": " + listAbilities[i].Description
				                          + " Costs " + listAbilities[i].minFear + " Fear"))) {
					//cursorAppearance.SetSprite (2);
					if(fearEnergy >= listAbilities[i].minFear){
						currentAbility = listAbilities[i];
					}
				}
				GUI.contentColor = Color.white;
				GUI.Label(new Rect(i*Screen.width/listAbilities.Length, Screen.height-140-fearBarHeight,
				                	Screen.width/listAbilities.Length, 100), GUI.tooltip);
				GUI.tooltip = null;

			}
		} else {
			// daytime GUI
			int index=0;
			GUI.backgroundColor = Color.magenta;
			GUI.contentColor = Color.green;
//			Debug.Log(currentFurnitureIndex);
			for (int row=2; row>=1; row--) {
				for (int x=0;x<furnitureTypes.Length/2;x++){
					GUI.contentColor = Color.green;
					if (currentFurnitureIndex == index){
						GUI.backgroundColor=Color.green;
					} else if (money < furnitureTypes[index].buyCost){
						GUI.contentColor=Color.gray;
					}
					else
						GUI.backgroundColor = Color.magenta;
					if (GUI.Button (new Rect(x*Screen.width/(furnitureTypes.Length/2),
					                         Screen.height-row*30,
					                         Screen.width/(furnitureTypes.Length/2),
					                         30), new GUIContent(furnitureTypes[index].DisplayName + ": $" +furnitureTypes[index].buyCost, 
					                          furnitureTypes[index].DisplayName + ": " + furnitureTypes[index].description))){
						if (money >= furnitureTypes[index].buyCost) {
							Debug.Log("Placing furniture: "+furnitureTypes[index].name);
							currentFurnitureIndex = index;
							GameVars.IsPlacingFurniture=true;
						}
					}
					GUI.contentColor = Color.white;
					GUI.Label (new Rect(x*Screen.width/(furnitureTypes.Length/2),
					                    630,
					                    Screen.width/(furnitureTypes.Length/2),
					                    60), GUI.tooltip);
					GUI.tooltip = null;
					index++;
				}
			}
		}
		GUI.skin.font = null;
	}

	private void BuyRoom(){
		Debug.Log ("buy room");
		if (money>= rooms[RoomsOpen].Cost){
			if (RoomsOpen < rooms.Length){
				rooms[RoomsOpen].Buy ();
				RoomsOpen++;
			}
	//			Debug.Log ("Buying a room. There are now "+RoomsOpen+" open rooms.");
			if (RoomsOpen < rooms.Length) {
	//			Debug.Log ("Moving padlock to floor "+(RoomsOpen+1));
				padlock.MoveToFloor(RoomsOpen);
			}
			else {
	//			Debug.Log ("Destroying padlock");
				padlock.Delete ();
				padlock=null;
			}
		} else {
			Debug.Log ("You need "+rooms[RoomsOpen].Cost+" to open a new floor.");
		}
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
	

	public void CheckEmptyHotel(){
		foreach (Room r in rooms){
			if (r.occupants.Count>0)
				return;
		}
		Debug.Log ("Nobody is left in the hotel!");
		StartDay ();
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
				money += 10*fearEnergy;
				fearEnergy=0;
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
			dvCam = rooms[currentRoomNumber].transform.position - cam.transform.position;
			dvCam = new Vector3(dvCam.x,dvCam.y,0);
			if (dvCam.magnitude>1f)
				cam.transform.position += BASE_CAM_SPEED*dvCam.normalized*Time.deltaTime; // Camera up/down speed
			else if (dvCam.magnitude<-1f)
				cam.transform.position += BASE_CAM_SPEED*dvCam.normalized*Time.deltaTime; // Camera up/down speed
			else if (dvCam.magnitude>0.001f)
				cam.transform.position += BASE_CAM_SPEED*dvCam*Time.deltaTime;
			else
				isChangingRooms=false;
		}
		if (GameVars.IsNight){
			if (nightTimerRealSeconds<nightDurationRealSecondsMax){
				nightTimerRealSeconds += Time.deltaTime; // here it makes sense to count up, not down
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
		padlock = GameObject.Find ("Padlock").GetComponent<Padlock>();
		cam = Camera.main.transform.gameObject; // two distinct references?
		// Set up rooms
		//rooms = new Room[GameObject.FindGameObjectsWithTag ("Room").Length];
		//Debug.Log (rooms.Length);
	//	foreach (Room r in rooms){
	//		Debug.Log (r.transform.name);
	//	}
		for (int i=0;i<rooms.Length;i++){
	//		string s = "Room "+(i+1).ToString ();
	//		GameObject o = GameObject.Find (s);
	//		Debug.Log ("Room "+s+" found: "+(bool)(o==null));
	//		rooms[i] = GameObject.Find ("Room "+(i+1).ToString ()).GetComponent<Room>();
			rooms[i].Cost = 500*(1+i);
			rooms[i].Start();
			if (i<RoomsOpen){
				rooms[i].Buy ();
			}
		}
//		Debug.Log("There are now "+RoomsOpen+" open rooms.");
		padlock.Initialize(rooms);
		padlock.MoveToFloor(RoomsOpen);
		// Maybe the camera wasn't at the right place when game started - that's okay
//		Vector3 d3 = rooms[0].transform.position - cam.transform.position;
//		cam.transform.position += new Vector3(d3.x,d3.y,0);

		GameMinutePerRealSecond = ((float)nightDurationGameMinutes/nightDurationRealSecondsMax);
		GameVars.WallLeft=transform.FindChild("Marker LeftWall").transform.position.x;//rooms[0].XLeft;
		GameVars.WallRight=transform.FindChild("Marker RightWall").transform.position.x;//rooms[0].XRight;
		GameVars.WallLeftSoft=transform.FindChild("Marker LeftSoftWall").transform.position.x;//=GameVars.WallLeft+2f;
		GameVars.WallRightSoft=transform.FindChild("Marker RightSoftWall").transform.position.x;//=GameVars.WallRight-2f;
		Debug.Log ("Walls: "+GameVars.WallLeft);
		Debug.Log ("Wall left (soft) = "+GameVars.WallLeftSoft);
		Debug.Log ("Walls: "+GameVars.WallRight);
		Debug.Log ("Wall right (soft) = "+GameVars.WallRightSoft);
		fearBarTexture = Resources.Load<Texture2D>("Sprites/gui/fearprogress-purple");
		fearBarTextureBlack = Resources.Load<Texture2D>("Sprites/gui/fearprogress-black");
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
		if (furniturePhysicalTypes==null || furnitureTypes==null)
			Debug.LogError ("Missing furniture types");
		priestObject = Resources.Load<GameObject>("Prefabs/Person/Priest");
		thugObject = Resources.Load<GameObject> ("Prefabs/Person/Thug");
		//GameVars.pickupCoin=Resources.Load<GameObject>("Prefabs/PickupCoin");
		//GameVars.pickupFear=Resources.Load<GameObject>("Prefabs/PickupFear");
		int alayer = 1 << LayerMask.NameToLayer("PersonLayer");
		int blayer = 1 << LayerMask.NameToLayer("FurnitureLayer");
		GameVars.interactLayer = (alayer | blayer);
		sound = this.GetComponent<Sound> ();
		collectSound = Resources.Load<AudioClip> ("Sounds/collect");
		collectFear = Resources.Load<AudioClip> ("Sounds/collect_fear");
		clickSound = Resources.Load<AudioClip> ("Sounds/click");
		lampSwitch = Resources.Load<AudioClip> ("Sounds/lamp_switch");
	}

	private IEnumerator wait(float time){
		yield return new WaitForSeconds(time);
	}

	private void StartDay(){
		Debug.Log ("Starting day...");
		if (sound != null)
			sound.playBgmDay ();
		else {
			Debug.LogWarning ("Day background music missing@");
		}
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
		if (sound != null)
			sound.playBgmNight ();
		else
			Debug.LogWarning ("Night background music missing");
		foreach (Room r in rooms){
			r.DisplayGrid (false);
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
			if(collectSound != null)
				AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position);
			money += 10;
		} else if (hit.collider.gameObject.CompareTag("FearPickup")){
			DestroyObject (hit.collider.gameObject);
			if(collectFear != null)
				AudioSource.PlayClipAtPoint (collectFear, Camera.main.transform.position);
			fearEnergy++;
		} else if (hit.collider.gameObject.CompareTag ("Person")){
			Person2 p = hit.collider.gameObject.GetComponent<Person2>();
			p.DisplayHP ();
		} else if (hit.collider.gameObject.CompareTag ("Furniture")){
			Furniture f = hit.collider.gameObject.GetComponent<Furniture>();
			f.DisplayHP();
		}
		if (Input.GetMouseButtonDown (0)){
			Debug.Log ("Hit: "+hit.collider.gameObject.name);
//			Debug.Log ("Clicked on "+hit.collider.name);
			if (currentRoomNumber<RoomsOpen && hit.collider.gameObject.CompareTag("Triangle Up")){
				if(clickSound != null)
					AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				currentRoomNumber++;
				isChangingRooms=true;
			} else if (currentRoomNumber > 0 && hit.collider.gameObject.CompareTag("Triangle Down")){
				if(clickSound != null)
					AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
				currentRoomNumber--;
				isChangingRooms=true;
			}
		}
		if (!GameVars.IsNight) {
			if (padlock!=null && Input.GetMouseButtonDown (0) && hit.collider.gameObject.CompareTag ("Padlock")){
				BuyRoom();
			} else {
				RegisterHitDaytime (hit);
			}
		} else if (Input.GetMouseButtonDown (0)){
			Debug.Log ("registering click...");

			if (currentAbility==listAbilities[4] && listAbilities[4].CanUse () && hit.collider.gameObject.CompareTag ("Person")){
				currentAbility.UseAbility (hit);
				//currentAbility=null; 
			} else if (hit.collider.gameObject.CompareTag ("Furniture")){
	//			Debug.Log ("registering click...on furniture");
				Furniture f = hit.collider.gameObject.GetComponent<Furniture>();
				if (currentAbility==listAbilities[1] && listAbilities[1].CanUse()){
					//Debug.Log (hit.collider.name);
					currentAbility.UseAbility (hit);
				} else if (f is Lamp){
					if(lampSwitch != null)
						AudioSource.PlayClipAtPoint(lampSwitch, Camera.main.transform.position);
					Lamp l = hit.collider.gameObject.GetComponent<Lamp>();
					if (l.Durability>0)
						l.Flip ();
				} else if (f is Trap && !(f as Trap).Used) {
					Debug.Log("Activating");
					(f as Trap).Activate ();
				}
			} else if (currentAbility!=null && currentAbility!=listAbilities[1] && currentAbility!=listAbilities[4]) {
	//			Debug.Log ("registering click...with ability");
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
