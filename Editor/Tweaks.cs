using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

namespace RocketPrinter.Tweaks.Editor
{
    public static partial class Tweaks
    {
        [MenuItem("Tweaks/Recompile",priority = 0)]
        public static void Recompile() 
        {
            UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
        }

        [MenuItem("Tweaks/Select GameObjects w MonoBehaviour", priority = 1)]
        public static void SelectGOwMonoBehaviour()
        { 
            // new and more messy mess
            // yes this is intentional
            Selection.objects = (from type in (from obj in Selection.objects where obj is MonoScript select (obj as MonoScript).GetClass()) select (from obj in Object.FindObjectsOfType(type) select (obj as MonoBehaviour).gameObject) ).SelectMany(array => array).ToArray();

            // old mess
            // //get current selection
            // Object[] objects = Selection.objects;
            // //filter and convert selection
            // System.Type[] types = (from obj in objects where obj is MonoScript select (obj as MonoScript).GetClass() ).ToArray();
            // // get all the components and create an array of their gameobjects
            // GameObject[] wtf = (from type in types select (from obj in Object.FindObjectsOfType(type) select (obj as MonoBehaviour).gameObject).ToArray()).SelectMany(array => array).ToArray();
            // //set selection
            // Selection.objects = wtf;
        }

        [MenuItem("Tweaks/Screenshot", priority = 2)]
        public static void Screenshot()
        {
            if (Application.isPlaying)
            {
                Debug.Log("Screenshot written to screenshot.png in project's root");
                ScreenCapture.CaptureScreenshot("screenshot.png");
            }
            else Debug.LogWarning("You must be in play mode to take a scresnhot");
        }
    }
}
