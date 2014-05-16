using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour{
	public AudioSource audioSource;
	public AudioClip sound;

	public void playBgmDay(){
			audioSource = Camera.main.transform.Find ("Sound").GetComponent<AudioSource> ();
			sound = Resources.Load<AudioClip> ("Sounds/bgm_final_day");
			audioSource.Stop ();
			audioSource.loop = true;
			audioSource.volume = 1;
			audioSource.clip = sound;
			audioSource.Play ();
	}

	public void playBgmNight(){
		audioSource = Camera.main.transform.Find ("Sound").GetComponent<AudioSource> ();
		sound = Resources.Load<AudioClip> ("Sounds/bgm_final_night");
		audioSource.Stop ();
		audioSource.loop = true;
		audioSource.volume = 1;
		audioSource.clip = sound;
		audioSource.Play ();
	}
}
