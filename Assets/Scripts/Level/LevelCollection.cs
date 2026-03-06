using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCollection", menuName = "FlipJump/Level Collection")]
public class LevelCollection : ScriptableObject
{
    [Serializable]
    public class LevelEntry
    {
        public string name;
        public TextAsset levelJson;
    }

    public LevelEntry[] levels;

    public int Count => levels != null ? levels.Length : 0;

    public LevelEntry GetLevel(int index)
    {
        if (levels == null || levels.Length == 0) return null;
        int clamped = Mathf.Clamp(index, 0, levels.Length - 1);
        return levels[clamped];
    }
}
