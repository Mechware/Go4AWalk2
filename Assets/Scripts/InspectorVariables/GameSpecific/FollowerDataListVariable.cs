using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using G4AW2.Events;
using UnityEngine;

namespace G4AW2.Variables {
    [CreateAssetMenu(menuName = "Variable/FollowerDataListVariable")]
    public class FollowerDataListVariable : ListVariable<FollowerData, UnityEventListFollowerData> {
    }
}

