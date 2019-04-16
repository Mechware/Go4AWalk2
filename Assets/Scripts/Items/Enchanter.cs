using CustomEvents;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enchanter : Item, ISaveable {

    public enum GemType {
        Gem = 0,
        Jewel = 15,
        Crystal = 30,
    }

    public EnchantingType Type;
    [Tooltip("Gem/Crystal/Other")]
    public GemType GemTypeType;
    public int RandomAddition;

    private int RandomlyGeneratedValue;

    public override string GetDescription() {
        return base.GetDescription();
    }

    public override void OnAfterObtained() {

    }

    public override bool ShouldCreateNewInstanceWhenPlayerObtained() {
        return true;
    }

    public override string GetName() {
        return base.GetName();
    }


    private class SaveObject {
        public int ID;
        public int RandomlyGeneratedNumber;
    }

    public string GetSaveString() {
        return JsonUtility.ToJson(new SaveObject() { RandomlyGeneratedNumber = RandomlyGeneratedValue, ID = ID });
    }

    public void SetData(string saveString, params object[] otherData) {

        SaveObject so = JsonUtility.FromJson<SaveObject>(saveString);
        ID = so.ID;
        RandomlyGeneratedValue = so.RandomlyGeneratedNumber;

        Enchanter original;

        if(otherData[0] is PersistentSetItem) {
            original = (otherData[0] as PersistentSetItem).First(it => it.ID == ID) as Enchanter;
        } else if (otherData[0] is Enchanter) {
            original = otherData[0] as Enchanter;
        } else {
            throw new System.Exception("Other data was not persistent set or enchanter");
        }

        GemTypeType = original.GemTypeType;
        Type = original.Type;
        CopyValues(original);
    }
}
