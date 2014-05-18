using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour{
	private AudioSource audioSource;
	private AudioClip sound;

	public void playBgmDay(){
		sound = Resources.Load<AudioClip> ("Sounds/rooster");
		AudioSource.PlayClipAtPoint (sound, Camera.main.transform.position);
		audioSource = Camera.main.transform.Find ("Sound").GetComponent<AudioSource> ();
		sound = Resources.Load<AudioClip> ("Sounds/bgm_final_day");
		audioSource.Stop ();
		audioSource.loop = true;
		audioSource.volume = 1;
		audioSource.clip = sound;
		audioSource.PlayDelayed (1.5f);
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
