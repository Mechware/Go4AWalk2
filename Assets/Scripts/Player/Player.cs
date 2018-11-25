using CustomEvents;
using G4AW2.Combat.Swiping;
using UnityEngine;
using G4AW2.Data.DropSystem;
using G4AW2.Data.Inventory; 

namespace G4AW2.Combat {

	[CreateAssetMenu(menuName = "Data/Player")]
	public class Player : ScriptableObject {

		public IntReference MaxHealth;

		public IntReference Health;
		public FloatReference Power;
        public FloatReference Armor;
		public IntReference Damage;

		public FloatReference PowerPerBlock;
		public GameEvent OnPowerMax;

        public InventoryList inventory;

        public Item hat, armor, weapon, boots, accessory;

		public void OnEnable() {
            if (weapon != null) Damage.Value = weapon.value;
		}

		public void OnDisable() {
		}

		public int GetLightDamage() {
			return Damage;
		}

		public void Hit( int damage ) {
			Health.Value -= Mathf.RoundToInt(damage*Armor/100);
		}

		public void Block( Swipe s ) {
			Power.Value = Mathf.Min(Power.Value + PowerPerBlock, 100f);
			if (Power.Value == 100f) {
				OnPowerMax.Raise();
			}
		}
        public void setItem(Item item)
        {
            //removeItem(item.type);
            switch (item.type)
            {
                case (ItemType.Weapon):
                    Damage.Value = item.value;
                    weapon = item;
                    break;
                case (ItemType.Torso):
                    Armor.Value = Armor.Value+item.value;
                    armor = item;
                    break;
                case (ItemType.Hat):
                    Armor.Value = Armor.Value+item.value;
                    hat = item;
                    break;
                case (ItemType.Boots):
                    Armor.Value = Armor.Value+item.value;
                    boots = item;
                    break;
            }
        }
        public void removeItem(ItemType type)
        {
            switch (type)
            {
                case (ItemType.Weapon):
                    if (weapon!=null)
                    {
                        Damage.Value = 1;
                        inventory.addItem(weapon);
                        weapon = null;
                    }
                    break;
                case (ItemType.Torso):
                    if (armor != null)
                    {
                        Armor.Value = Armor.Value-armor.value;
                        inventory.addItem(armor);
                        armor = null;
                    }
                    break;
                case (ItemType.Hat):
                    if (hat != null)
                    {
                        Armor.Value = Armor.Value-hat.value;
                        inventory.addItem(hat);
                        hat = null;
                    }
                    break;
                case (ItemType.Boots):
                    if (boots != null)
                    {
                        Armor.Value = Armor.Value-boots.value;
                        inventory.addItem(boots);
                        boots = null;
                    }
                    break;
            }
        }

        public Item returnItem(ItemType type)
        {            
            switch (type)
            {
                case (ItemType.Weapon):
                    return weapon;
                case (ItemType.Torso):
                    return armor;
                case (ItemType.Hat):
                    return hat;
                case (ItemType.Boots):
                    return boots;
                    
            }
            return null;
        }


#if UNITY_EDITOR
		[ContextMenu("Restore Health")]
		private void ResetHealth() {
			Health.Value = MaxHealth;
		}
#endif
	}

}
