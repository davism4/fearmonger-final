using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game2 : MonoBehaviour {

	// Stuff we can leave open in the editor
	public int  money=0, fearEnergy=0;
	public bool START_AT_NIGHT=false;
	public float BASE_CAM_SPEED = 25f;

	// don't edit these values:
	private const float fearDecayCooldownMax=9f;
	private float nightTimerRealSeconds =0f, GameMinutePerRealSecond;
	public float enemyGenCooldown=0f, roomCheckCooldown=0f, fearDecayCooldown=0f;
	private const float enemyGenCooldownMax=20f, roomCheckCooldownMax=5f;
	private const int fearEnergyMax=100;
	private int days=1, currentRoomNumber=0;
	public int Day{get { return days; }}
	public int CurrentRoomNumber { get { return currentRoomNumber; }}
	public int RoomsOpen=0;
	
	private const int nightDurationRealSecondsMax = 180; // real seconds per night round
	public int GAME_MINUTES {
		get { return nightDurationRealSecondsMax/60; }
	}
	private const int nightDurationGameMinutes = 720; // 6pm to 6am =  12 hours * 60min/hr
	public Room[] rooms;
	private Room roomWithPriest, roomWithThug;
	private GameObject priestObject, thugObject;
	private HotelRoof hotelRoof;
	private List<Room> tempRoomList = new List<Room>(); // this is used for checking in priests and thugs
	private bool roomsHaveTraps=false, roomsHaveFurns=false, isChangingRooms=true;
	private Vector3 dvCam=Vector3.zero;//private float dyCam=99f;
	private GameObject cam;
	private RaycastHit2D hit;
	private Ray ray;
	private Ability[] listAbilities;
	private Ability currentAbility;
	static string tooltip;
	public GUIStyle nostyle;
	private Font mytype;
	public int NextRoomCost {
		get {return rooms[RoomsOpen].Cost;}
	}

	//private int count; // what was this supposed to do?

	private Sound sound; // which sound is this referring to? It is referenced in Start() 
	private AudioClip collectSound;
	private AudioClip collectFear;
	private AudioClip clickSound;
	private AudioClip lampSwitch;

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
//	private Texture2D[] abilityIcons;
	private int bottomGuiHeight, abilityButtonHeight, furnitureButtonHeight,
	abilityButtonWidth, furnitureButtonWidth;
	
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
		bottomGuiHeight = Screen.height/6;
		GUI.DrawTexture (new Rect(0,Screen.height-bottomGuiHeight,
		                          Screen.width,bottomGuiHeight),fearBarTextureBlack,ScaleMode.StretchToFill);
		
		if (Time.timeScale<=0) return;
		abilityButtonHeight = bottomGuiHeight/2;
		abilityButtonWidth = Screen.width/listAbilities.Length;
		furnitureButtonHeight = bottomGuiHeight/2;
		furnitureButtonWidth = Screen.width/(furnitureTypes.Length/2);

		GUI.skin.font = mytype;
		nostyle.fontSize = bottomGuiHeight/2;
		string str;
		if (GameVars.IsNight){
			// Fear percentage bar
			GUI.DrawTexture (new Rect(0,Screen.height-bottomGuiHeight,
			                          Screen.width*(((fearEnergy-1)*fearDecayCooldownMax+fearDecayCooldown)/(fearDecayCooldownMax*fearEnergyMax)),
			                          bottomGuiHeight),
			                 fearBarTexture,ScaleMode.ScaleAndCrop);
			nostyle.fontSize = abilityButtonHeight/2;
			GUI.contentColor=Color.white;
			GUI.Label(new Rect(0,Screen.height - abilityButtonHeight,Screen.width,abilityButtonHeight),
			          "F e a r:  "+fearEnergy+" %",nostyle);
			for (int i=0;i<listAbilities.Length;i++){
				str = listAbilities[i].ShowName();
				GUI.skin.button.fontSize = Mathf.Min(2*abilityButtonHeight/5, abilityButtonWidth/(str.Length-3));
				if(fearEnergy < listAbilities[i].MinFear){ 
					GUI.contentColor=Color.gray;
					GUI.backgroundColor=Color.gray;
				} else {
					GUI.backgroundColor=Color.magenta;
					if (listAbilities[i].isCooldown)
						GUI.contentColor=Color.yellow;
					else if (currentAbility==listAbilities[i]){
						GUI.contentColor=Color.green;
						GUI.backgroundColor=Color.green;
					} else
						GUI.contentColor=Color.white;
				}
				if (GUI.Button (new Rect (i*abilityButtonWidth, Screen.height-bottomGuiHeight,
				                          abilityButtonWidth, abilityButtonHeight),str/* new GUIContent(str,
				                          listAbilities[i].ShowName() + ": " + listAbilities[i].Description
				                          + " Costs " + listAbilities[i].MinFear + " Fear")*/)) {
					if(fearEnergy >= listAbilities[i].MinFear){
						currentAbility = listAbilities[i];
					}
				}
			}
		} else {
			// daytime GUI
			int index=0;
			GUI.skin.button.fontSize = Mathf.Max (furnitureButtonWidth,furnitureButtonHeight)/10;

			GUI.DrawTexture (new Rect(0,Screen.height-2*furnitureButtonHeight,Screen.width,2.01f*furnitureButtonHeight),fearBarTextureBlack,ScaleMode.StretchToFill);

			for (int row=2; row>=1; row--) {
				for (int x=0;x<furnitureTypes.Length/2;x++){
					GUI.backgroundColor = Color.magenta;
					GUI.contentColor = Color.white;
					if (currentFurnitureIndex == index){
						GUI.contentColor=Color.green;
						GUI.backgroundColor=Color.green;
					} else if (money < furnitureTypes[index].buyCost){
						GUI.contentColor=Color.gray;
						GUI.backgroundColor= Color.gray;
					}
					else {
						GUI.backgroundColor = Color.magenta;
						GUI.contentColor = Color.white;
					}
					str = furnitureTypes[index].DisplayName + ": $" +furnitureTypes[index].buyCost;
					GUI.skin.button.fontSize = Mathf.Min(2*furnitureButtonHeight/5, furnitureButtonWidth/(str.Length-3));
					if (GUI.Button (new Rect(1+x*Screen.width/(furnitureTypes.Length/2),
					                         Screen.height-row*furnitureButtonHeight,
					                         Screen.width/(furnitureTypes.Length/2) - furnitureTypes.Length/2,
					                         furnitureButtonHeight),
					                str)){
					                /*new GUIContent(furnitureTypes[index].DisplayName + ": $" +furnitureTypes[index].buyCost, 
					                          furnitureTypes[index].DisplayName + ": " + furnitureTypes[index].description)*/
						if (money >= furnitureTypes[index].buyCost) {
			//				Debug.Log("Placing furniture: "+furnitureTypes[index].name);
							currentFurnitureIndex = index;
							GameVars.IsPlacingFurniture=true;
						}
					}
					/*GUI.contentColor = Color.white;
					GUI.Label (new Rect(x*Screen.width/(furnitureTypes.Length/2),
					                    630,
					                    Screen.width/(furnitureTypes.Length/2),
					                    60), GUI.tooltip);
					GUI.tooltip = null;*/
					index++;
				}
			}
		}
		GUI.skin.font = null;
	}

	public void BuyRoom(){
	//	Debug.Log ("buy room");
		if (money>= rooms[RoomsOpen].Cost){
			if (RoomsOpen < rooms.Length) {

				money -= rooms[RoomsOpen].Cost;
				rooms[RoomsOpen].Buy ();
				rooms[RoomsOpen-1].open = true;
				hotelRoof.MoveToFloor(rooms[RoomsOpen]);
				RoomsOpen++;
			}
		} else {
			Debug.Log ("You need "+rooms[RoomsOpen].Cost+" to open a new floor.");
		}
	}

	private bool Buy(Furniture f, RaycastHit2D hit){
		if (money >= f.buyCost){
			money -= f.buyCost;
			return true;
		} else {
			return false;
			// Not enough money!
		}
	}
	private void Sell(GameObject obFurn){
		money += obFurn.GetComponent<Furniture>().SellValue;
		obFurn.GetComponent<Furniture>().Sell();
	}
	

	public void CheckEmptyHotel(){
		if (GameVars.IsNight){
		//	Debug.Log("Checking empty hotel...");
			foreach (Room r in rooms){
				Debug.Log("Checking room "+r.name+" has "+r.occupants.Count);
				if (r.occupants.Count>0)
					return;
			}
			Debug.Log ("Nobody is left in the hotel!");
			StartDay ();
		}
	}

	// Update is called once per frame
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit ();
		}
		if (Day==1){
			if (START_AT_NIGHT){
				START_AT_NIGHT = false; // only run once
				StartNight ();
			} else {

				//StartDay ();
			}
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
			if (fearEnergy>0){
				if (fearDecayCooldown <= 0){
					fearEnergy--;
					fearDecayCooldown = fearDecayCooldownMax;
				}
				else
					fearDecayCooldown -= Time.deltaTime;
			}
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
			if (currentFurnitureIndex<0)
				GameVars.IsPlacingFurniture=false;
			// DAYTIME GAME LOGIC
		}
		// CLICKING ON STUFF
		if (!GameVars.IsPausedTutorial){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			hit = Physics2D.Raycast(ray.origin,ray.direction);
			if (hit){
				RegisterHit(hit);
			}
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

	public void CheckOutThug(){
		roomWithThug=null;
		Debug.Log ("Checked out the thug");
		enemyGenCooldown=enemyGenCooldownMax;
	}
	public void CheckOutPriest(){
		roomWithPriest=null;
		Debug.Log ("Checked out the priest");
		enemyGenCooldown=enemyGenCooldownMax;
	}

	// Find a room with at least 1 trap, and put the priest in it
	void CheckInPriest(){
		tempRoomList.Clear ();
		foreach (Room r in rooms){
			if (r==roomWithThug)
				continue;
			if (r.TrapCount()>0){
				tempRoomList.Add (r);
			}
		}
		if (tempRoomList.Count>0){
			int roomIndex=UnityEngine.Random.Range (0,tempRoomList.Count-1);
		//	Debug.Log("Checking priest into room "+roomIndex);
			roomWithPriest = tempRoomList[roomIndex];
			roomWithPriest.AddEnemy(priestObject);
			enemyGenCooldown = enemyGenCooldownMax;
//			Debug.Log("WARNING - PRIEST HAS ENTERED INTO "+roomWithPriest.name);
		}
	}

	// Find a room with at least 1 furniture, and put the thug in it
	void CheckInThug(){
		tempRoomList.Clear ();
		foreach (Room r in rooms){
			if (r==roomWithPriest)
				continue;
			if (r.NonTrapFurnitureCount()>0){
				tempRoomList.Add (r);
			}
		}
		if (tempRoomList.Count>0){
			int roomIndex=UnityEngine.Random.Range (0,tempRoomList.Count-1);
			roomWithThug = tempRoomList[roomIndex];
			roomWithThug.AddEnemy(thugObject);
			enemyGenCooldown = enemyGenCooldownMax;
	//		Debug.Log("WARNING - THUG HAS ENTERED INTO "+roomWithThug.name);
		}
	}

	// =============================================== //
	// ================= GAME SETUP ================== //
	// =============================================== //
	
	private void Start() {
		hotelRoof = GameObject.Find ("HotelRoof").GetComponent<HotelRoof>();
		cam = Camera.main.transform.gameObject; // two distinct references?
		mytype = Resources.Load<Font>("Fonts/my_type_of_font/mytype");

		for (int i=0;i<rooms.Length;i++){
			rooms[i].Cost = 500*(1+i);
			rooms[i].Start();
			if (i<RoomsOpen){
				rooms[i].Buy ();
			} else {
				rooms[i].transform.position += new Vector3(100,0,0);
			}
		}
		hotelRoof.Initialize(rooms);
		hotelRoof.MoveToFloor(rooms[RoomsOpen-1]);

		GameMinutePerRealSecond = ((float)nightDurationGameMinutes/nightDurationRealSecondsMax);
		GameVars.WallLeft=transform.FindChild("Marker LeftWall").transform.position.x;//rooms[0].XLeft;
		GameVars.WallRight=transform.FindChild("Marker RightWall").transform.position.x;//rooms[0].XRight;
		GameVars.WallLeftSoft=transform.FindChild("Marker LeftSoftWall").transform.position.x;//=GameVars.WallLeft+2f;
		GameVars.WallRightSoft=transform.FindChild("Marker RightSoftWall").transform.position.x;//=GameVars.WallRight-2f;

		fearBarTexture = Resources.Load<Texture2D>("Sprites/gui/fearprogress-purple");
		fearBarTextureBlack = Resources.Load<Texture2D>("Sprites/gui/fearprogress-black");

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
		int alayer = 1 << LayerMask.NameToLayer("PersonLayer");
		int blayer = 1 << LayerMask.NameToLayer("FurnitureLayer");
		GameVars.interactLayer = (alayer | blayer);
		sound = this.GetComponent<Sound> ();
		collectSound = Resources.Load<AudioClip> ("Sounds/collect");
		collectFear = Resources.Load<AudioClip> ("Sounds/collect_fear");
		clickSound = Resources.Load<AudioClip> ("Sounds/click");
		lampSwitch = Resources.Load<AudioClip> ("Sounds/lamp_switch");

		if (START_AT_NIGHT){
			sound.playBgmNight();
		} else {
			sound.playBgmDay();
		}
	}

	private IEnumerator wait(float time){
		yield return new WaitForSeconds(time);
	}

	public void StartDay(){
		GetComponent<Light>().enabled=true;
		money += 10*fearEnergy;
		fearEnergy=0;
		Debug.Log ("Starting day...");
		if (sound != null) {
			wait(0.2f);
			sound.playBgmDay ();
		}
		else {
			Debug.LogWarning ("Day background music missing");
		}
		GameVars.IsNight=false;
		days++;
		foreach (Room r in rooms){
			r.CheckOut ();
			r.DisplayGrid (true);
		}
	}
	
	public void StartNight(){
		bool okay=true;
		foreach (Room r in rooms){
			if (!r.Empty){
				okay=true;
				break;
			}
		}
		if(okay){
			GameVars.IsNight=true;
			GetComponent<Light>().enabled=false;
			fearDecayCooldown = fearDecayCooldownMax;
			roomCheckCooldown = roomCheckCooldownMax;
			nightTimerRealSeconds = 0;

//			Debug.Log ("Starting night..");
			if (sound != null)
				sound.playBgmNight ();
			else
				Debug.LogWarning ("Night background music missing");
			
			foreach (Room r in rooms){
				r.DisplayGrid (false);
	//			Debug.Log (name + " is open: "+r.open);
				if (r.open){
//					Debug.Log (r.name+" is open.");
					if (r.CheckIn()){
		//				Debug.Log (r.name+" check in.");
						if (!roomsHaveTraps)
							roomsHaveTraps = (bool)(r.TrapCount()>0);
						if (!roomsHaveFurns)
							roomsHaveFurns = (bool)(r.NonTrapFurnitureCount()>0);
					}
				}
			}
			// No non-trap furniture = no thug tonight
			// No traps = no priest tonight
	//		Debug.Log ("Checking for traps..."+roomsHaveTraps);
	//		Debug.Log ("Checking for furniture..."+roomsHaveFurns);
			if (roomsHaveFurns || roomsHaveTraps)
				enemyGenCooldown=enemyGenCooldownMax;
		} else {
			Debug.Log("Hotel is empty!");
		}
	}

	public void MoveUp(){
		if(clickSound != null)
			AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		currentRoomNumber++;
		isChangingRooms=true;
	}

	public void MoveDown(){
		if(clickSound != null)
			AudioSource.PlayClipAtPoint(clickSound, Camera.main.transform.position);
		currentRoomNumber--;
		isChangingRooms=true;
	}

	private void RegisterHit(RaycastHit2D hit){
		if (hit.collider.gameObject.CompareTag ("Money")){
			DestroyObject (hit.collider.gameObject);
			if(collectSound != null)
				AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position);
			money += 25;
		} else if (hit.collider.gameObject.CompareTag("FearPickup")){
			DestroyObject (hit.collider.gameObject);
			if(collectFear != null)
				AudioSource.PlayClipAtPoint (collectFear, Camera.main.transform.position);
			if (fearEnergy<fearEnergyMax){
				fearEnergy += 2;
				fearEnergy = Mathf.Min (fearEnergy,fearEnergyMax);
				fearDecayCooldown=fearDecayCooldownMax;
			}
			else
				money+=50;
		} else if (hit.collider.gameObject.CompareTag ("Person")){
			Person2 p = hit.collider.gameObject.GetComponent<Person2>();
			p.DisplayHP ();
		} else if (hit.collider.gameObject.CompareTag ("Furniture")){
			Furniture f = hit.collider.gameObject.GetComponent<Furniture>();
			f.DisplayHP();
		}
		if (Input.GetMouseButtonDown (0)){
			if (currentRoomNumber<RoomsOpen && hit.collider.gameObject.CompareTag("Triangle Up")){
				MoveUp ();
			} else if (currentRoomNumber > 0 && hit.collider.gameObject.CompareTag("Triangle Down")){
				MoveDown ();
			}
		}
		if (!GameVars.IsNight) {
				RegisterHitDaytime (hit);
		} else if (Input.GetMouseButtonDown (0)){
			if (currentAbility!=null && currentAbility!=listAbilities[1] && currentAbility!=listAbilities[4]) {

				if (hit.collider.gameObject.CompareTag ("Room") || hit.collider.gameObject.CompareTag ("Node") ||
				    hit.collider.gameObject.CompareTag ("Furniture") || hit.collider.gameObject.CompareTag ("Person")){
					if (currentAbility.CanUse ()){
						currentAbility.UseAbility (hit);
						currentAbility=null;
					}
				}
			} else if (hit.collider.gameObject.CompareTag ("Hazard")){
				hit.collider.GetComponent<Hazard>().Fade();
			}
			else if (currentAbility==listAbilities[4] && listAbilities[4].CanUse () && hit.collider.gameObject.CompareTag ("Person")){
				currentAbility.UseAbility (hit);
				currentAbility=null;  // possession
			} else if (hit.collider.gameObject.CompareTag ("Furniture")){
				Furniture f = hit.collider.gameObject.GetComponent<Furniture>();
				if (currentAbility==listAbilities[1] && listAbilities[1].CanUse()){
					currentAbility.UseAbility (hit);
					currentAbility=null;
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
			}
		}

	}
}
