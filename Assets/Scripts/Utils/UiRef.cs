using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Scripts.DoubleJump {
    [AttributeUsage(AttributeTargets.Field)]
    public class UiRef : Attribute {
        public string NameOverride;
        public bool Required;

        public UiRef(string nameOverride = null, bool required = true) {
            NameOverride = nameOverride;
            Required = required;
        }

        public static void Init<T>(T obj) where T : MonoBehaviour {
            Init(obj, obj.gameObject);
        }

        public static T Init<T>(GameObject parent) where T : class, new() {
            var t = Init(new T(), parent);
            return t;
        }

        public static T Init<T>(T toFill, GameObject parent) where T : class {
            var type = toFill.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach(var field in fields) {
                var attr = (UiRef) field.GetCustomAttributes(typeof(UiRef)).FirstOrDefault();
                if(attr == null)
                    continue;

                var objName = attr.NameOverride ?? field.Name;

                var child = objName == parent.name ? parent : parent.GetChild(objName);

                if(child == null) {
                    if(attr.Required)
                        Debug.LogError($"Failed to find child {objName} under obj {parent.name}");
                    continue;
                }

                var comp = child.GetComponent(field.FieldType);

                if(comp == null) {
                    if(attr.Required)
                        Debug.LogError($"Failed to find component {field.FieldType} in obj {objName} in children of {parent.name}");
                    continue;
                }


                field.SetValue(toFill, comp);
            }

            return toFill;
        }
    }
}

