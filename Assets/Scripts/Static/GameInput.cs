using UnityEngine;
using System.Collections;

public static class GameInput {



	// Removed. It works better in individual scripts.
	//public static Vector2 GetMouse2D() {
	//}
	
	public static bool IsInViewport(Vector3 v) {
		Camera cam = Camera.main;
		v = cam.WorldToScreenPoint(v);
		if (v.x < cam.pixelRect.xMin) return false;
		else if (v.x > cam.pixelRect.xMax) return false;
		else if (v.y < cam.pixelRect.yMin) return false;
		else if (v.y > cam.pixelRect.yMax) return false;
		else return true;
	}
	
	// Functions to normalize keyboard input
	public static bool Right()
	{
		return (Input.GetKey("right") && !Input.GetKey("left"));
	}
	
	public static bool RightOnce()
	{
		return (Input.GetKeyDown("right") && !Input.GetKey("left"));
	}
	
	public static bool Quit()
	{
		return (Input.GetKeyDown("escape"));
	}
	
	public static bool Left()
	{
		return (Input.GetKey("left") && !Input.GetKey("right"));
	}
	
	public static bool LeftOnce()
	{
		return (Input.GetKeyDown("left") && !Input.GetKey("right"));
	}
	
	public static bool Up()
	{
		return (Input.GetKey("up") && !Input.GetKey("down"));
	}
	
	public static bool UpOnce()
	{
		return (Input.GetKeyDown("up") && !Input.GetKey("down"));
	}
	
	public static bool Down()
	{
		return (Input.GetKey("down") && !Input.GetKey("up"));
	}
	
	public static bool DownOnce()
	{
		return (Input.GetKeyDown("down") && !Input.GetKey("up"));
	}
	
	public static bool Invisible()
	{
		return (Input.GetKeyDown("c"));
	}
	
	public static bool MapMenu()
	{
		return (Input.GetKeyDown("x") && !Input.GetKeyDown("z"));
	}
	
	public static bool StatMenu()
	{
		//return (Input.GetKeyDown("z"));
		return (Input.GetMouseButtonDown (1));
	}
}
