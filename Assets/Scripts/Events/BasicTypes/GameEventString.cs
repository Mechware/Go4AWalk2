using System;
using UnityEngine;

namespace G4AW2.Events {
    [Serializable]
    [CreateAssetMenu(menuName = "Events/String")]
    public class GameEventString : GameEventGeneric<string, GameEventString, UnityEventString> {
    }
}