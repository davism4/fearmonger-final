using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// GLOBAL VARIABLES
public static class GameVars {

	public static float WallLeft = -100f; // these get set by MainGame
	public static float WallRight = 100f; // these get set by MainGame
	public static float WallLeftSoft = -99f; // these get set by MainGame
	public static float WallRightSoft = 99f; // these get set by MainGame

	public static int interactLayer; // set by Game2
	public static int userInteractLayer;

	public static bool IsPlacingFurniture = false;

	public static bool IsPausedTutorial = false;

	public static bool IsPaused=false;
	public static bool IsNight=false;

	/*public static float duration_spiders=1f;
	public static int damage_spiders=1;
	public static float duration_ghost=2f;
	public static int damage_ghost=1;
	public static float duration_darkness=2f;
	public static int damage_darkness=2;
	public static float duration_claw=8f;
	public static int damage_claw=3;
	public static float duration_monster=18f;
	public static int damage_monster=4;
	public static float duration_possession_short=6f;
	public static float duration_possession_long=12f;
	public static int damage_possession=5;*/

	public static GameObject pickupFear = Resources.Load<GameObject>("Prefabs/PickupFear");
	public static GameObject pickupCoin = Resources.Load<GameObject>("Prefabs/PickupCoin");
	public static Texture2D hpBarRed = Resources.Load<Texture2D>("Sprites/gui/hpBarRed");
	public static Texture2D hpBarGreen = Resources.Load<Texture2D>("Sprites/gui/hpBarGreen");

	//public static float Difficulty = 7.5f;
	// higher numbers = easier (lower exp drain)
	// prevent multiple scripts from registering the same input simultaneously
	//public static bool JustClicked=false;
	//public static bool JustPressedKey=false;
	//public const float Tick = 1f;
}
