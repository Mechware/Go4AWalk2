using System;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Combat;
using G4AW2.Utils;
using UnityEngine;

namespace Items {
    public class ConsumableManager : MonoBehaviour {
    
        public static ConsumableManager Instance;


        private void Awake() {
            Instance = this;
        }

        public void OnLoad() {
            for (int index = 0; index < SaveGame.SaveData.Consumables.Count; index++) {
                var consumeable = SaveGame.SaveData.Consumables[index];
                if (RandomUtils.GetTime() < consumeable.EndTime) {
                    UseConsumable((ConsumableConfig) Configs.Instance.Items.First(it => it.Id == consumeable.Id));
                }
            }
            ConsumableUi.Instance.Refresh();
        }

        public void Update() {
            for (int index = 0; index < SaveGame.SaveData.Consumables.Count; index++) {
                var consumeable = SaveGame.SaveData.Consumables[index];
                if (RandomUtils.GetTime() > consumeable.EndTime) {
                    SaveGame.SaveData.Consumables.RemoveAt(index);
                    index--;
                    FinishConsumable((ConsumableConfig) Configs.Instance.Items.First(it => it.Id == consumeable.Id));
                    ConsumableUi.Instance.Refresh();
                }
            }
        }

        public bool UseConsumable(ConsumableConfig c) {
            if (c.Type == ConsumableConfig.ConsumableType.Health) {
                UseHealthPotion(c);
                return false;
            }

            if (c.Type == ConsumableConfig.ConsumableType.Damage) {
                UseDamagePotion(c);
                return true;
            }

            if (c.Type == ConsumableConfig.ConsumableType.Speed) {
                UseSpeedPotion(c);
                return true;
            }
            
            if (c.Type == ConsumableConfig.ConsumableType.Bait) {
                BaitBuffController.Instance.StartBuff((Bait) c);
                return true;
            }

            return false;
        }

        public void FinishConsumable(ConsumableConfig c) {
            if (c.Type == ConsumableConfig.ConsumableType.Health) {
                Debug.LogError("Tried to finish a health conusmable, that's not how that works.");
                return;
            }

            if (c.Type == ConsumableConfig.ConsumableType.Damage) {
                FinishDamagePotion(c);
                return;
            }

            if (c.Type == ConsumableConfig.ConsumableType.Bait) {
                // Do nothing because bait controller handles this
            }

            if (c.Type == ConsumableConfig.ConsumableType.Speed) {
                FinishSpeed(c);
            }
        }
    
        public void UseHealthPotion(ConsumableConfig c) {
            var player = Player.Instance;
            player.Health = Mathf.Min(player.Health + (int)c.Affect, player.MaxHealth);
        }

        public void UseDamagePotion(ConsumableConfig c) {
            var player = Player.Instance;
            player.DamageMultiplier *= c.Affect;
            SaveGame.SaveData.Consumables.Add(new ConsumableSaveData {
                EndTime = RandomUtils.GetTime() + c.Duration,
                Id = c.Id
            });
        }

        public void FinishDamagePotion(ConsumableConfig c) {
            var player = Player.Instance;
            player.DamageMultiplier /= c.Affect;
        }
        
        public void UseSpeedPotion(ConsumableConfig c) {
            Player.Instance.SpeedMultiplier *= c.Affect;
            SaveGame.SaveData.Consumables.Add(new ConsumableSaveData {
                EndTime = RandomUtils.GetTime() + c.Duration,
                Id = c.Id
            });
        }

        public void FinishSpeed(ConsumableConfig c) {
            Player.Instance.SpeedMultiplier /= c.Affect;
        }
        
    }


}