using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {
	public AudioClip bgmDay = Resources.Load<AudioClip>("Sounds/bgm_final_day");

	public void playBgmDay(){
		if(bgmDay != null)
			AudioSource.PlayClipAtPoint (bgmDay, Camera.main.transform.position);
	}
}
