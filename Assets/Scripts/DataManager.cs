using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public PersistentSetFollowerData AllFollowers;
    public StatTracker StatTracker;
    public PersistentSetCraftingRecipe AllRecipes;
    public PersistentSetItem AllItems;
    public PersistentSetQuest AllQuests;

#if UNITY_EDITOR
    public void UpdateAll() {
        AllFollowers.AddAllOfType();
        AllRecipes.AddAllOfType();
        AllItems.AddAllOfType();
        AllQuests.AddAllOfType();

        while(AllFollowers.Contains(null))
            AllFollowers.Remove(null);
        while(AllRecipes.Contains(null))
            AllRecipes.Remove(null);
        while(AllItems.Contains(null))
            AllItems.Remove(null);
        while(AllQuests.Contains(null))
            AllQuests.Remove(null);

        foreach (var v in StatTracker.EnemyKillCount.Where(ekc => ekc.Enemy == null)) {
            StatTracker.EnemyKillCount.Remove(v);
        }
        foreach(var v in StatTracker.ItemObtainedCount.Where(ekc => ekc.Item == null)) {
            StatTracker.ItemObtainedCount.Remove(v);
        }
    }
#else
     public void UpdateAll() {}
#endif

    // Use this for initialization
    void Awake () {
		UpdateAll();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
