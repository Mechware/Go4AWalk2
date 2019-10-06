using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Song")]
public class SongData : ScriptableObject {
	public float PixelsPerSecond;
	public List<NoteData> TopNotes;
	public List<NoteData> MiddleNotes;
	public List<NoteData> BottomNotes;

	public List<Drop> DropChances;
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
