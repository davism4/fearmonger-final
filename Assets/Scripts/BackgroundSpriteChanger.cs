using UnityEngine;
using System.Collections;

public class BackgroundSpriteChanger : MonoBehaviour {

	public Sprite nightBackground;
	public Sprite dayBackground;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameVars.IsNight == true) {
			this.GetComponent<SpriteRenderer> ().sprite = nightBackground;
		} else {
			this.GetComponent<SpriteRenderer> ().sprite = dayBackground;
		}
		transform.position = new Vector3(0, 10+Camera.main.transform.position.y*0.9f,40);
		//if (Input.GetKeyDown ("space")) {
		//	GameVars.IsNight = !GameVars.IsNight;
		//}
	}
}
