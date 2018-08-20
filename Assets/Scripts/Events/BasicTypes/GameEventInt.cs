using System;
using UnityEngine;

namespace G4AW2.Events {
    [Serializable]
    [CreateAssetMenu(menuName = "Events/Int")]
    public class GameEventInt : GameEventGeneric<int, GameEventInt, UnityEventInt> {
    }
}
