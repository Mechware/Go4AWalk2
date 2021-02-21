using System;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Utils;
using UnityEngine;

namespace Items {
    public class ConsumableManager : MonoBehaviour {
    
        [NonSerialized] public List<ConsumableSaveData> SaveData = new List<ConsumableSaveData>();
        [Obsolete("Singleton")] public static ConsumableManager Instance;


        private void Awake() {
            Instance = this;
        }

        public void OnLoad() {
            for (int index = 0; index < SaveData.Count; index++) {
                var consumeable = SaveData[index];
                if (RandomUtils.GetTime() < consumeable.EndTime) {
                    UseConsumable((Consumable) DataManager.Instance.AllItems.First(it => it.ID == consumeable.Id));
                }
            }
            ConsumableUi.Instance.Refresh();
        }

        public void Update() {
            for (int index = 0; index < SaveData.Count; index++) {
                var consumeable = SaveData[index];
                if (RandomUtils.GetTime() > consumeable.EndTime) {
                    SaveData.RemoveAt(index);
                    index--;
                    FinishConsumable((Consumable) DataManager.Instance.AllItems.First(it => it.ID == consumeable.Id));
                    ConsumableUi.Instance.Refresh();
                }
            }
        }

        public bool UseConsumable(Consumable c) {
            if (c.Type == Consumable.ConsumableType.Health) {
                UseHealthPotion(c);
                return false;
            }

            if (c.Type == Consumable.ConsumableType.Damage) {
                UseDamagePotion(c);
                return true;
            }

            if (c.Type == Consumable.ConsumableType.Speed) {
                UseSpeedPotion(c);
                return true;
            }
            
            if (c.Type == Consumable.ConsumableType.Bait) {
                BaitBuffController.Instance.StartBuff((Bait) c);
                return true;
            }

            return false;
        }

        public void FinishConsumable(Consumable c) {
            if (c.Type == Consumable.ConsumableType.Health) {
                Debug.LogError("Tried to finish a health conusmable, that's not how that works.");
                return;
            }

            if (c.Type == Consumable.ConsumableType.Damage) {
                FinishDamagePotion(c);
                return;
            }

            if (c.Type == Consumable.ConsumableType.Bait) {
                // Do nothing because bait controller handles this
            }

            if (c.Type == Consumable.ConsumableType.Speed) {
                FinishSpeed(c);
            }
        }
    
        public void UseHealthPotion(Consumable c) {
            var player = DataManager.Instance.Player;
            player.Health.Value = Mathf.Min(player.Health + (int)c.Affect, player.MaxHealth);
        }

        public void UseDamagePotion(Consumable c) {
            var player = DataManager.Instance.Player;
            player.DamageMultiplier *= c.Affect;
            SaveData.Add(new ConsumableSaveData {
                EndTime = RandomUtils.GetTime() + c.Duration,
                Id = c.ID
            });
        }

        public void FinishDamagePotion(Consumable c) {
            var player = DataManager.Instance.Player;
            player.DamageMultiplier /= c.Affect;
        }
        
        public void UseSpeedPotion(Consumable c) {
            DataManager.Instance.Player.SpeedMultiplier *= c.Affect;
            SaveData.Add(new ConsumableSaveData {
                EndTime = RandomUtils.GetTime() + c.Duration,
                Id = c.ID
            });
        }

        public void FinishSpeed(Consumable c) {
            DataManager.Instance.Player.SpeedMultiplier /= c.Affect;
        }
        
    }

    public class ConsumableSaveData {
        public int Id;
        public double EndTime;
    }
}