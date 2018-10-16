using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using G4AW2.Data.DropSystem;


    namespace CustomEvents
{

    [Serializable][CreateAssetMenu(menuName = "Events/General/Item")]
    public class GameEventItem : ScriptableObject
    {
        private readonly List<GameEventListenerItem> listeners = new List<GameEventListenerItem>();

        public void Raise(Item item)
        {
            foreach (GameEventListenerItem listener in listeners)
            {
                listener.OnEventRaised(item);
            }
        }

        public void RegisterListener(GameEventListenerItem listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListenerItem listener)
        {
            listeners.Remove(listener);
        }
    }

}
