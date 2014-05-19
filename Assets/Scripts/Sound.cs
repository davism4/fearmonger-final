using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour{
	private AudioSource audioSource;
	private AudioClip sound;

	public void playBgmDay(){
		Debug.Log ("Playing day background music");
		sound = Resources.Load<AudioClip> ("Sounds/rooster");
		AudioSource.PlayClipAtPoint (sound, Camera.main.transform.position);
		audioSource = Camera.main.transform.GetComponent<AudioSource> ();
		sound = Resources.Load<AudioClip> ("Sounds/bgm_final_day");
		audioSource.Stop ();
		audioSource.loop = true;
		audioSource.volume = 0.5f;
		audioSource.clip = sound;
		audioSource.PlayDelayed (1.5f);
	}

	public void playBgmNight(){
		Debug.Log ("Playing night background music");
		audioSource = Camera.main.transform.GetComponent<AudioSource> ();
		sound = Resources.Load<AudioClip> ("Sounds/bgm_final_night");
		audioSource.Stop ();
		audioSource.loop = true;
		audioSource.volume = 0.75f;
		audioSource.clip = sound;
		audioSource.Play ();
	}
}
