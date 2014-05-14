using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {

	public AudioClip doorOpen;
	public AudioClip bgmNight;

	bool isBgmPlayed = false;

	// Use this for initialization
	void Start () {
		AudioSource.PlayClipAtPoint (doorOpen, Camera.main.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		if(!isBgmPlayed){
			isBgmPlayed = true;
			AudioSource.PlayClipAtPoint(bgmNight, Camera.main.transform.position);
		}
	}
}
