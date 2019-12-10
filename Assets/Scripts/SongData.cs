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
using Random = System.Random;

[CreateAssetMenu(menuName = "Data/Song")]
public class SongData : ScriptableObject {
	public string DisplayName;
	
	public float PixelsPerSecond;
	public List<NoteData> TopNotes;
	public List<NoteData> MiddleNotes;
	public List<NoteData> BottomNotes;


	public ActiveQuestBase QuestToUnlock;

	public bool IsUnlocked => QuestToUnlock == null || SaveGame.SaveData.CompletedQuests.Any(id => id == QuestToUnlock.ID);

	[Header("Buff")]
	public float BuffDuration;
	public List<Drop> DropChances;
	public float MinDropTime;
	public float MaxDropTime;


	public float GetSpawnTime(float acc) {
		
		return MinDropTime + (1f - acc) * (MaxDropTime - MinDropTime) +
				UnityEngine.Random.Range(-0.5f, 0.5f) * (MaxDropTime - MinDropTime);
	}
	
}


[Serializable]
public class NoteData {
	public float AppearTime;
}