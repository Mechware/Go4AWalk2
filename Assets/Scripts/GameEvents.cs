using G4AW2.Data;
using G4AW2.Data.Crafting;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace G4AW2
{
	public class GameEvents : MonoBehaviour
	{

		public Action OnStart;
		public Action OnAfterLoad;
		public Action OnPause;
		public Action OnQuit;
		public Action OnSceneExitEvent;
		public Action OnFocus;
		public Action OnUnFocus;
		public Action OnPlay;

		public Action<QuestInstance> OnQuestSet;
		public Action<EnemyInstance> EnemyKilled;
		public Action<ItemInstance> LootObtained;
		public Action<Achievement> AchievementObtained;
		public Action<CraftingRecipe> CraftingRecipeUnlocked;
		public Action<Area> AreaChanged;

		void Start()
		{
			OnStart?.Invoke();
			SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
		}

		private void SceneManager_sceneUnloaded(Scene arg0)
		{
			OnSceneExitEvent?.Invoke();
		}

		private void OnApplicationFocus(bool focus)
		{
			if (focus)
			{
				OnFocus?.Invoke();
			}
			else
			{
				OnUnFocus?.Invoke();
			}
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause)
			{
				OnPause?.Invoke();
			}
			else
			{
				OnPlay?.Invoke();
			}
		}

		private void OnApplicationQuit()
		{
			OnQuit?.Invoke();
		}

		public void OnQuestChanged(QuestInstance quest)
		{
			OnQuestSet?.Invoke(quest);
		}

		public void OnEnemyKilled(EnemyInstance ed)
		{
			EnemyKilled?.Invoke(ed);
		}

		public void OnAchievementObtained(Achievement a) => AchievementObtained?.Invoke(a);

		public void OnLootObtained(IEnumerable<ItemInstance> its)
		{
			foreach (var it in its) OnLootObtained(it);
		}

		public void OnLootObtained(ItemInstance it)
		{
			LootObtained?.Invoke(it);
		}

		public void OnLootObtained(ItemInstance it, int amount)
		{
			for (int i = 0; i < amount; i++) LootObtained?.Invoke(it);
		}

		public void OnRecipeUnlocked(CraftingRecipe recipe)
		{
			CraftingRecipeUnlocked?.Invoke(recipe);
		}

		public void OnAreaChanged(Area area)
		{
			AreaChanged?.Invoke(area);
		}
	}

}
