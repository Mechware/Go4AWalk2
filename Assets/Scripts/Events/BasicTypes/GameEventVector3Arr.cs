using System;
using UnityEngine;

namespace G4AW2.Events {
    [Serializable]
    [CreateAssetMenu(menuName = "Events/Vector3Array")]
    public class GameEventVector3Arr : GameEventGeneric<Vector3[], GameEventVector3Arr, UnityEventVector3Arr> {
    }
}
