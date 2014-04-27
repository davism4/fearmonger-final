using UnityEngine;
using System.Collections;

// This code manages what the cursor looks like. NOT INPUT.
// Clicking ingame for abilities is in AbilityManager
public class CursorAppearance : MonoBehaviour {

	//Transform targetTransform;
	//private Sprite[] cursorSprites;
	//SpriteRenderer spriteRenderer;
	private Game g;
	private RaycastHit2D hit;
//	private Ray ray;
//	private Vector2 mouse2d;
	// sound effect
	public AudioClip mouseClickSound;
	private Texture2D[] texes;
	private int texw, texh;
	private int offsetx=-3, offsety=2;

	// Use this for initialization
	private void Start () {
		//cursorSprites = Resources.LoadAll<Sprite>("Sprites/Cursors");
		texes = Resources.LoadAll<Texture2D>("Sprites/Cursors");
		//spriteRenderer = transform.GetComponent<SpriteRenderer>();
		texw=texes[0].width;
		texh=texes[0].height;
		g = GameObject.Find ("GameManager").GetComponent<Game>();
	}

	// An ability is selected and you click on some point in the room
	/*
	void ClickOnEmpty() {
		if (mouseClickSound)
			AudioSource.PlayClipAtPoint (mouseClickSound, transform.position); // play the mouseClickSound when clicked
	}

	// Clicked on a person
	void ClickOnPerson() {
		if (mouseClickSound)
			AudioSource.PlayClipAtPoint (mouseClickSound, transform.position); // play the mouseClickSound when clicked
	}


	// Based on what the mouse is hovering over, it changes appearance
	// we'll have preloaded sprites and then switch between them
	void UpdateAppearance(){
	}
	*/

	//public void SetSprite(int index){
	//	spriteRenderer.sprite = cursorSprites[index];
	//}




	private void OnGUI(){
		// draw the cursor on GUI level?
	//	ray = Camera.main.ScreenPointToRay(Input.mousePosition);    
		//mouse2d = (Vector2)(ray.origin + ray.direction);
	//	mouse2d = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		//transform.position = new Vector3(mouse2d.x, mouse2d.y, GameVars.DepthCursor);
		if (g.currentView==Game.View.Room && hit && hit.collider.gameObject.CompareTag("Door")){
			//spriteRenderer.sprite = cursorSprites[1];
			GUI.DrawTexture (new Rect(Input.mousePosition.x-(texw/4)+offsetx,Screen.height-Input.mousePosition.y-(texh/4)+offsety,.6f*texw,.6f*texh),texes[1]);
		}
		else if (g.currentView==Game.View.Room && hit && hit.collider.gameObject.CompareTag ("Lamp")){
			GUI.DrawTexture (new Rect(Input.mousePosition.x-(texw/4)+offsetx,Screen.height-Input.mousePosition.y-(texh/4)+offsety,.6f*texw,.6f*texh),texes[2]);
			//spriteRenderer.sprite = cursorSprites[2];
		}
		else {
			GUI.DrawTexture (new Rect(Input.mousePosition.x-(texw/4)+offsetx,Screen.height-Input.mousePosition.y-(texh/4)+offsety,.6f*texw,.6f*texh),texes[0]);
			//spriteRenderer.sprite = cursorSprites[0];
		}
		if (hit){
			if ((Input.GetMouseButtonDown(0) && hit.collider.gameObject.CompareTag ("Door")) ||
			    (Input.GetMouseButtonDown(0) && hit.collider.gameObject.CompareTag ("Lamp"))) {
				AudioSource.PlayClipAtPoint(mouseClickSound, transform.position);
			}
		}


	}
}
