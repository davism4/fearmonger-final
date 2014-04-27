using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	// GLOBAL VARIABLES GO HERE:
	public enum View { Start, Room, Map, Menu}
	public View currentView;
	public int NumOccupiedRooms=0;
	
	// ABILITY AND INPUT
	public Ability[] listAbilities;
	public Ability currentAbility;
	private Vector2 clickLocation2D;
	private RaycastHit2D hit;
	private Ray2D ray;
	//private int selectedIndex=10;
	
	// INTERFACE LOGIC
	private CameraObject cameraObject;
//	private CursorAppearance cursorAppearance;
	private Transform cameraMapPositionTransform;
	private Rect textRect;
	public GUIStyle messageTextStyle;
	private GUIStyle abilityTextStyle;
	public Font typewriterFont;
	private float textTimer=0, textTimerMax=4f;
	private bool isWritingText=false;
	
	// we also need to keep track of this
	[HideInInspector] public PlayerLevel playerLevel;
	
	// ROOMS
	public RoomObject currentRoom;
//	private RoomObject lastRoom;
	private int floorsUnlocked=1;
	private Transform[,] squares;
	private RoomObject[,] rooms;
	private int[,] PeoplePerRoom;
	private int row=0, col=0;
	private float upBound, leftBound, rightBound, downBound;

	private int currentRow;
	private int currentColumn;
	
	// By convention, timers start at zero and increment to max
	// this counts the time, in seconds, between check-ins.
	// the timer max will change randomly
	private float checkInTimer=0, checkInTimerMax=5f;
	
	public void Pause(){
		Time.timeScale=0f;
		GameVars.IsPaused=true;
	}
	public void Unpause(){
		Time.timeScale=1f;
		GameVars.IsPaused=false;
	}
	
	public void unlockFloor()
	{
		if (floorsUnlocked < 4){
			rooms[floorsUnlocked,0].Unlock(10f);
			rooms[floorsUnlocked,1].Unlock(10f);
			rooms[floorsUnlocked,2].Unlock(10f);
			rooms[floorsUnlocked,3].Unlock(10f);
			floorsUnlocked++;
		}
	}

	private string textstring="";
	private int textstringLength=0;
	public void WriteText(string str){
		textstring=str;
		textstringLength=textstring.Length;
		textTimer=0f;
		isWritingText=true;
	}

	private Rect WriteBox(){
		int height =20;
		//int yOffset = 50;
		return new Rect((Screen.width-textstringLength)/2,0,textstringLength,height);
	}

	private void Start ()
	{
		listAbilities = new Ability[5];
		listAbilities[0] = new Ability_Spiders(); 
		listAbilities[1] = new Ability_Darkness();
		listAbilities[2] = new Ability_Claw();
		listAbilities[3] = new Ability_Monster();
		listAbilities[4] = new Ability_Possess();
	//	selectedIndex = 10;
//		cursorAppearance = GameObject.Find("Cursor").GetComponent<CursorAppearance>();
		cameraObject = GameObject.FindGameObjectWithTag("MainCamera").transform.GetComponent<CameraObject>();
		playerLevel = transform.GetComponent<PlayerLevel>();
		if (playerLevel==null){
			throw(new MissingReferenceException());
		}
		squares = new Transform[4,4];
		rooms = new RoomObject[4,4];
		PeoplePerRoom = new int[4,4];
		for (int i=0; i<4; i++) {
			for (int j=0; j<4; j++) { // Get all of the rooms
				squares[i,j] = GameObject.Find("Room "+(i+1)+"0"+(j+1)).transform;
				rooms[i,j] = (RoomObject)squares[i,j].GetComponent<RoomObject>();
			}
		}
		currentRoom = rooms[0,0]; 		// Initialize room 101?
		currentRow = 0;
		currentColumn = 0;
		rooms[0,0].Unlock(2f);
		rooms[0,1].Unlock(8f);
		rooms[0,2].Unlock(14f);
		rooms[0,3].Unlock(20f);
		currentView = View.Map;
		messageTextStyle.contentOffset = new Vector2(0,(Screen.height-2*messageTextStyle.fontSize));
		GoToMap ();
		//print (Screen.height);
	}
	public bool isAtMap{
		get { return (bool)(currentView==View.Map);}}

	
	public void GoToRoom(RoomObject room)
	{
		//if (room.isUnlocked) {
			//Unpause();
			//if (currentView==View.Map) {
				//Debug.Log("Go to room "+room.RoomName);
				cameraObject.ZoomIn(room);
				currentView = View.Room;
			//}
		//}
		//else {
			//WriteText (room.RoomName+" is locked.");
			//A "room is locked!" message is displayed
		//}
	}
	
	public void GoToRoom(int row, int column){
		currentRoom = rooms[row,column];
		GoToRoom(currentRoom);

		currentRow = row;
		currentColumn = column;
	}
	
	public void GoToMap() {
		Debug.Log("Going to map");
		//Pause ();
		if (currentView==View.Room) {
	//		lastRoom = currentRoom;
			cameraObject = GameObject.FindGameObjectWithTag("MainCamera").transform.GetComponent<CameraObject>();
			currentView = View.Map;
			cameraObject.ZoomOut();
		}
	}
	
	
	private void SelectAbility(int index){
		Debug.Log("Selected ability: "+listAbilities[index].Name);
		currentAbility = listAbilities[index];
		// other stuff
	}
	
	
	private void OnGUI(){
		//int orginalFontSize = GUI.skin.button.fontSize;
		GUI.skin.font=typewriterFont;

		// main HUD text
		if (isWritingText)
			GUI.Label (WriteBox (), textstring, messageTextStyle);

		// GAME MODE = LIST OF ABILITIES
		if (currentView == View.Room){
			GUI.skin.button.fontSize = 15;
			for (int i=0;i<listAbilities.Length;i++){
				if(listAbilities[i].Locked && listAbilities[i].BuyCost <= playerLevel.BuyPoints){
					GUI.contentColor=Color.gray;
				} else if (listAbilities[i].Locked && listAbilities[i].BuyCost > playerLevel.BuyPoints){
					GUI.contentColor=Color.red;
				} else if (currentAbility==listAbilities[i]){
					GUI.contentColor=Color.green;
				} else {
					GUI.contentColor=Color.white;
				}
				if (GUI.Button (new Rect (10, 80+50*i, 145, 40), listAbilities [i].Name)) {
					//cursorAppearance.SetSprite (2);
					if(listAbilities[i].Locked && listAbilities[i].BuyCost <= playerLevel.BuyPoints)
					{
						playerLevel.BuyAbility(listAbilities[i]);
						SelectAbility(i);
						WriteText("New ability: "+listAbilities[i].Name+".");
					}
					else if(listAbilities[i].Locked && listAbilities[i].BuyCost > playerLevel.BuyPoints)
					{
						GUI.color=Color.red;
						WriteText("You need "+listAbilities[i].BuyCost+" points to buy "+listAbilities[i].Name+".");
					}
					else{
						SelectAbility(i);
						WriteText("Current ability: "+listAbilities[i].Name+".");
					}
				}

			}

			/*for (int i=0; i<listAbilities.Length ; i++) {
				if (!listAbilities[i].Locked) {
					GUI.color = Color.cyan;
					GUI.Box(new Rect(115, (61 + (i * 30)), 20, 30),"E");
				}
				else if (listAbilities[i].Locked) {
					GUI.color = Color.red;
					GUI.Box(new Rect(115, (61 + (i * 30)), 20, 30), listAbilities[i].BuyCost.ToString());
				}
			}*/
		}

		//GUI.skin.button.fontSize = orginalFontSize;
		// MAP = ROOM SELECTION
		else if (currentView == View.Map){
			GUI.skin.button.fontSize = 18;
			float w= Screen.width*0.085f;
			float h = Screen.height*0.12f;
			for (int x=0;x<4;x++){
				for (int y=0; y<4; y++){
					if (rooms[y,x].isUnlocked){
						GUI.color=Color.white;
					} else {
						GUI.color=Color.grey;
					}
					/*if (GUI.Button (new Rect(Screen.width*(.273f+0.12f*x),
					                         Screen.height*(.7f-0.1645f*y),
					                         Screen.width*0.085f,
					                         Screen.height*0.12f),
					                rooms[y,x].MapName()))*/

					if (GUI.Button (new Rect(Camera.main.WorldToScreenPoint(rooms[y,x].transform.position).x-w/2,// Screen.width*(.273f+0.12f*x),
					                         Camera.main.WorldToScreenPoint(rooms[3-y,x].transform.position).y-h/4,// Screen.height*(.7f-0.1645f*y),
					                         w,
					                         h),
					                rooms[y,x].MapName()))
					{GoToRoom (y,x);}
				}
			}
		}
	}

	
	private void Update() {
		//cursorAppearance.SetSprite (0);
		if (isWritingText){
			if (textTimer<textTimerMax){
				textTimer+=GameVars.Tick*Time.deltaTime;
			} else {
				textTimer=0f;
				isWritingText=false;
			}
		}

		// UPDATE THE ROOMS AND PEOPLE
		for (int i=0; i<4; i++) for (int j=0; j<4; j++) {
			PeoplePerRoom[i,j] = rooms[i,j].numberOccupants;
		}
		if (!currentRoom) {
			currentRoom = rooms[row,col];

			currentRow = row;
			currentColumn = col;
		}
		// CHECKING PEOPLE IN AND OUT
		if (NumOccupiedRooms<floorsUnlocked*4) {
			if (checkInTimer < checkInTimerMax) {
				checkInTimer += GameVars.Tick*Time.deltaTime;
			}
			else {
				// check into a new vacant room, if able room
				for (int i=0; i<floorsUnlocked; i++){
					for (int j=0; j<4; j++){
						if (!rooms[i,j].isOccupied)
						{
							rooms[i,j].CheckIn();
							//WriteText("People checked into "+rooms[i,j].RoomName+".");
							i=5; j=5;
							checkInTimer =0;
							checkInTimerMax = Random.Range(20f,40f);
						}
					}
				}
			}
		}
		// REGISTER INPUT
		hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		Ray ray3d = Camera.main.ScreenPointToRay(Input.mousePosition);    
		clickLocation2D = (Vector2)(ray3d.origin + ray3d.direction);
		// Only show the HP bar when mouse over
		if (hit && hit.collider.gameObject.CompareTag("Person")){
			Person p = hit.collider.gameObject.GetComponent<Person>();
			if (p!=null)
				p.ShowHPBar ();
		}
		// Register click
		if (Input.GetMouseButtonDown (0)) {
			if ((currentView == View.Room) && hit) {

				if(hit.collider.gameObject.CompareTag("TriangleUp") && (currentRow < 3)){
					Debug.Log("Move UP to next room");
					GoToRoom(currentRow+1, currentColumn);
				}
				if(hit.collider.gameObject.CompareTag("TriangleDown") && (currentRow > 0)){
					Debug.Log("Move Dpwn a Room");
					GoToRoom(currentRow-1, currentColumn);
				}

				if (hit.collider.gameObject.CompareTag("Door")){
					// clicked on the door -> go to the map
					//cursorAppearance.SetSprite (1);
					GoToMap ();
				}
				else if (hit.collider.gameObject.CompareTag ("Lamp")){
					LampObject l = hit.collider.gameObject.GetComponent<LampObject>();
					l.Switch ();
				}
				else if (currentAbility!=null){
					if (playerLevel.EnergyCurrent>=currentAbility.EnergyCost){
						if( !hit.collider.gameObject.CompareTag("Solid") && !hit.collider.gameObject.CompareTag("Background"))
							currentAbility.UseAbility(this, clickLocation2D);
					}
					else {
						WriteText("Not enough energy!");
						//Debug.Log ("Not enough energy!"); // play a "fail" sound
					}
				}

				//else {
				//	WriteText("No ability selected.");
					//Debug.Log ("No ability selected."); // play a "fail" sound
				//}
			}
		}
	}
}