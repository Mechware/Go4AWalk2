using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G4AW2.Data.DropSystem;
using UnityEngine.Events;

namespace CustomEvents
{

    [System.Serializable]
    public class MyResponse : UnityEvent<Item>
    {
    }

    public class GameEventListenerItem : MonoBehaviour
    {

        public GameEventItem Event;
        public MyResponse Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);   
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(Item item)
        {

            Response.Invoke(item);
        }
    }
}

