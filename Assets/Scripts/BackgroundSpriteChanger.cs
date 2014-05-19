using UnityEngine;
using System.Collections;

public class BackgroundSpriteChanger : MonoBehaviour {

	public Sprite nightBackground;
	public Sprite dayBackground;
	public bool stationary=false;
	public float yOffset = 10f;

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
		if (!stationary)
			transform.position = new Vector3(0, yOffset+Camera.main.transform.position.y*0.9f,40);
		//if (Input.GetKeyDown ("space")) {
		//	GameVars.IsNight = !GameVars.IsNight;
		//}
	}
}
