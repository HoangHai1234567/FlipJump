using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public static class GroundShapeSetup
{
    [MenuItem("Tools/Setup GroundShape Assets")]
    public static void Setup()
    {
        CreateSpriteShapeProfile();
        CreateGroundShapePrefab();
        RegisterInLevelLoader();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[GroundShapeSetup] All done! SpriteShape profile + GroundShape prefab created.");
    }

    private static void CreateSpriteShapeProfile()
    {
        string dir = "Assets/SpriteShape";
        if (!AssetDatabase.IsValidFolder(dir))
            AssetDatabase.CreateFolder("Assets", "SpriteShape");

        string path = dir + "/GrassStone.asset";
        if (AssetDatabase.LoadAssetAtPath<SpriteShape>(path) != null)
        {
            Debug.Log("[GroundShapeSetup] GrassStone profile already exists, skipping.");
            return;
        }

        SpriteShape profile = ScriptableObject.CreateInstance<SpriteShape>();

        // Set fill texture
        Texture2D fillTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Textures/Terrain/core_stone.png");
        if (fillTex != null)
            profile.fillTexture = fillTex;
        else
            Debug.LogWarning("[GroundShapeSetup] core_stone.png not found at Assets/Textures/Terrain/");

        // Set edge sprite - need to load as Sprite
        Sprite edgeSprite = LoadSpriteFromTexture("Assets/Textures/Terrain/surface_grass.png");
        if (edgeSprite != null)
        {
            profile.angleRanges.Clear();
            AngleRange range = new AngleRange();
            range.start = -180f;
            range.end = 180f;
            range.order = 0;
            range.sprites = new System.Collections.Generic.List<Sprite> { edgeSprite };
            profile.angleRanges.Add(range);
        }
        else
        {
            Debug.LogWarning("[GroundShapeSetup] surface_grass.png sprite not found. Make sure texture import is set to Sprite.");
        }

        profile.useSpriteBorders = true;

        AssetDatabase.CreateAsset(profile, path);
        Debug.Log($"[GroundShapeSetup] Created SpriteShape profile at {path}");
    }

    private static void CreateGroundShapePrefab()
    {
        string path = "Assets/Prefabs/GroundShape.prefab";
        if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null)
        {
            Debug.Log("[GroundShapeSetup] GroundShape prefab already exists, skipping.");
            return;
        }

        GameObject go = new GameObject("GroundShape");

        // Set tag and layer
        go.tag = "Ground";
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer >= 0)
            go.layer = groundLayer;

        // Add SpriteShapeController
        SpriteShapeController ssc = go.AddComponent<SpriteShapeController>();

        // Assign profile
        SpriteShape profile = AssetDatabase.LoadAssetAtPath<SpriteShape>("Assets/SpriteShape/GrassStone.asset");
        if (profile != null)
            ssc.spriteShape = profile;

        // Set default spline (flat ground shape)
        Spline spline = ssc.spline;
        spline.Clear();
        spline.InsertPointAt(0, new Vector3(-5f, 0f, 0f));
        spline.InsertPointAt(1, new Vector3(5f, 0f, 0f));
        spline.InsertPointAt(2, new Vector3(5f, -2f, 0f));
        spline.InsertPointAt(3, new Vector3(-5f, -2f, 0f));

        // Enable collider generation
        ssc.autoUpdateCollider = true;
        ssc.colliderDetail = 16;
        ssc.splineDetail = 16;

        // Add GroundLiftZone
        GroundLiftZone lift = go.AddComponent<GroundLiftZone>();
        lift.liftForceY = 3f;
        lift.zoneHeight = 8f;
        lift.playerLayer = LayerMask.GetMask("Player");

        // Save as prefab
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);

        Debug.Log($"[GroundShapeSetup] Created GroundShape prefab at {path}");
    }

    private static void RegisterInLevelLoader()
    {
        LevelLoader loader = Object.FindFirstObjectByType<LevelLoader>();
        if (loader == null)
        {
            Debug.LogWarning("[GroundShapeSetup] No LevelLoader found in scene. Please add GroundShape to prefabRegistry manually.");
            return;
        }

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Terrain - GroundShape.prefab");
        if (prefab == null) return;

        // Check if already registered
        if (loader.prefabRegistry != null)
        {
            foreach (PrefabEntry entry in loader.prefabRegistry)
            {
                if (entry.name == "Terrain - GroundShape")
                {
                    Debug.Log("[GroundShapeSetup] Terrain - GroundShape already in prefabRegistry.");
                    return;
                }
            }
        }

        // Add to registry
        var list = new System.Collections.Generic.List<PrefabEntry>(loader.prefabRegistry ?? new PrefabEntry[0]);
        list.Add(new PrefabEntry { name = "Terrain - GroundShape", prefab = prefab });
        loader.prefabRegistry = list.ToArray();

        EditorUtility.SetDirty(loader);
        Debug.Log("[GroundShapeSetup] Added GroundShape to LevelLoader prefabRegistry.");
    }

    private static Sprite LoadSpriteFromTexture(string path)
    {
        // Try to load sprite directly
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        foreach (Object asset in assets)
        {
            if (asset is Sprite sprite)
                return sprite;
        }

        // If no sprite found, configure the texture importer
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null && importer.textureType != TextureImporterType.Sprite)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.SaveAndReimport();

            assets = AssetDatabase.LoadAllAssetsAtPath(path);
            foreach (Object asset in assets)
            {
                if (asset is Sprite sprite)
                    return sprite;
            }
        }

        return null;
    }
}
