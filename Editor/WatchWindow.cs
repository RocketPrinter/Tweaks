using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace RocketPrinter.Tweaks.Editor
{
    // THIS CLASS IS WIP AND BROKEN
    class WatchWindow : EditorWindow
    {
        public class WatchField
        {
            public string name;
            public bool important;
            public FieldInfo fi;
        }
        static WatchWindow instance;
        [UnityEditor.Callbacks.DidReloadScripts] 
        static void ClearWatchFields()
        {
            if (instance!=null)
                instance.watchFields = null;
        }

        List<WatchField> watchFields; 

        public void GenerateWatchFields(bool filter)
        {
            watchFields = new List<WatchField>();

            //project root path
            string path = Application.dataPath;
            path = path.Remove(path.Length - @"/Assets".Length, @"/Assets".Length);
            path = path.Replace('/', '\\');

            //get assemblies and filter them
            Assembly[] assemblies = (from assembly in System.AppDomain.CurrentDomain.GetAssemblies()
                                     where !filter || assembly.Location.Contains(path)
                                     select assembly).ToArray();

            //debug
            //foreach (Assembly item in assemblies)
            //{
            //    Debug.Log($"Loaded:  {item.GetTypes().Length}   {item.Location}    {item.GetName()}");
            //}

            // get the types
            System.Type[] types = (from asm in assemblies select asm.GetTypes()).SelectMany(array => array).ToArray();

            Debug.Log("Loaded " + assemblies.Count().ToString() + " Assemblies and " + types.Count().ToString() + " Types.");
            foreach (System.Type type in types)
            {
                //get all the fields of the type
                FieldInfo[] fields = type.GetFields();
                foreach (FieldInfo field in fields)
                {
                    System.Attribute attrib = field.GetCustomAttribute(typeof(WatchAttribute));
                    if (attrib != null)
                    {
                        WatchAttribute wAttrib = attrib as WatchAttribute;

                        WatchField wf = new WatchField()
                        {
                            name = (wAttrib.name == null ? field.Name : wAttrib.name),
                            important = wAttrib.important,
                            fi = field,
                        };

                        watchFields.Add(wf);
                    }
                }
            }
        }

        //bool autoRefreshValues,refreshValues;
        bool onlyImportant;
        bool autoRefreshWatches; 

        //[MenuItem("Tweaks/Watches", priority = 2)]
        public static void ShowWindow()
        {
            GetWindow(typeof(WatchWindow),false,"Watches");
        }

        void OnGUI()
        {
            // reloading options
            int width = Screen.width;

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(width/2));
                {
                    //autoRefreshValues = EditorGUILayout.Toggle("Auto Refresh Values",autoRefreshValues);
                    
                    autoRefreshWatches = EditorGUILayout.Toggle("Auto Refresh Watches", autoRefreshWatches);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUILayout.Width(width / 2));
                {
                    //if (GUILayout.Button("Refresh Values")) refreshValues = true;
                    //else refreshValues = false;
                    if (autoRefreshWatches==true||GUILayout.Button("Refresh Watches")) GenerateWatchFields(true);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            onlyImportant = EditorGUILayout.Toggle("Only Important Watches", onlyImportant);
            EditorGUILayout.Space();

            if (watchFields == null) return;

            EditorGUILayout.LabelField(watchFields.Count.ToString() + " Found");
            //draw fields
            foreach (WatchField field in watchFields)
            {
                //EditorGUILayout.ObjectField(field.name,(Object)field.obj,field.type,true,GUILayout.Width(width));
            }
        }
    }
}