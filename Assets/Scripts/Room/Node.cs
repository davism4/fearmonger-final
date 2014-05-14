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

	public void DisplayGrid(bool on){
		if (empty){
			GetComponent<SpriteRenderer>().enabled=on;
		//	GetComponent<BoxCollider2D>().enabled=on;
		}
	}

	void Start () {
	//	box = GetComponent<BoxCollider2D>();
		room = transform.parent.GetComponent<Room>();
	}

	public void Add(GameObject o){
		if (empty){
			GameObject instance = Instantiate(o,this.transform.position,Quaternion.identity) as GameObject;
			this.content = instance;
			room.AddItem(this.content);
			this.content.GetComponent<Furniture>().SetNode(this);
			GetComponent<SpriteRenderer>().enabled=false;
			//GetComponent<BoxCollider2D>().enabled=false;
		}
	}

	public void Clear(){
		room.RemoveItem (this.content);
		this.content = null;
		GetComponent<SpriteRenderer>().enabled=true;
	}
}
