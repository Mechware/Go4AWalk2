using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Followers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongMenu : MonoBehaviour {

	public RuntimeSetSongData UnlockedSongs;
	public RuntimeSetSongData LockedSongs;
	public Button SongPrefab;
	public RectTransform Parent;
	public FluteController Flute;
	public FollowerSpawner Spawner;

	public PersistentSetSongData AllSongs;
	
	public void Start() {
		
		LockedSongs.AddRange(AllSongs);
		
		UnlockedSongs.OnAdd.AddListener((a) => {
			UpdateSongs();
		});

		UpdateSongs();
	}

	List<SongData> songsToRemove = new List<SongData>(); 

	public void Update() {
		
		foreach (var song in LockedSongs) {
			if (song.IsUnlocked) {
				UnlockedSongs.Add(song);
				songsToRemove.Add(song);
			}
		}
		
		if(songsToRemove.Count > 0) UpdateSongs();
		
		songsToRemove.ForEach(s => LockedSongs.Remove(s));
		songsToRemove.Clear();
	}
	
	public void UpdateSongs() {
		Parent.GetComponentsInChildren<RectTransform>().ForEach(f => {
			if(f != Parent.transform) Destroy(f.gameObject);
		});
		
		foreach (var song in UnlockedSongs) {
			var button = Instantiate(SongPrefab, Parent, false);
			
			button.onClick.AddListener(() => {
				Flute.StartSong(acc => {
					SongBuffController.Instance.OnSongFinish(song, acc);
				});
			});
			
			button.GetComponentInChildren<TextMeshProUGUI>().text = song.DisplayName;
		}
	}
}
