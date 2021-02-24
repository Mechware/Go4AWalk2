using G4AW2.Data;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Followers
{
    [CreateAssetMenu(menuName = "Data/Follower/QuestGiver")]
	public class QuestGiverConfig : FollowerConfig {

		public List<QuestConfig> QuestConfigToGive;
		public AnimationClip GivingQuest;
	    public AnimationClip WalkingAnimation;
	}
}

