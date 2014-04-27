using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// SOMEHOW THIS FILE WILL DETERMINE HOW TO GENERATE
// THE COMBINATIONS OF PEOPLE WHO CAN CHECK INTO A ROOM

public static class PersonLists {
	/*====== PEOPLE GENERATION LISTS ======
	 * As you level up, there are more possible combinations of visitors
	 * More difficult combinations become possible at higher levels.
	 * Index:
	 * 0 = boy, 1 = girl
	 * 2 = man, 3 = woman
	 * 4 = man w/ candle, 5 = woman w/ candle
	 * 6 = priest
	 */
	// Starting list, level 1: 1 child, 1 adult, or 2 adults -> RoomObjects access this
	public static List<int[]> combo1 = new List<int[]>(){
		new int[]{2,2}, new int[]{0,2}, new int[]{0,3}, new int[]{2,3},
		new int[]{1,2}, new int[]{1,3}, new int[]{3,3},
	};

	// PlayerLevel accesses this
	public static List<int[]> Combinations = combo1;

	public static void GetNewCombos(int level){
		Combinations = combo1;
		//if (level >= 1){
			AddLists (combo2);
			AddLists (combo3);
		//}
		if (level >= 2){
			AddLists(combo4);
		} if (level >= 3){
			AddLists(combo5);
			AddLists(combo6);
			AddLists(combo8);
		} if (level >= 4){
			AddLists(combo13);
			AddLists (combo14);
			AddLists (combo7);
		} if (level >= 5){
			AddLists (combo9);
			AddLists (combo17);
			AddLists(combo10);
		} if (level >= 6){
			AddLists (combo11);
			AddLists (combo12);
		} if (level >= 7){
			AddLists(combo15);
			AddLists(combo18);
		} if (level >= 8){
			AddLists(combo19);
			AddLists (combo16);
		} if (level >= 9){
			AddLists (combo20);
			AddLists (combo21);
		} if (level >= 10){
			AddLists (combo22);
		}
	}
	static void AddLists(List<int[]> l){
		foreach (int[] i in l){
			Combinations.Add (i);
		}
	}


	// Next list: 2 children, 1 adult
	static List<int[]> combo2 = new List<int[]>(){
		new int[]{0,0,2}, new int[]{0,0,3}, new int[]{0,1,2}, new int[]{0,1,3},
		new int[]{1,1,2}, new int[]{1,1,3},
	};
	// Next list: 1 child, 2 adults
	static List<int[]> combo3 = new List<int[]>(){
		new int[]{0,2,2}, new int[]{0,2,3}, new int[]{0,3,3}, 
		new int[]{1,2,2}, new int[]{1,2,3}, new int[]{1,3,3},
	};
	// Next list: 3 adults
	static List<int[]> combo4 = new List<int[]>(){
		new int[]{2,2,2}, new int[]{2,2,3}, new int[]{2,3,3}, new int[]{3,3,3},
	};
	// Next list: 1 child, 1 candle
	static List<int[]> combo5 = new List<int[]>(){
		new int[]{0,4}, new int[]{0,5}, new int[]{1,4}, new int[]{1,5},
	};
	// Next list: 2 children, 1 candle
	static List<int[]> combo6 = new List<int[]>(){
		new int[]{0,0,4}, new int[]{0,0,5}, new int[]{0,1,4}, new int[]{0,1,5},
		new int[]{1,1,4}, new int[]{1,1,5}, 
	};
	// Next list: 1 child, 1 adult, 1 candle adult
	static List<int[]> combo7 = new List<int[]>(){
		new int[]{0,2,4}, new int[]{0,2,5}, new int[]{0,3,4}, new int[]{0,3,5},
		new int[]{1,2,4}, new int[]{1,2,5}, new int[]{1,3,4}, new int[]{1,3,5},
	};
	// Next list: 1 children, 2 candle adults
	static List<int[]> combo8 = new List<int[]>{
		new int[]{0,4,4}, new int[]{0,4,5}, new int[]{0,5,5}, new int[]{1,4,4},
		new int[]{1,4,5}, new int[]{1,5,5},
	};
	// Then you get rooms with 2 adults, 1 candle adults
	static List<int[]> combo9 = new List<int[]>(){
		new int[]{2,2,4}, new int[]{2,2,5}, new int[]{2,3,4}, new int[]{2,3,5},
		new int[]{3,3,4}, new int[]{3,3,5}, 
	};
	static List<int[]> combo10 = new List<int[]>(){
		new int[]{2,4}, new int[]{2,5}, new int[]{3,4}, new int[]{3,5},
	};
	static List<int[]> combo11 = new List<int[]>(){
		new int[]{4,4}, new int[]{4,5}, new int[]{5,5},
	};
	static List<int[]> combo12 = new List<int[]>(){
		new int[]{2,4,4}, new int[]{2,4,5}, new int[]{2,5,5}, new int[]{3,4,4},
		new int[]{3,4,5}, new int[]{3,5,5},
	};
	static List<int[]> combo13 = new List<int[]>(){
		new int[]{0,6}, new int[]{1,6},
	};
	// Then you can get 2 children, 1 priest
	static List<int[]> combo14 = new List<int[]>(){
		new int[]{0,0,6}, new int[]{0,1,6}, new int[]{1,1,6},
	};
	//  1 adult, 1 priest
	static List<int[]> combo15 = new List<int[]>(){
		new int[]{2,6}, new int[]{3,6},
	};
	// 1 child, 1 adult, 1 priest
	static List<int[]> combo16 = new List<int[]>(){
		new int[]{0,2,6}, new int[]{0,3,6}, new int[]{1,2,6}, new int[]{1,3,6},
	};
	// 3 candle people
	static List<int[]> combo17 = new List<int[]>(){
		new int[]{4,4,4}, new int[]{4,4,5}, new int[]{4,5,5}, new int[]{5,5,5},
	};
	// 2 adults, 1 priest
	static List<int[]> combo18 = new List<int[]>(){
		new int[]{2,2,6}, new int[]{2,3,6}, new int[]{3,3,6},
	};
	// 1 candle person, 1 priest
	static List<int[]> combo19 = new List<int[]>(){
		new int[]{4,6}, new int[]{5,6},
	};
	// 1 child, 1 candle person, 1 priest
	static List<int[]> combo20 = new List<int[]>(){
		new int[]{0,4,6}, new int[]{0,5,6}, new int[]{1,4,6}, new int[]{1,5,6},
	};
	// 1 adult, 1 candle person, 1 priest
	static List<int[]> combo21 = new List<int[]>(){
		new int[]{2,4,6}, new int[]{2,5,6}, new int[]{3,4,6}, new int[]{3,5,6},
	};
	// 2 candle adults, 1 priest
	static List<int[]> combo22 = new List<int[]>(){
		new int[]{4,4,6}, new int[]{4,5,6}, new int[]{5,5,6},
	};
	// Let's say no more than 1 priest per room, so combo22 is the most difficult

}
