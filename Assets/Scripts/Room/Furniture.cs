using UnityEngine;
using System.Collections;

public class Furniture : MonoBehaviour {
	
	private float showHPCooldown, damageCooldown;
	private const float showHPCooldownMax=0.2f, damageCooldownMax=0.6f;
	public string description="";
	public int buyCost=0;
	private int durability;
	public int durabilityMax; // set in inspector
	public int Durability {get {return durability; }}
	public string DisplayName { 
		get { return name; }
	}
	public int SellValue {
		get { return (durability*buyCost)/durabilityMax; }
	}

	[HideInInspector] public Node node = null;
	public Room room {
		get {return this.node.room;}
	}
	public Vector3 offset = Vector3.zero;
	public bool IsTrap=false;


	public float healthPercent {
		get { return ((float)durability)/durabilityMax; }
	}

	public bool Damage(int delta){
		if (damageCooldown<=0){
			durability -= delta;
			damageCooldown = damageCooldownMax;
			if (durability<=0)
				Break();
			return (bool)(durability <=0);
		} else {
			return false;
		}
	}

	public virtual void Repair(int delta){
		durability += delta;
		if (durability > durabilityMax)
			durability=durabilityMax;
	}


	// Called during daytime furniture placement
	//public void Delete(){
		
//	}

	public void DisplayHP(){
		showHPCooldown=showHPCooldownMax;
	}

	protected virtual void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Person")){
			Person2 p = other.transform.GetComponent<Person2>();
			if (UnityEngine.Random.Range (0,10)>5)
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
	}

	protected virtual void Update(){
		if(showHPCooldown>0f){
			showHPCooldown -= Time.deltaTime;
		}
		if (damageCooldown>0f){

			damageCooldown -= Time.deltaTime;

			if (damageCooldown >0f)
				GetComponent<SpriteRenderer>().color = Color.red;
			else
				GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

	public void Break(){
		node.Clear();
		Destroy (this.gameObject);
	}
	public void Sell(){
		node.Clear();
		Destroy (this.gameObject);
	}

	private void LoadHPBar(){
		spriteWidth = GameVars.hpBarRed.width*0.1f;
		spriteHeight = GameVars.hpBarRed.height*0.15f;
	}

	private float spriteWidth, spriteHeight;
	private void OnGUI(){
		if (Time.timeScale<=0) return;
		if (showHPCooldown>0f){
			Vector3 v = Camera.main.WorldToScreenPoint(transform.position);//-hpBarOffset);
			GUI.DrawTexture (new Rect(v.x-spriteWidth/2,Screen.height-v.y,spriteWidth,spriteHeight),GameVars.hpBarRed,ScaleMode.StretchToFill);
			GUI.DrawTexture (new Rect(v.x-spriteWidth/2,Screen.height-v.y,spriteWidth*healthPercent,spriteHeight),GameVars.hpBarGreen,ScaleMode.StretchToFill);
		}
	}

}
