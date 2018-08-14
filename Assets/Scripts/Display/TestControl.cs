using System.Collections;
using System.Collections.Generic;
using DropSystem;
using TMPro;
using UnityEngine;

namespace Displays
{
    public class TestControl : MonoBehaviour
    {
        public ItemDropper MyItemDropper;

        void Start() {
            MyItemDropper.TestGetItems();
        }

#if UNITY_EDITOR
        [ContextMenu("ContextTest")]
        public void ContextMenuTest()
        {
            print("Context test");
        }

        [UnityEditor.MenuItem("Test/MyTest", false, 10)]
        public static void MenuTest()
        {
            print("Menu test");
        }
#endif
    }
}
