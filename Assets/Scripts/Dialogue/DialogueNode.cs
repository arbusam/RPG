using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [TextArea] public string text;
        public List<string> children = new List<string>();
        public Rect rectPosition = new Rect(0, 0, 200, 90);
    }
}