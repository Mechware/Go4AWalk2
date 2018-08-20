using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using G4AW2.Events;
using UnityEngine;

namespace G4AW2.Variables {
    [Serializable]
    public class FollowerDataListReference : ListReference<FollowerData, FollowerDataListVariable, UnityEventListFollowerData> {
    }
}
