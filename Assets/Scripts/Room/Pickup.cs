using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public float countdown, timeMax=5f;

	public void Collect(){
		DestroyObject(gameObject);
		//DestroyImmediate(this);
	}

	// Use this for initialization
	void Start () {
		countdown=timeMax;
	}
	
	// Update is called once per frame
	void Update () {
		// they fade away at daytime
		if (!GameVars.IsNight){// || timer>timerMax){
			DestroyObject(gameObject);
			//DestroyImmediate (this);
		} else if (countdown<0){
			DestroyObject(gameObject);
			//DestroyImmediate(this);
		} else {
			countdown -= Time.deltaTime;
		}
	}
}
