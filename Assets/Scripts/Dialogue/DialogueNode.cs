using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string uniqueID;
        [TextArea] public string text;
        public string[] children;
        public Rect rectPosition = new Rect(0, 0, 200, 100);
    }
}