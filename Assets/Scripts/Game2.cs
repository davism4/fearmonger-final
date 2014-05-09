using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game2 : MonoBehaviour {

	// don't edit these values:
	private float nightTimerRealSeconds =0f, GameMinutePerRealSecond;
	public float enemyGenCooldown=0f, roomCheckCountdown=0f;
	public const float enemyGenCooldownMax=20f, roomCheckCountdownMax=2f;
	public int currentRoomNumber=0, money=0, fearEnergy=0;
	public const int fearEnergyMax=100;
	private int  days=0;
	private const int nightDurationRealSecondsMax = 300; // real seconds per night round
	private const int nightDurationGameMinutes = 720; // 6pm to 6am =  12 hours * 60min/hr
	[HideInInspector] public Room[] rooms;
	private Room roomWithPriest, roomWithThug;
	private GameObject priestObject, thugObject;

	private List<Room> tempRoomList;
	private bool roomsHaveTraps=false, roomsHaveFurns=false, isChangingRooms=true;
	private float dyCam=99f;
	GameObject cam;
	private RaycastHit2D hit;
	private Ray ray;
	public Ability[] listAbilities;
	public Ability currentAbility;
	
	// VARIABLES FOR DAYTIME INPUT AND UI GO DOWN HERE

	// Values used in UI
	float percentNight;
	public Furniture[] furnitureTypes; // Viewable in daytime GUI menu - don't change
	public GameObject[] furniturePhysicalTypes; // use to instantiate from buy menu - don't change



	// =============================================== //
	// ================== INTERFACE ================== //
	// =============================================== //
	Texture2D fearBarTexture, fearBarTextureBlack, nightProgressTexture;
	Texture2D[] abilityIcons;
	float fearBarHeight=10f;

	[ExecuteInEditMode]
	private void OnGUI(){
		if (GameVars.IsNight){
			GUI.DrawTexture (new Rect(0,Screen.height-fearBarHeight,((float)Screen.width*fearEnergy)/fearEnergyMax,fearBarHeight),fearBarTexture,ScaleMode.StretchToFill);
			//Debug.Log (((float)fearEnergy)/fearEnergyMax);
			GUI.DrawTexture (new Rect(0,Screen.height-fearBarHeight,Screen.width,fearBarHeight),fearBarTextureBlack,ScaleMode.StretchToFill);

			for (int i=0;i<listAbilities.Length;i++){
				if(fearEnergy < listAbilities[i].minFearCost){ 
					GUI.contentColor=Color.gray;
				} else {
					if (listAbilities[i].isCooldown)
						GUI.contentColor=Color.yellow;
					else if (currentAbility==listAbilities[i])
						GUI.contentColor=Color.green;
					else
						GUI.contentColor=Color.white;
				}

				if (GUI.Button (new Rect (i*Screen.width/5, Screen.height-40-fearBarHeight, Screen.width/5, 40), listAbilities [i].ShowName())) {
					//cursorAppearance.SetSprite (2);
					if(fearEnergy >= listAbilities[i].minFearCost){
						currentAbility = listAbilities[i];
					}
				}
			}
		} else {
			// daytime GUI
		}
	}

	public void Buy(Furniture f){
		if (money >= f.buyCost){
			money -= f.buyCost;
			// get new furniture thing
			// place it somewhere
		} else {
			// Not enough money!
		}
	}
	
	private void RegisterHitDaytime(RaycastHit2D hit){
		if (Input.GetMouseButtonDown (0)){
		}
	}

	// Update is called once per frame
	private void Update () {
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
		GameVars.IsNight=true;
		tempRoomList = new List<Room>(); // this is used for checking in priests and thugs
		cam = Camera.main.transform.gameObject; // two distinct references?
		// Set up rooms
		rooms = new Room[GameObject.FindGameObjectsWithTag ("Room").Length];
		for (int i=1;i<=rooms.Length;i++){
			rooms[i-1] = GameObject.Find ("Room "+i).GetComponent<Room>();
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
		listAbilities = new Ability[5]; // these are attached to the Main Game transform
		listAbilities[0] = transform.GetComponent<Ability_Ghost>();
		listAbilities[1] = transform.GetComponent<Ability_Darkness>();
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
		GameVars.pickupCoin=Resources.Load<GameObject>("Prefabs/PickupCoin");
		GameVars.pickupFear=Resources.Load<GameObject>("Prefabs/PickupFear");
		int alayer = 1 << LayerMask.NameToLayer("PersonLayer");
		int blayer = 1 << LayerMask.NameToLayer("FurnitureLayer");
		GameVars.interactLayer = (alayer | blayer);
	}

	private void StartDay(){
		GameVars.IsNight=false;
		days++;
	}
	
	private void StartNight(){
		GameVars.IsNight=true;
		foreach (Room r in rooms){
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
		if (roomsHaveFurns || roomsHaveTraps)
			enemyGenCooldown=enemyGenCooldownMax;
	}
	
	private void RegisterHit(RaycastHit2D hit){
		Debug.Log (hit.collider.gameObject.name);
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
			if (currentRoomNumber<rooms.Length-1 && hit.collider.gameObject.CompareTag("Triangle Up")){
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
			if (currentAbility==listAbilities[4] && currentAbility.CanUse () && hit.collider.gameObject.CompareTag ("Person")){
				currentAbility.UseAbility (hit.point);
				currentAbility=null;
			} else if (hit.collider.gameObject.CompareTag ("Furniture")){
				//Debug.Log ("Switched lamp");
				if (hit.collider.gameObject.GetComponent<Furniture>() is Lamp){
					Lamp l = hit.collider.gameObject.GetComponent<Lamp>();
					l.Flip ();
				}
			} /*else if (false) { // select ability from its icon
					// select that as active ability
					// show info
				}*/
			else if (currentAbility!=null && currentAbility!=listAbilities[4]) {
				if (hit.collider.gameObject.CompareTag ("Room") || hit.collider.gameObject.CompareTag ("Furniture")){
					if (currentAbility.CanUse ()){
						currentAbility.UseAbility (hit.point);
						currentAbility=null;
					}
				}
			}
		}
	}



}
