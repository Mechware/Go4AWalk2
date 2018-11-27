using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using G4AW2.Data.DropSystem;
using UnityEditorInternal;
using System.IO;
using System.Linq;
using System.Globalization;
using UnityEngine.UI;
using G4AW2.Data.Inventory;

namespace G4AW2.Tools
{
    public class ItemImporter : EditorWindow
    {

       [Serializable]
       public class Anim
        {
            public string name;
            public int frameCount;
            public int milliPerFrame;

            public Anim(string name, int frameCount, int milliPerFrame)
            {
                this.name = name;
                this.frameCount = frameCount;
                this.milliPerFrame = milliPerFrame;
            }
        }



        public Sprite SpritePNG;        //Inventory sprite
        public Texture2D AnimationPNG;  //Animation spritesheet
        public int SpriteSizes;         //Size to cut spritesheet

        public string ItemName;         //Name of item

        public int value;             //Value of item

        public string description;      //Item description

        public Rarity rarity;           //Item rarity
        public ItemType type;           //Item type

        public Anim animation = new Anim("Walking", 8, 125);    //Animation

        public bool skipImportStep = false;

        private string path = "Assets/Data/Items/";             //Path to save items

        [MenuItem("Tools/Item Importer")]                       //Create editor window under "tools/item importer"
        static void Init()
        {
            EditorWindow window = GetWindowWithRect(typeof(ItemImporter), new Rect(0, 0, 400, 200));
            window.Show();
        }

        void OnGUI()                    
        {

            ItemName = EditorGUILayout.TextField("Item Name", ItemName);            //Enter item name
            value = EditorGUILayout.IntField("Value", value);                     //Enter item value
            description = EditorGUILayout.TextField("Description", description);    //Enter item description    

            rarity = (Rarity) EditorGUILayout.EnumPopup("Rarity", rarity);          //Select item rarity
            type = (ItemType) EditorGUILayout.EnumPopup("Item Type", type);         //Select item type

            ScriptableObject target = this;                                         //Create a scriptableObject
            SerializedObject so = new SerializedObject(target);                     //Create a serializedObject
            SerializedProperty animProperty = so.FindProperty("AnimationPNG");      //Get serializedproperty of animation spritesheet
            SerializedProperty spriteProperty = so.FindProperty("SpritePNG");       //Get serializedproperty of sprite
            EditorGUILayout.PropertyField(spriteProperty, true);                    //Select item inventory sprite
            so.ApplyModifiedProperties();                                           //Apply modified properties

            switch (type)                                                           //Item type switch statement
            {
                case ItemType.Accessory:
                    path = "Assets/Data/Items/Accessories";                                      //Assign path to correct folder
                    break;
                case ItemType.Boots:
                    EditorGUILayout.PropertyField(animProperty, true);              //select animation spritesheet
                    SpriteSizes = EditorGUILayout.IntField("Sprite Size (32/64)", SpriteSizes);     //Choose spritesize
                    EditorGUILayout.LabelField("Don't forget to loop the animation!");
                    path = "Assets/Data/Items/Boots";
                    break;
                case ItemType.Consumable:
                    path = "Assets/Data/Items/Consumables";
                    break;
                case ItemType.Hat:
                    EditorGUILayout.PropertyField(animProperty, true);
                    SpriteSizes = EditorGUILayout.IntField("Sprite Size (32/64)", SpriteSizes);
                    EditorGUILayout.LabelField("Don't forget to loop the animation!");
                    path = "Assets/Data/Items/Hats";
                    break;
                case ItemType.Material:
                    path = "Assets/Data/Items/Materials";
                    break;
                case ItemType.Torso:
                    EditorGUILayout.PropertyField(animProperty, true);
                    SpriteSizes = EditorGUILayout.IntField("Sprite Size (32/64)", SpriteSizes);
                    EditorGUILayout.LabelField("Don't forget to loop the animation!");
                    path = "Assets/Data/Items/Armors";
                    break;
                case ItemType.Weapon:
                    EditorGUILayout.PropertyField(animProperty, true);
                    SpriteSizes = EditorGUILayout.IntField("Sprite Size (32/64)", SpriteSizes);
                    EditorGUILayout.LabelField("Don't forget to loop the animation!");
                    path = "Assets/Data/Items/Weapons";
                    break;
            }

            so.ApplyModifiedProperties();           

            //skipImportStep = EditorGUILayout.Toggle("Skip Import Step?", skipImportStep);

            if (GUILayout.Button("Create Item"))
            {
                 string animPath = AssetDatabase.GetAssetPath(AnimationPNG);
                                     
                if (!skipImportStep)
                {
                    TextureImporter importer = (TextureImporter) TextureImporter.GetAtPath(animPath);
                    importer.wrapMode = TextureWrapMode.Clamp;
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Multiple;
                    importer.filterMode = FilterMode.Point;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;

                    EditorUtility.SetDirty(importer);
                    importer.SaveAndReimport();
                
                    Rect[] rects = InternalSpriteUtility.GenerateGridSpriteRectangles(AnimationPNG, Vector2.zero, new Vector2(SpriteSizes, SpriteSizes),
                    Vector2.zero);
                
                    string filenameNoExtension = Path.GetFileNameWithoutExtension(animPath);
                    List<SpriteMetaData> metas = new List<SpriteMetaData>();
                    int rectNum = 0;
                
                    foreach (Rect rect in rects)
                    {
                        SpriteMetaData meta = new SpriteMetaData();
                        meta.rect = rect;
                        meta.name = filenameNoExtension + "_" + rectNum++;
                        metas.Add(meta);
                    }
                    
                    importer.spritesheet = metas.ToArray();
                
                    EditorUtility.SetDirty(importer);
                    importer.SaveAndReimport();
                }

                Sprite[] spriteSheet = AssetDatabase.LoadAllAssetsAtPath(animPath).OfType<Sprite>().ToArray();
                spriteSheet = spriteSheet.OrderBy(l => l.name, new NaturalComparer()).ToArray();

                bool loadedData = true;
                string ItemPath = Path.Combine(path, ItemName + ".asset");

                Item itemScriptableObject = AssetDatabase.LoadAssetAtPath<Item>(ItemPath);

                if(itemScriptableObject == null)
                {
                    itemScriptableObject = CreateInstance<Item>();
                    loadedData = false;
                }

                itemScriptableObject.rarity = rarity;
                itemScriptableObject.type = type;
                itemScriptableObject.value = value;
                itemScriptableObject.description = description;
                itemScriptableObject.image = SpritePNG;

                AnimationClip ac = CreateAnimation(animation, spriteSheet, ItemName, animPath);

                itemScriptableObject.Walking = ac;


                if (!loadedData) AssetDatabase.CreateAsset(itemScriptableObject, ItemPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }

        }

        private AnimationClip CreateAnimation(Anim a, Sprite[] Sprites, string itemName, string pngPath)
        {
            AnimationClip clip = new AnimationClip();
            float FramesPerSecond = 1000f / a.milliPerFrame;
            clip.frameRate = FramesPerSecond;

            EditorCurveBinding curveBinding = new EditorCurveBinding();

            curveBinding.type = typeof(Image);
            curveBinding.path = "";
            curveBinding.propertyName = "m_Sprite";

            ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[Sprites.Length];
            for (int i = 0 ; i < Sprites.Length ; i++)
            {
                keyframes[i] = new ObjectReferenceKeyframe();
                keyframes[i].time = i / FramesPerSecond;
                keyframes[i].value = Sprites[i];
            }

            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);
           // clip.wrapMode = WrapMode.Loop;

           /* AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
            clipSettings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, clipSettings);*/


            string saveLocation = Path.Combine(Path.GetDirectoryName(pngPath), itemName + a.name + ".anim");

            AssetDatabase.CreateAsset(clip, saveLocation);        

            return clip;
        }

        private class NaturalComparer : IComparer<string>
        {
            private readonly CultureInfo _CultureInfo = CultureInfo.CurrentCulture;

            public int Compare(string x, string y)
            {
                // simple cases
                if (x == y) // also handles null
                    return 0;
                if (x == null)
                    return -1;
                if (y == null)
                    return +1;

                int ix = 0;
                int iy = 0;
                while (ix < x.Length && iy < y.Length)
                {
                    if (Char.IsDigit(x[ix]) && Char.IsDigit(y[iy]))
                    {
                        // We found numbers, so grab both numbers
                        int ix1 = ix++;
                        int iy1 = iy++;
                        while (ix < x.Length && Char.IsDigit(x[ix]))
                            ix++;
                        while (iy < y.Length && Char.IsDigit(y[iy]))
                            iy++;
                        string numberFromX = x.Substring(ix1, ix - ix1);
                        string numberFromY = y.Substring(iy1, iy - iy1);

                        // Pad them with 0's to have the same length
                        int maxLength = Math.Max(
                            numberFromX.Length,
                            numberFromY.Length);
                        numberFromX = numberFromX.PadLeft(maxLength, '0');
                        numberFromY = numberFromY.PadLeft(maxLength, '0');

                        int comparison = _CultureInfo
                            .CompareInfo.Compare(numberFromX, numberFromY);
                        if (comparison != 0)
                            return comparison;
                    } else
                    {
                        int comparison = _CultureInfo
                            .CompareInfo.Compare(x, ix, 1, y, iy, 1);
                        if (comparison != 0)
                            return comparison;
                        ix++;
                        iy++;
                    }
                }

                // we should not be here with no parts left, they're equal
                Debug.Assert(ix < x.Length || iy < y.Length);

                // we still got parts of x left, y comes first
                if (ix < x.Length)
                    return +1;

                // we still got parts of y left, x comes first
                return -1;
            }
        }

    }
}
