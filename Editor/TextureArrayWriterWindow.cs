using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace RocketPrinter.Tweaks.Editor
{
    public class TextureArrayWriterWindow : EditorWindow
    {
        string path;
        public Texture2D[] textures;
        SerializedProperty texturesSP;

        SerializedObject windowSO;

        [MenuItem("Tweaks/Texture Array Writer", priority = 3)]
        public static void ShowWindow()
        {
            GetWindow(typeof(TextureArrayWriterWindow), false, "Texture Array Writer");
        }

        private void OnEnable()
        {
            //https://answers.unity.com/questions/859554/editorwindow-display-array-dropdown.html
            windowSO = new SerializedObject(this);
            texturesSP = windowSO.FindProperty("textures");
        }

        void OnGUI()
        {
            path = EditorGUILayout.TextField("Assets/", path);
            EditorGUILayout.LabelField("Default extension: .asset");
            EditorGUILayout.LabelField("All textures must have the same size and format");

            EditorGUILayout.PropertyField(texturesSP, true);
            windowSO.ApplyModifiedProperties();

            if (GUILayout.Button("Set path to selected object"))
            {
                string[] assetGUID = Selection.assetGUIDs;
                if (assetGUID.Length > 0)
                {
                    string newPath = AssetDatabase.GUIDToAssetPath(assetGUID[0]);
                    if (newPath.StartsWith("Assets/"))
                    {
                        path = Path.ChangeExtension(newPath.Substring("Assets/".Length),"asset");
                    }
                    else
                        Debug.LogWarning("Asset must be in Assets/");
                }
                else
                    Debug.LogWarning("No asset selected!");
            }

            if (GUILayout.Button("Write"))
            {
                if (textures.Length == 0)
                {
                    Debug.LogWarning("You must use at least one texture!");
                    return;
                }

                string fullPath = "Assets/" + path;
                if (Path.GetExtension(path) == "")
                    fullPath += ".asset";

                Texture2DArray obj = new Texture2DArray(textures[0].width,textures[0].height, textures.Length, textures[0].format, true, false);

                for (int i=0;i<textures.Length;i++)
                {
                    Graphics.CopyTexture(textures[i], 0, obj, i);
                }

                AssetDatabase.CreateAsset(obj, fullPath);
            }
        }
    }
}