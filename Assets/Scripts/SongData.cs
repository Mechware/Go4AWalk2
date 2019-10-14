using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Questing;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Song")]
public class SongData : ScriptableObject {
	public string DisplayName;
	
	public float PixelsPerSecond;
	public List<NoteData> TopNotes;
	public List<NoteData> MiddleNotes;
	public List<NoteData> BottomNotes;

	public List<Drop> DropChances;

	public ActiveQuestBase QuestToUnlock;

	public bool IsUnlocked => QuestToUnlock == null || StatTracker.Instance.CompletedQuests.Any(q => q.ID == QuestToUnlock.ID);
}


[Serializable]
public class NoteData {
	public float AppearTime;
}

[Serializable]
public class Drop {
	public FollowerData Data;
	public float MinAccuracy;
	public float Chance;
}
