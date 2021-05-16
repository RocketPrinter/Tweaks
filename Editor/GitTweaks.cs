using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace RocketPrinter.Tweaks.Editor
{
    public static partial class Tweaks
    {
        [MenuItem("Tweaks/Git/Init", priority = 0)]
        public static void GitInit()
        {
            System.Diagnostics.Process.Start("CMD.exe",
                 "/k cd "
                + "\"" + System.IO.Directory.GetCurrentDirectory() + " \""
                + " && git init"
                );
            VersionControlSettings.mode = "Visible Meta Files";
            Debug.LogWarning("Please add the files from GitFiles.zip to the root of the project");
            Debug.LogWarning("Please set Project Settings / Editor / Asset Serialization / Mode: “Force Text”");
        }
        [MenuItem("Tweaks/Git/Open cmd", priority = 1)]
        public static void GitOpenCMD()
        {
            System.Diagnostics.Process.Start("CMD.exe",
                 "/k cd "
                + "\"" + System.IO.Directory.GetCurrentDirectory() + " \""
                + " && git status"
                );
        }
    }
}
