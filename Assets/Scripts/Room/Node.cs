using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	//BoxCollider2D box;
	public Room room;
	public GameObject content;
	public bool empty {
		get {return (content == null);}
	}
	public Furniture furniture {
		get {return content.GetComponent<Furniture>();}
	}
	private SpriteRenderer srenderer;

	public void DisplayBorder(bool on){
		if (empty){
			srenderer.enabled=on;
		//	GetComponent<BoxCollider2D>().enabled=on;
		}
	}

	private void Update(){
		srenderer.enabled = (bool)(!GameVars.IsNight && GameVars.IsPlacingFurniture &&
		                          content==null);
	}

	void Start () {
	//	box = GetComponent<BoxCollider2D>();
		room = transform.parent.GetComponent<Room>();
		srenderer = GetComponent<SpriteRenderer>();
	}

	public void Add(GameObject o){
		if (empty){
			GameObject instance = Instantiate(o,this.transform.position,Quaternion.identity) as GameObject;
			this.content = instance;
			room.AddItem(this.content);
			this.content.GetComponent<Furniture>().SetNode(this);
			srenderer.enabled=false;
			GameVars.IsPlacingFurniture=false;
			//GetComponent<BoxCollider2D>().enabled=false;
		}
	}

	public void BoxDisable(){
		GetComponent<BoxCollider2D>().enabled=false;
	}

	public void BoxEnable(){
		GetComponent<BoxCollider2D>().enabled=true;
	}

	public void Clear(){
		room.RemoveItem (this.content);
		this.content = null;
		srenderer.enabled=true;
	}
}
