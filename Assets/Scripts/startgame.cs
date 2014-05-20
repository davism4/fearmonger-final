using UnityEngine;
using System.Collections;

public class startgame : MonoBehaviour {

	//private Texture2D[] texes;
	//private int texw, texh;
	//private int offsetx=-3, offsety=2;

	public Font typewriterFont;

	private void OnGUI(){
		GUI.skin.font=typewriterFont;
		GUI.skin.button.fontSize = 15;
		//spriteRenderer.sprite = cursorSprites[0];
		for (int i=0;i<2;i++){
			if (GUI.Button(new Rect(Screen.width/2-90f,Screen.height/2,180f,40f),"CLICK TO START...")){
				Application.LoadLevel (1);
			}
		}
		GUI.backgroundColor = Color.black;
		GUI.Label(new Rect(20,2f*Screen.height/3,Screen.width-40,Screen.height*2f/3f),
		          "CREDITS\n\n" +
		          "Michael Davis: Project Lead, Code\n" +
		          "Farah Khan: Lead Artist\n" +
		          "Mark Henkel: Animation, Programming\n" +
		          "Steve Hart: Animation, Programming\n" +
		          "Kevin Jang: Sound, Programming\n" +
		          "Jonathan Otterbein: Programming\n" +
		          "Wendy Mao: Music Composer");
		//GUI.DrawTexture (new Rect(Input.mousePosition.x-(texw/4)+offsetx,Screen.height-Input.mousePosition.y-(texh/4)+offsety,.6f*texw,.6f*texh),texes[0]);
	}

	//private void Start(){
	//	Screen.showCursor = true; 
	//	texes = Resources.LoadAll<Texture2D>("Sprites/Cursors");
		//spriteRenderer = transform.GetComponent<SpriteRenderer>();
	//	texw=texes[0].width;
	//	texh=texes[0].height;
	//}
}