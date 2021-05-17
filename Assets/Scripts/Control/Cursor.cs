using UnityEngine;

namespace RPG.Control
{
    public enum CursorType
    {
        None,
        Movement,
        Attack,
        UI,
        Collect
    }

    [System.Serializable]
    public struct CursorMapping
    {
        public CursorType type;
        public Texture2D texture;
        public Vector2 hotspot;
    }
}