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
		
		if (Input.GetKeyDown ("space")) {
			GameVars.IsNight = !GameVars.IsNight;
		}
	}
}
