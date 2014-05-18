using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour{
	private AudioSource audioSource;
	private AudioClip sound;
	private AudioClip rooster;

	public void playBgmDay(){
		rooster = Resources.Load<AudioClip> ("Sounds/rooster");
		AudioSource.PlayClipAtPoint (rooster, Camera.main.transform.position);
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
