using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Questing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameEvents : MonoBehaviour {

	public Action OnStart;
	public Action OnAfterLoad;
	public Action OnPause;
	public Action OnQuit;
	public Action OnSceneExitEvent;
	public Action OnFocus;
	public Action OnUnFocus;
	public Action OnPlay;

	public Action OnNewGame;

	public Action<ActiveQuestBase> OnQuestSet;
	public Action<ActiveQuestBase> QuestChanged;
	public Action<EnemyData> EnemyKilled;
	public Action<Item> LootObtained;

	void Start () {
        OnStart?.Invoke();
		SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
	}

	private void SceneManager_sceneUnloaded( Scene arg0 ) {
		OnSceneExitEvent?.Invoke();
	}

	private void OnApplicationFocus( bool focus ) {
		if (focus) {
			OnFocus?.Invoke();
		}
		else {
			OnUnFocus?.Invoke();
		}
	}

	private void OnApplicationPause( bool pause ) {
		if (pause) {
			OnPause?.Invoke();
		}
		else {
			OnPlay?.Invoke();
		}
	}

	private void OnApplicationQuit() {
		OnQuit?.Invoke();
	}

	public void OnQuestChanged(ActiveQuestBase quest)
	{
		QuestChanged?.Invoke(quest);
	}

	public void OnEnemyKilled(EnemyData ed)
	{
		EnemyKilled?.Invoke(ed);
	}

	public void OnLootObtained(IEnumerable<Item> its)
	{
		foreach (var it in its) OnLootObtained(it);
	}

	public void OnLootObtained(Item it)
	{
		LootObtained?.Invoke(it);
	}

	public void OnLootObtained(Item it, int amount)
	{
		for (int i = 0; i < amount; i++) LootObtained?.Invoke(it);
	}
}
