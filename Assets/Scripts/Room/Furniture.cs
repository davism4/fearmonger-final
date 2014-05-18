using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour {

	/** Placeable = permanent dropped into the grid layout. People interact with them.
	 * width, height = # blocks for object 2D dimensions
	 * admireValue = max of how much $ people drop when admiring
	 * buyCost = how much $ the player needs for a new piece
	 */
	//public int admireValue=0;
	//private bool isShowingDurability=false;
	private float showHPCooldown, damageCooldown;
	private const float showHPCooldownMax=0.2f, damageCooldownMax=0.6f;
	public string description="";
	public int buyCost=0;
	private int durability, durabilityMax=99;
	public int Durability {get {return durability; }}
	public string DisplayName="";	
	[HideInInspector] public Node node = null;
	public Room room {
		get {return this.node.room;}
	}
	public Vector3 offset = Vector3.zero;


	public float healthPercent {
		get { return ((float)durability)/durabilityMax; }
	}

	public bool Damage(int delta){
		if (damageCooldown<=0){
			durability -= delta;
			damageCooldown = damageCooldownMax;
			return (bool)(durability <=0);
		} else {
			return false;
		}
	}

	public void Repair(int delta){
		durability += delta;
		if (durability > durabilityMax)
			durability=durabilityMax;
	}


	// Called during daytime furniture placement
	public void Delete(){
		
	}

	public void DisplayHP(){
		showHPCooldown=showHPCooldownMax;
	}

	protected virtual void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			Person2 p = other.transform.GetComponent<Person2>();
			Damage(1);
			p.Interact (this);
		}
	}

	protected virtual void Start(){
		durability = durabilityMax;
		LoadHPBar ();
		transform.position += offset;
	}

	public void SetNode(Node n){
		this.node = n;
	//	Debug.Log(name+"'s node is "+node.name+" , room is "+room.name);
	}

	protected virtual void Update(){
		if(showHPCooldown>0f){
			showHPCooldown -= Time.deltaTime;
		}
		if (damageCooldown>0f){
			damageCooldown -= Time.deltaTime;
		}
//		Debug.Log (node.name);
	}

	public void Break(){
		// animation
		Destroy (this.gameObject);
	}
	public void Sell(){
		Debug.Log ("Sold "+name);
		Destroy (this.gameObject);
	}

	private void LoadHPBar(){
		spriteWidth = GameVars.hpBarRed.width*0.1f;
		spriteHeight = GameVars.hpBarRed.height*0.1f;
		hpBarOffsetY = 0f;//GetComponent<SpriteRenderer>().sprite.bounds.size.y;
	}

	private float hpBarOffsetY, spriteWidth, spriteHeight;
	private void OnGUI(){
		if (Time.timeScale<=0) return;
		if (showHPCooldown>0f){
			Vector3 v = Camera.main.WorldToScreenPoint(transform.position);
			GUI.DrawTexture (new Rect(v.x-spriteWidth/2,Screen.height-v.y-hpBarOffsetY,spriteWidth,spriteHeight),GameVars.hpBarRed,ScaleMode.StretchToFill);
			GUI.DrawTexture (new Rect(v.x-spriteWidth/2,Screen.height-v.y-hpBarOffsetY,spriteWidth*healthPercent,spriteHeight),GameVars.hpBarGreen,ScaleMode.StretchToFill);
		}
	}

}
