using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FluteController : MonoBehaviour {

	public ClickReceiver NotePrefab;

	public RectTransform TopParent;
	public RectTransform MiddleParent;
	public RectTransform BottomParent;
	
	public ClickReceiver MissArea;

	public SongData Song;

	public float StartX = 94f;

	public GameObject Blocker;
	public GameObject Area;

	public void StartSong(Action<float> onDone) {
		Blocker.SetActive(true);
		Area.SetActive(true);
		StartCoroutine(PlaySong(onDone));
	}
	
	IEnumerator PlaySong(Action<float> onDone) {
		float time = 0;

		
		int topIndex = 0;
		int middleIndex = 0;
		int bottomIndex = 0;
		
		List<ClickReceiver> ActiveNotes = new List<ClickReceiver>();
		List<ClickReceiver> NotesToDestroy = new List<ClickReceiver>();
		
		int Misses = 0;
		int Hits = 0;

		MissArea.MouseClick2D.RemoveAllListeners();
		MissArea.MouseClick2D.AddListener(pos => {
			SmoothPopUpManager.ShowPopUp(pos, "Miss!", Color.red, true);
			Misses++;
		});
		
		while (ActiveNotes.Count > 0 || 
		       topIndex < Song.TopNotes.Count ||
		       middleIndex < Song.MiddleNotes.Count ||
		       bottomIndex < Song.BottomNotes.Count()) {

			time += Time.deltaTime;
			if (topIndex < Song.TopNotes.Count && Song.TopNotes[topIndex].AppearTime < time) {
				//Spawn Top Note
				var note = Instantiate(NotePrefab, TopParent, false);
				
				RectTransform rt = (RectTransform)note.transform;
				rt.anchoredPosition = rt.anchoredPosition.SetX(StartX);
				
				note.MouseDown2D.AddListener(vec => {
					NotesToDestroy.Add(note);
					Hits++;
					SmoothPopUpManager.ShowPopUp(note.transform.position, "Hit!", Color.green, true);
				});
				ActiveNotes.Add(note);
				topIndex++;
			}

			if (middleIndex < Song.MiddleNotes.Count() && Song.MiddleNotes[middleIndex].AppearTime < time) {
				// Spawn middle note
				var note = Instantiate(NotePrefab, MiddleParent, false);
				
				RectTransform rt = (RectTransform)note.transform;
				rt.anchoredPosition = rt.anchoredPosition.SetX(StartX);
				
				note.MouseDown2D.AddListener(vec => {
					NotesToDestroy.Add(note);
					Hits++;
					SmoothPopUpManager.ShowPopUp(note.transform.position, "Hit!", Color.green, true);
				});
				ActiveNotes.Add(note);
				middleIndex++;
			}
			
			if (bottomIndex < Song.BottomNotes.Count && Song.BottomNotes[bottomIndex].AppearTime < time) {
				//Spawn bottom note
				var note = Instantiate(NotePrefab, BottomParent, false);
				
				RectTransform rt = (RectTransform)note.transform;
				rt.anchoredPosition = rt.anchoredPosition.SetX(StartX);
				
				note.MouseDown2D.AddListener(vec => {
					NotesToDestroy.Add(note);
					Hits++;
					SmoothPopUpManager.ShowPopUp(note.transform.position, "Hit!", Color.green, true);
				});
				ActiveNotes.Add(note);
				bottomIndex++;
			}

			float dist = Time.deltaTime * Song.PixelsPerSecond;
			
			foreach (var note in ActiveNotes) {
				
				RectTransform rt = (RectTransform) note.transform;
				rt.anchoredPosition = rt.anchoredPosition.AddX(-dist);
				
				if (rt.anchoredPosition.x < 0) {
					if (NotesToDestroy.Contains(note)) {
						// Click it on the frame it was going out
					}
					else {
						SmoothPopUpManager.ShowPopUp(note.transform.position, "Miss!", Color.red, true);
						Misses++;
					}
					NotesToDestroy.Add(note);
				}
			}

			foreach (var note in NotesToDestroy) {
				ActiveNotes.Remove(note);
				Destroy(note.gameObject);
			}
			
			NotesToDestroy.Clear();

			yield return null;
		}

		float acc;
		if (Misses == 0) acc = 1;
		else acc = (float)Hits / (Hits + Misses);
		Debug.Log($"Song done. Acc: {acc}");
		Blocker.SetActive(false);
		Area.SetActive(false);
		onDone?.Invoke(acc);
	}
}
