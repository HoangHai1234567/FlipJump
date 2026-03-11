using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

public class LevelEditorWindow : EditorWindow
{
    private string levelName = "New Level";
    private string lastSavePath = "";

    private const string PrefabSearchFolder = "Assets/Prefabs";
    private const string LevelContainerName = "Level";
    private static readonly string[] ContainerNames = { "Obstacles", "Background", "Terrain", "DecorObject" };

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

        GUI.backgroundColor = new Color(0.3f, 0.9f, 0.3f);
        if (GUILayout.Button("▶  Play Level", GUILayout.Height(40)))
            PlayLevel();
        GUI.backgroundColor = Color.white;

        EditorGUILayout.Space();

        if (!string.IsNullOrEmpty(lastSavePath))
            EditorGUILayout.HelpBox($"Last saved: {lastSavePath}", MessageType.Info);
    }

    private void PlayLevel()
    {
        Transform container = FindLevelContainer();
        if (container == null) return;

        Dictionary<string, GameObject> prefabMap = BuildPrefabMap();
        Dictionary<GameObject, string> reversePrefabMap = new Dictionary<GameObject, string>();
        foreach (var kvp in prefabMap)
            reversePrefabMap[kvp.Value] = kvp.Key;

        List<LevelElement> elements = new List<LevelElement>();
        CollectElements(container, prefabMap, reversePrefabMap, elements, null);
        foreach (string cName in ContainerNames)
        {
            Transform sub = container.Find(cName);
            if (sub != null)
                CollectElements(sub, prefabMap, reversePrefabMap, elements, cName);
        }

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

        string json = JsonUtility.ToJson(data);
        SessionState.SetString("LevelEditor_PlayJson", json);
        EditorApplication.isPlaying = true;
    }

    private void NewLevel()
    {
        Transform container = FindOrCreateLevelContainer();

        Undo.RegisterFullObjectHierarchyUndo(container.gameObject, "New Level");

        foreach (string name in ContainerNames)
        {
            Transform sub = container.Find(name);
            if (sub != null)
                ClearContainer(sub);
            else
            {
                GameObject go = new GameObject(name);
                go.transform.SetParent(container);
                Undo.RegisterCreatedObjectUndo(go, $"Create {name} Container");
            }
        }

        levelName = "New Level";
        EditorUtility.SetDirty(container.gameObject);
        Debug.Log("[LevelEditor] New level created.");
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

        CollectElements(container, prefabMap, reversePrefabMap, elements, null);
        foreach (string cName in ContainerNames)
        {
            Transform sub = container.Find(cName);
            if (sub != null)
                CollectElements(sub, prefabMap, reversePrefabMap, elements, cName);
        }

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

        // Clear all sub-containers and direct children
        foreach (string cName in ContainerNames)
        {
            Transform sub = container.Find(cName);
            if (sub != null)
                ClearContainer(sub);
        }
        ClearContainerExceptList(container, ContainerNames);

        // Ensure all containers exist
        Dictionary<string, Transform> containers = new Dictionary<string, Transform>();
        foreach (string cName in ContainerNames)
        {
            Transform sub = container.Find(cName);
            if (sub == null)
            {
                GameObject go = new GameObject(cName);
                go.transform.SetParent(container);
                sub = go.transform;
                Undo.RegisterCreatedObjectUndo(go, $"Create {cName} Container");
            }
            containers[cName] = sub;
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
                    Transform parent;
                    if (!string.IsNullOrEmpty(el.category) && containers.TryGetValue(el.category, out Transform sub))
                        parent = sub;
                    else if (string.IsNullOrEmpty(el.category))
                        parent = container;
                    else
                    {
                        // Backward compat: unknown category → use IsObstaclePrefab
                        parent = IsObstaclePrefab(el.prefab, prefabMap) ? containers["Obstacles"] : container;
                    }

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
                { SpawnEditorElement(el, containers["Obstacles"], prefabMap); count++; }
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
        ApplySplinePoints(go, el.splinePoints);

        // Record spline overrides so they survive Play mode
        SpriteShapeController ssc = go.GetComponent<SpriteShapeController>();
        if (ssc != null)
        {
            PrefabUtility.RecordPrefabInstancePropertyModifications(ssc);
            EditorUtility.SetDirty(ssc);
        }

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

    private static bool IsContainerName(string name)
    {
        foreach (string c in ContainerNames)
            if (c == name) return true;
        return false;
    }

    private static void CollectElements(Transform parent, Dictionary<string, GameObject> prefabMap,
        Dictionary<GameObject, string> reversePrefabMap, List<LevelElement> elements, string category)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            // Skip sub-containers when collecting from root
            if (category == null && IsContainerName(child.name))
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
                category = category,
                position = new float[] { child.position.x, child.position.y, child.position.z },
                scale = new float[] { child.localScale.x, child.localScale.y, child.localScale.z }
            };

            Vector3 euler = child.eulerAngles;
            if (euler != Vector3.zero)
                el.rotation = new float[] { euler.x, euler.y, euler.z };

            el.components = SerializeComponents(child.gameObject);
            el.splinePoints = SerializeSplinePoints(child.gameObject);

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
                    new ComponentProperty { name = "playerLayer", value = lift.playerLayer.value },
                    new ComponentProperty { name = "groundLayer", value = lift.groundLayer.value },
                }
            });
        }

        WinZone win = go.GetComponent<WinZone>();
        if (win != null)
        {
            List<ComponentProperty> winProps = new List<ComponentProperty>
            {
                new ComponentProperty { name = "delay", value = win.delay },
            };

            if (win.celebrateSpawnPoint != null)
            {
                Vector3 lp = win.celebrateSpawnPoint.localPosition;
                winProps.Add(new ComponentProperty { name = "posX", value = lp.x });
                winProps.Add(new ComponentProperty { name = "posY", value = lp.y });
            }

            list.Add(new ComponentData
            {
                type = "WinZone",
                properties = winProps.ToArray()
            });
        }

        CreeperAttach creeper = go.GetComponent<CreeperAttach>();
        if (creeper != null)
        {
            list.Add(new ComponentData
            {
                type = "CreeperAttach",
                properties = new[]
                {
                    new ComponentProperty { name = "angleA", value = creeper.angleA },
                    new ComponentProperty { name = "angleB", value = creeper.angleB },
                    new ComponentProperty { name = "moveSpeed", value = creeper.moveSpeed },
                    new ComponentProperty { name = "releaseForce", value = creeper.releaseForce },
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
                        case "playerLayer": lift.playerLayer = (int)p.value; break;
                        case "groundLayer": lift.groundLayer = (int)p.value; break;
                    }
                }
            }
            else if (cd.type == "WinZone")
            {
                WinZone win = go.GetComponent<WinZone>();
                if (win == null) continue;
                float posX = 0f, posY = 0f;
                bool hasPos = false;
                foreach (ComponentProperty p in cd.properties)
                {
                    switch (p.name)
                    {
                        case "delay": win.delay = p.value; break;
                        case "posX": posX = p.value; hasPos = true; break;
                        case "posY": posY = p.value; break;
                    }
                }
                if (hasPos && win.celebrateSpawnPoint != null)
                    win.celebrateSpawnPoint.localPosition = new Vector3(posX, posY, 0f);
            }
            else if (cd.type == "CreeperAttach")
            {
                CreeperAttach creeper = go.GetComponent<CreeperAttach>();
                if (creeper == null) continue;
                foreach (ComponentProperty p in cd.properties)
                {
                    switch (p.name)
                    {
                        case "angleA": creeper.angleA = p.value; break;
                        case "angleB": creeper.angleB = p.value; break;
                        case "moveSpeed": creeper.moveSpeed = p.value; break;
                        case "releaseForce": creeper.releaseForce = p.value; break;
                    }
                }
            }
        }
    }

    private static SplinePointData[] SerializeSplinePoints(GameObject go)
    {
        SpriteShapeController ssc = go.GetComponent<SpriteShapeController>();
        if (ssc == null) return null;

        Spline spline = ssc.spline;
        int count = spline.GetPointCount();
        if (count == 0) return null;

        SplinePointData[] points = new SplinePointData[count];
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = spline.GetPosition(i);
            Vector3 lt = spline.GetLeftTangent(i);
            Vector3 rt = spline.GetRightTangent(i);
            int mode = (int)spline.GetTangentMode(i);

            points[i] = new SplinePointData
            {
                position = new float[] { pos.x, pos.y, pos.z },
                leftTangent = new float[] { lt.x, lt.y, lt.z },
                rightTangent = new float[] { rt.x, rt.y, rt.z },
                mode = mode
            };
        }
        return points;
    }

    private static void ApplySplinePoints(GameObject go, SplinePointData[] points)
    {
        if (points == null || points.Length == 0) return;

        SpriteShapeController ssc = go.GetComponent<SpriteShapeController>();
        if (ssc == null) return;

        Undo.RecordObject(ssc, "Apply Spline Points");

        Spline spline = ssc.spline;
        spline.Clear();

        for (int i = 0; i < points.Length; i++)
        {
            float[] p = points[i].position;
            spline.InsertPointAt(i, new Vector3(p[0], p[1], p[2]));

            spline.SetTangentMode(i, ShapeTangentMode.Broken);

            float[] lt = points[i].leftTangent;
            spline.SetLeftTangent(i, new Vector3(lt[0], lt[1], lt[2]));

            float[] rt = points[i].rightTangent;
            spline.SetRightTangent(i, new Vector3(rt[0], rt[1], rt[2]));
        }

        ssc.BakeCollider();
        EditorUtility.SetDirty(ssc);
    }

    private static bool IsGroundPrefab(string prefabName)
    {
        return prefabName == "Square" || prefabName == "Platform" || prefabName == "Terrain - GroundShape";
    }

    /// <summary>
    /// Returns true if the prefab is an obstacle (not ground, not a zone).
    /// </summary>
    private static bool IsObstaclePrefab(string prefabName, Dictionary<string, GameObject> prefabMap)
    {
        if (prefabName == "Square" || prefabName == "Platform" || prefabName == "PushForce" || prefabName == "WinZone" || prefabName == "Terrain - GroundShape")
            return false;
        return true;
    }

    private static void ClearContainer(Transform container)
    {
        if (container == null) return;
        for (int i = container.childCount - 1; i >= 0; i--)
            Undo.DestroyObjectImmediate(container.GetChild(i).gameObject);
    }

    private static void ClearContainerExceptList(Transform container, string[] keepNames)
    {
        if (container == null) return;
        HashSet<string> keep = new HashSet<string>(keepNames);
        for (int i = container.childCount - 1; i >= 0; i--)
        {
            Transform child = container.GetChild(i);
            if (!keep.Contains(child.name))
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
