using System.Collections;
using System.Collections.Generic;
using G4AW2.Events;
using G4AW2.Utils;
using G4AW2.Variables;
using UnityEngine;

namespace G4AW2.Followers {
    public class FollowerSpawner : MonoBehaviour {

        public FollowerDataListVariable AllPossibleFollowers;
        public FollowerDataListVariable CurrentFollowers;

        
        public void AddFollower() {
            // randomly choose a follower!
            CurrentFollowers.Value.Add(AllPossibleFollowers.Value.GetRandom());
            CurrentFollowers.OnChange.Invoke(CurrentFollowers.Value);
        }
    }
}


