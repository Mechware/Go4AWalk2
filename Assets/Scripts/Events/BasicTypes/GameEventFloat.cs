using System;
using UnityEngine;

namespace G4AW2.Events {
    [Serializable]
    [CreateAssetMenu(menuName = "Events/Float")]
    public class GameEventFloat : GameEventGeneric<float, GameEventFloat, UnityEventFloat> {
    }
}
