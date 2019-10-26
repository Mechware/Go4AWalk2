using System;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Utils;
using UnityEngine;

namespace Items {
    public class ConsumableManager : MonoBehaviour {
    
        [NonSerialized] public List<ConsumableSaveData> SaveData = new List<ConsumableSaveData>();

        public void OnLoad() {
            for (int index = 0; index < SaveData.Count; index++) {
                var consumeable = SaveData[index];
                if (RandomUtils.GetTime() < consumeable.EndTime) {
                    UseConsumable((Consumable) DataManager.Instance.AllItems.First(it => it.ID == consumeable.Id));
                }
            }
        }
        
        public void Update() {
            for (int index = 0; index < SaveData.Count; index++) {
                var consumeable = SaveData[index];
                if (RandomUtils.GetTime() > consumeable.EndTime) {
                    index--;
                    SaveData.RemoveAt(index);
                    FinishConsumable((Consumable) DataManager.Instance.AllItems.First(it => it.ID == consumeable.Id));
                }
            }
        }

        public bool UseConsumable(Consumable c) {
            if (c.Type == Consumable.ConsumableType.Health) {
                UseHealthPotion(c);
                return true;
            }

            if (c.Type == Consumable.ConsumableType.Damage) {
                UseDamagePotion(c);
                return true;
            }

            if (c.Type == Consumable.ConsumableType.Speed) {
                return false;
            }
            
            if (c.Type == Consumable.ConsumableType.Bait) {
                return false;
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
                //?
            }

            if (c.Type == Consumable.ConsumableType.Speed) {
                
            }
        }
    
        public void UseHealthPotion(Consumable c) {
            var player = DataManager.Instance.Player;
            player.Health.Value = Mathf.Min(player.Health + (int)c.Affect, player.MaxHealth);
        }

        public void UseDamagePotion(Consumable c) {
            var player = DataManager.Instance.Player;
            player.Weapon.Value.DamageMultiple *= c.Affect;
            SaveData.Add(new ConsumableSaveData {
                EndTime = RandomUtils.GetTime() + c.Duration,
                Id = c.ID
            });
        }

        public void FinishDamagePotion(Consumable c) {
            var player = DataManager.Instance.Player;
            player.Weapon.Value.DamageMultiple /= c.Affect;
        }
        
        public void FinishSpeed(Consumable c) { }
        
        public void FinishBait(Consumable c){ }
        
    }

    public class ConsumableSaveData {
        public int Id;
        public double EndTime;
    }
}