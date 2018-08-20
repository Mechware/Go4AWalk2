using UnityEngine;
using System.Collections;
using G4AW2.Events;

namespace G4AW2.Variables {
    [CreateAssetMenu(menuName = "Variable/General/ScriptableObjectList")]
    public class ListSOVariable : ListVariable<ScriptableObject, UnityEventListSO> {
    }
}
