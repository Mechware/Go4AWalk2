using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Followers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongMenu : MonoBehaviour {

	public RuntimeSetSongData Songs;
	public Button SongPrefab;
	public RectTransform Parent;
	public FluteController Flute;
	public FollowerSpawner Spawner;

	public PersistentSetSongData AllSongs;
	
	public void Start() {
		// TODO: Figure out a better way to get songs
		Songs.AddRange(AllSongs);
		
		Songs.OnAdd.AddListener((a) => {
			UpdateSongs();
		});

		UpdateSongs();
	}

	public void UpdateSongs() {
		foreach (var song in Songs) {
			var button = Instantiate(SongPrefab, Parent, false);
			button.onClick.AddListener(() => {
				
				
				Flute.StartSong(acc => {
					Spawner.SpawnMonster(song, acc);
				});
			});
			button.GetComponentInChildren<TextMeshProUGUI>().text = song.name;
		}
	}
}
