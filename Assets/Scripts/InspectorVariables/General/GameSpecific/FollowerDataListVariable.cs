using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Variables {

    public class FollowerDataListEvent : UnityEvent<List<FollowerData>> { }

    [CreateAssetMenu(menuName = "Variable/FollowerDataListVariable")]
    public class FollowerDataListVariable : ListVariable<FollowerData, FollowerDataListEvent> {
    }
}

