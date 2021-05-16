using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace RocketPrinter.Tweaks
{
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class WatchAttribute : System.Attribute
    {
        public string name { get; private set; }
        public bool important { get; private set; }

        public WatchAttribute(string name=null, bool important=false)
        {
            this.name = name;
            this.important = important;
        }
    }
}