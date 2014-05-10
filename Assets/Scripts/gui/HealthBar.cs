using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	Person2 parentPerson;
	Furniture parentFurniture;
	float height, width, offset;
	bool parentIsPerson;
	Texture2D green, red;
	const float scale = 0.1f;

	void Start () {
		Debug.Log (transform.parent.name);
		if (transform.parent.CompareTag ("Person")){
			parentIsPerson=true;
			parentPerson = transform.parent.GetComponent<Person2>();
		} else if (transform.parent.CompareTag ("Furniture")){
			parentIsPerson=false;
			parentFurniture = transform.parent.GetComponent<Furniture>();
		}
		green = GameVars.hpBarGreen;
		red = GameVars.hpBarRed;
		width = green.width*scale;
		height = green.height*scale;
		offset = 6f*transform.parent.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
	}
	
	void OnGUI(){
		Vector3 v = Camera.main.WorldToScreenPoint(transform.parent.position);
		GUI.DrawTexture (new Rect(v.x-width/2,Screen.height-v.y-offset,width,height),red,ScaleMode.StretchToFill);
		if (parentIsPerson){
			GUI.DrawTexture (new Rect(v.x-width/2,Screen.height-v.y-offset,width*parentPerson.healthPercent,height),green,ScaleMode.StretchToFill);
		} else {
			GUI.DrawTexture (new Rect(v.x-width/2,Screen.height-v.y-offset,width*parentFurniture.healthPercent,height),green,ScaleMode.StretchToFill);
		}
	}
}
