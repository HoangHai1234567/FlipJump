using UnityEditor;
using UnityEngine;

public static class LevelCollectionSetup
{
    [MenuItem("Tools/Populate LevelCollection")]
    public static void Populate()
    {
        LevelCollection lc = AssetDatabase.LoadAssetAtPath<LevelCollection>("Assets/Data/LevelCollection.asset");
        if (lc == null)
        {
            lc = ScriptableObject.CreateInstance<LevelCollection>();
            if (!AssetDatabase.IsValidFolder("Assets/Data"))
                AssetDatabase.CreateFolder("Assets", "Data");
            AssetDatabase.CreateAsset(lc, "Assets/Data/LevelCollection.asset");
        }

        TextAsset l1 = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Levels/level001.json");
        TextAsset l2 = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Levels/level002.json");
        TextAsset l3 = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Levels/level003.json");

        lc.levels = new LevelCollection.LevelEntry[]
        {
            new LevelCollection.LevelEntry { name = "Level 1", levelJson = l1 },
            new LevelCollection.LevelEntry { name = "Level 2", levelJson = l2 },
            new LevelCollection.LevelEntry { name = "Level 3", levelJson = l3 },
        };

        EditorUtility.SetDirty(lc);
        AssetDatabase.SaveAssets();
        Debug.Log("[LevelCollectionSetup] LevelCollection populated with 3 levels.");
    }
}
