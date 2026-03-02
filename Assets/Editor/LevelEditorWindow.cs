using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private string levelName = "New Level";
    private string lastSavePath = "";

    private const string PrefabSearchFolder = "Assets/Prefabs";
    private const string LevelContainerName = "Level";

    [MenuItem("Tools/Level Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        levelName = EditorGUILayout.TextField("Level Name", levelName);

        EditorGUILayout.Space();

        if (GUILayout.Button("New Level", GUILayout.Height(30)))
            NewLevel();

        EditorGUILayout.Space();

        if (GUILayout.Button("Save Level", GUILayout.Height(30)))
            SaveLevel();

        if (GUILayout.Button("Load Level", GUILayout.Height(30)))
            LoadLevel();

        EditorGUILayout.Space();

        if (!string.IsNullOrEmpty(lastSavePath))
            EditorGUILayout.HelpBox($"Last saved: {lastSavePath}", MessageType.Info);
    }

    private void NewLevel()
    {
        Transform container = FindOrCreateLevelContainer();

        Undo.RegisterFullObjectHierarchyUndo(container.gameObject, "New Level");

        // Clear obstacles container only
        Transform obstacles = container.Find("Obstacles");
        if (obstacles != null)
            ClearContainer(obstacles);
        else
        {
            GameObject obstaclesGo = new GameObject("Obstacles");
            obstaclesGo.transform.SetParent(container);
            Undo.RegisterCreatedObjectUndo(obstaclesGo, "Create Obstacles Container");
        }

        levelName = "New Level";
        EditorUtility.SetDirty(container.gameObject);
        Debug.Log("[LevelEditor] New level — obstacles cleared.");
    }

    private void SaveLevel()
    {
        Transform container = FindLevelContainer();
        if (container == null) return;

        Dictionary<string, GameObject> prefabMap = BuildPrefabMap();

        // Build reverse map: prefab asset -> name
        Dictionary<GameObject, string> reversePrefabMap = new Dictionary<GameObject, string>();
        foreach (var kvp in prefabMap)
            reversePrefabMap[kvp.Value] = kvp.Key;

        List<LevelElement> elements = new List<LevelElement>();

        // Collect from Level direct children (Square, PushForce, etc.) and Obstacles children
        CollectElements(container, prefabMap, reversePrefabMap, elements, skipContainers: true);
        Transform obstacles = container.Find("Obstacles");
        if (obstacles != null)
            CollectElements(obstacles, prefabMap, reversePrefabMap, elements, skipContainers: false);

        float[] playerPos = null;
        GameObject player = FindPlayer();
        if (player != null)
        {
            Vector3 p = player.transform.position;
            playerPos = new float[] { p.x, p.y, p.z };
        }

        LevelDataV2 data = new LevelDataV2
        {
            version = 2,
            name = levelName,
            playerPosition = playerPos,
            elements = elements.ToArray()
        };

        string defaultDir = Path.Combine(Application.dataPath, "Levels");
        if (!Directory.Exists(defaultDir))
            Directory.CreateDirectory(defaultDir);

        string path = EditorUtility.SaveFilePanel("Save Level", defaultDir, levelName + ".json", "json");
        if (string.IsNullOrEmpty(path)) return;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

        lastSavePath = path;

        if (path.StartsWith(Application.dataPath))
            AssetDatabase.Refresh();

        Debug.Log($"[LevelEditor] Saved level '{levelName}' with {elements.Count} elements to {path}");
    }

    private void LoadLevel()
    {
        Transform container = FindOrCreateLevelContainer();

        string defaultDir = Path.Combine(Application.dataPath, "Levels");
        string path = EditorUtility.OpenFilePanel("Load Level", defaultDir, "json");
        if (string.IsNullOrEmpty(path)) return;

        string json = File.ReadAllText(path);
        VersionCheck vc = JsonUtility.FromJson<VersionCheck>(json);

        Undo.RegisterFullObjectHierarchyUndo(container.gameObject, "Load Level");

        // Clear obstacles, keep the Obstacles container itself
        Transform obstacles = container.Find("Obstacles");
        if (obstacles != null)
            ClearContainer(obstacles);

        // Clear direct children except Obstacles container
        ClearContainerExcept(container, "Obstacles");

        // Re-create Obstacles container if it was missing
        if (obstacles == null)
        {
            GameObject obstaclesGo = new GameObject("Obstacles");
            obstaclesGo.transform.SetParent(container);
            obstacles = obstaclesGo.transform;
            Undo.RegisterCreatedObjectUndo(obstaclesGo, "Create Obstacles Container");
        }

        Dictionary<string, GameObject> prefabMap = BuildPrefabMap();
        int groundLayerIndex = LayerMask.NameToLayer("Ground");

        if (vc != null && vc.version == 2)
        {
            LevelDataV2 data = JsonUtility.FromJson<LevelDataV2>(json);
            if (data == null) { Debug.LogError("[LevelEditor] Failed to parse V2 JSON."); return; }

            levelName = data.name ?? "Loaded Level";

            if (data.elements != null)
            {
                foreach (LevelElement el in data.elements)
                {
                    Transform parent = IsObstaclePrefab(el.prefab, prefabMap) ? obstacles : container;
                    GameObject go = SpawnEditorElement(el, parent, prefabMap);
                    if (go != null && IsGroundPrefab(el.prefab))
                    {
                        go.tag = "Ground";
                        SetLayerRecursive(go, groundLayerIndex);
                    }
                }
            }

            if (data.playerPosition != null && data.playerPosition.Length >= 3)
                SetPlayerPosition(new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]));

            Debug.Log($"[LevelEditor] Loaded V2 level '{data.name}' ({data.elements?.Length ?? 0} elements)");
        }
        else
        {
            LevelDataV1 data = JsonUtility.FromJson<LevelDataV1>(json);
            if (data == null) { Debug.LogError("[LevelEditor] Failed to parse V1 JSON."); return; }

            levelName = data.name ?? "Loaded Level";
            int count = 0;

            if (data.groundSegments != null)
                foreach (LevelElement el in data.groundSegments)
                {
                    GameObject go = SpawnEditorElement(el, container, prefabMap);
                    if (go != null) { go.tag = "Ground"; SetLayerRecursive(go, groundLayerIndex); count++; }
                }
            if (data.platforms != null)
                foreach (LevelElement el in data.platforms)
                {
                    GameObject go = SpawnEditorElement(el, container, prefabMap);
                    if (go != null) { go.tag = "Ground"; SetLayerRecursive(go, groundLayerIndex); count++; }
                }
            if (data.obstacles != null)
                foreach (LevelElement el in data.obstacles)
                { SpawnEditorElement(el, obstacles, prefabMap); count++; }
            if (data.zones != null)
                foreach (LevelElement el in data.zones)
                { SpawnEditorElement(el, container, prefabMap); count++; }

            Debug.Log($"[LevelEditor] Loaded V1 level '{data.name}' ({count} elements)");
        }

        EditorUtility.SetDirty(container.gameObject);
    }

    /// <summary>
    /// Auto-discover all prefabs under Assets/Prefabs/ and build a name-to-prefab map.
    /// </summary>
    private static Dictionary<string, GameObject> BuildPrefabMap()
    {
        Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { PrefabSearchFolder });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab != null)
                map[prefab.name] = prefab;
        }

        return map;
    }

    private static Transform FindLevelContainer()
    {
        GameObject level = GameObject.Find(LevelContainerName);
        if (level == null)
        {
            Debug.LogError($"[LevelEditor] No '{LevelContainerName}' GameObject found in scene.");
            return null;
        }
        return level.transform;
    }

    private static Transform FindOrCreateLevelContainer()
    {
        GameObject level = GameObject.Find(LevelContainerName);
        if (level == null)
        {
            level = new GameObject(LevelContainerName);
            Undo.RegisterCreatedObjectUndo(level, "Create Level Container");
        }
        return level.transform;
    }

    private static GameObject SpawnEditorElement(LevelElement el, Transform parent, Dictionary<string, GameObject> prefabMap)
    {
        if (!prefabMap.TryGetValue(el.prefab, out GameObject prefab))
        {
            Debug.LogWarning($"[LevelEditor] Unknown prefab '{el.prefab}', skipping.");
            return null;
        }

        GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);

        Vector3 pos = el.position != null && el.position.Length >= 3
            ? new Vector3(el.position[0], el.position[1], el.position[2])
            : Vector3.zero;
        go.transform.position = pos;

        if (el.rotation != null && el.rotation.Length >= 3)
            go.transform.eulerAngles = new Vector3(el.rotation[0], el.rotation[1], el.rotation[2]);

        if (el.scale != null && el.scale.Length >= 3)
            go.transform.localScale = new Vector3(el.scale[0], el.scale[1], el.scale[2]);

        ApplyComponents(go, el.components);

        Undo.RegisterCreatedObjectUndo(go, $"Spawn {el.prefab}");
        return go;
    }

    private static GameObject FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            var players = Object.FindObjectsByType<ForcePoint>(FindObjectsSortMode.None);
            if (players.Length > 0)
                player = players[0].transform.root.gameObject;
        }
        return player;
    }

    private static void SetPlayerPosition(Vector3 pos)
    {
        GameObject player = FindPlayer();
        if (player != null)
        {
            Undo.RecordObject(player.transform, "Set Player Position");
            player.transform.position = pos;
        }
    }

    private static void CollectElements(Transform parent, Dictionary<string, GameObject> prefabMap,
        Dictionary<GameObject, string> reversePrefabMap, List<LevelElement> elements, bool skipContainers)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            // Skip the Obstacles container itself (not a prefab)
            if (skipContainers && child.name == "Obstacles")
                continue;

            GameObject prefabSource = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);

            string prefabName = null;
            if (prefabSource != null && reversePrefabMap.TryGetValue(prefabSource, out string name))
            {
                prefabName = name;
            }
            else
            {
                prefabName = child.gameObject.name.Replace("(Clone)", "").Trim();
                if (!prefabMap.ContainsKey(prefabName))
                {
                    Debug.LogWarning($"[LevelEditor] Could not resolve prefab for '{child.name}', skipping.");
                    continue;
                }
            }

            LevelElement el = new LevelElement
            {
                prefab = prefabName,
                position = new float[] { child.position.x, child.position.y, child.position.z },
                scale = new float[] { child.localScale.x, child.localScale.y, child.localScale.z }
            };

            Vector3 euler = child.eulerAngles;
            if (euler != Vector3.zero)
                el.rotation = new float[] { euler.x, euler.y, euler.z };

            el.components = SerializeComponents(child.gameObject);

            elements.Add(el);
        }
    }

    private static ComponentData[] SerializeComponents(GameObject go)
    {
        List<ComponentData> list = new List<ComponentData>();

        PushForceZone push = go.GetComponent<PushForceZone>();
        if (push != null)
        {
            list.Add(new ComponentData
            {
                type = "PushForceZone",
                properties = new[]
                {
                    new ComponentProperty { name = "forceX", value = push.forceX },
                    new ComponentProperty { name = "zoneWidth", value = push.zoneWidth },
                    new ComponentProperty { name = "zoneHeight", value = push.zoneHeight },
                    new ComponentProperty { name = "maxVelocityX", value = push.maxVelocityX },
                }
            });
        }

        GroundLiftZone lift = go.GetComponent<GroundLiftZone>();
        if (lift != null)
        {
            list.Add(new ComponentData
            {
                type = "GroundLiftZone",
                properties = new[]
                {
                    new ComponentProperty { name = "liftForceY", value = lift.liftForceY },
                    new ComponentProperty { name = "zoneHeight", value = lift.zoneHeight },
                    new ComponentProperty { name = "zoneWidth", value = lift.zoneWidth },
                }
            });
        }

        WinZone win = go.GetComponent<WinZone>();
        if (win != null)
        {
            list.Add(new ComponentData
            {
                type = "WinZone",
                properties = new[]
                {
                    new ComponentProperty { name = "delay", value = win.delay },
                }
            });
        }

        return list.Count > 0 ? list.ToArray() : null;
    }

    private static void ApplyComponents(GameObject go, ComponentData[] components)
    {
        if (components == null) return;

        foreach (ComponentData cd in components)
        {
            if (cd.type == "PushForceZone")
            {
                PushForceZone push = go.GetComponent<PushForceZone>();
                if (push == null) continue;
                foreach (ComponentProperty p in cd.properties)
                {
                    switch (p.name)
                    {
                        case "forceX": push.forceX = p.value; break;
                        case "zoneWidth": push.zoneWidth = p.value; break;
                        case "zoneHeight": push.zoneHeight = p.value; break;
                        case "maxVelocityX": push.maxVelocityX = p.value; break;
                    }
                }
            }
            else if (cd.type == "GroundLiftZone")
            {
                GroundLiftZone lift = go.GetComponent<GroundLiftZone>();
                if (lift == null) continue;
                foreach (ComponentProperty p in cd.properties)
                {
                    switch (p.name)
                    {
                        case "liftForceY": lift.liftForceY = p.value; break;
                        case "zoneHeight": lift.zoneHeight = p.value; break;
                        case "zoneWidth": lift.zoneWidth = p.value; break;
                    }
                }
            }
            else if (cd.type == "WinZone")
            {
                WinZone win = go.GetComponent<WinZone>();
                if (win == null) continue;
                foreach (ComponentProperty p in cd.properties)
                {
                    switch (p.name)
                    {
                        case "delay": win.delay = p.value; break;
                    }
                }
            }
        }
    }

    private static bool IsGroundPrefab(string prefabName)
    {
        return prefabName == "Square" || prefabName == "Platform";
    }

    /// <summary>
    /// Returns true if the prefab is an obstacle (not ground, not a zone).
    /// </summary>
    private static bool IsObstaclePrefab(string prefabName, Dictionary<string, GameObject> prefabMap)
    {
        if (prefabName == "Square" || prefabName == "Platform" || prefabName == "PushForce" || prefabName == "WinZone")
            return false;
        return true;
    }

    private static void ClearContainer(Transform container)
    {
        if (container == null) return;
        for (int i = container.childCount - 1; i >= 0; i--)
            Undo.DestroyObjectImmediate(container.GetChild(i).gameObject);
    }

    private static void ClearContainerExcept(Transform container, string keepName)
    {
        if (container == null) return;
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Transform child = container.GetChild(i);
            if (child.name != keepName)
                Undo.DestroyObjectImmediate(child.gameObject);
        }
    }

    private static void SetLayerRecursive(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
            SetLayerRecursive(child.gameObject, layer);
    }
}
