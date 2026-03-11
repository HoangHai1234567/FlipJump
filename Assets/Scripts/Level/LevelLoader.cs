using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelJson;
    public PrefabEntry[] prefabRegistry;

    public Transform levelContainer;
    public Transform groundContainer;
    public Transform platformsContainer;
    public Transform obstaclesContainer;

    public LayerMask groundLayer;
    public Transform playerTransform;

    private Dictionary<string, GameObject> prefabMap;
    private static string cachedLevelJson;

    private static readonly HashSet<string> GroundPrefabs = new HashSet<string> { "Square", "Platform", "Terrain - GroundShape" };

    private void Start()
    {
        BuildPrefabMap();

#if UNITY_EDITOR
        string editorJson = UnityEditor.SessionState.GetString("LevelEditor_PlayJson", "");
        if (!string.IsNullOrEmpty(editorJson))
        {
            UnityEditor.SessionState.EraseString("LevelEditor_PlayJson");
            cachedLevelJson = editorJson;
            LoadLevelFromJson(editorJson);
            return;
        }
#endif

        if (!string.IsNullOrEmpty(cachedLevelJson))
        {
            LoadLevelFromJson(cachedLevelJson);
            return;
        }

        LoadLevel();
    }

    public void BuildPrefabMap()
    {
        prefabMap = new Dictionary<string, GameObject>();
        foreach (PrefabEntry entry in prefabRegistry)
        {
            if (entry.prefab != null && !string.IsNullOrEmpty(entry.name))
                prefabMap[entry.name] = entry.prefab;
        }
    }

    public Dictionary<string, GameObject> GetPrefabMap()
    {
        if (prefabMap == null)
            BuildPrefabMap();
        return prefabMap;
    }

    public void LoadLevel()
    {
        if (levelJson == null)
        {
            Debug.LogWarning("[LevelLoader] No level JSON assigned.");
            return;
        }

        LoadLevelFromJson(levelJson.text);
    }

    public void LoadLevelFromJson(string json)
    {
        StartCoroutine(LoadLevelCoroutine(json));
    }

    private IEnumerator LoadLevelCoroutine(string json)
    {
        // Freeze player before spawning anything
        ForcePoint forcePoint = null;
        if (playerTransform != null)
        {
            forcePoint = playerTransform.GetComponentInChildren<ForcePoint>();
            if (forcePoint != null)
                forcePoint.FreezeAll();
        }

        VersionCheck vc = JsonUtility.FromJson<VersionCheck>(json);

        if (vc != null && vc.version == 2)
            LoadV2(json);
        else
            LoadV1(json);

        // Wait 1 frame for colliders/mesh to rebuild
        yield return null;

        // Unfreeze player
        if (forcePoint != null)
            forcePoint.FreezeAll(); // Keep frozen — ForcePoint.ApplyForce() will unfreeze on first click
    }

    private void LoadV2(string json)
    {
        LevelDataV2 data = JsonUtility.FromJson<LevelDataV2>(json);
        if (data == null)
        {
            Debug.LogError("[LevelLoader] Failed to parse V2 level JSON.");
            return;
        }

        ClearContainer(levelContainer);

        int groundLayerIndex = GetLayerIndex(groundLayer);
        Dictionary<string, Transform> containers = new Dictionary<string, Transform>();

        if (data.elements != null)
        {
            foreach (LevelElement el in data.elements)
            {
                Transform parent = levelContainer;
                if (!string.IsNullOrEmpty(el.category))
                {
                    if (!containers.TryGetValue(el.category, out parent))
                    {
                        GameObject containerGo = new GameObject(el.category);
                        containerGo.transform.SetParent(levelContainer);
                        parent = containerGo.transform;
                        containers[el.category] = parent;
                    }
                }

                GameObject go = SpawnElement(el, parent);
                if (go != null && GroundPrefabs.Contains(el.prefab))
                {
                    go.tag = "Ground";
                    SetLayerRecursive(go, groundLayerIndex);
                }
            }
        }

        if (playerTransform != null && data.playerPosition != null && data.playerPosition.Length >= 2)
        {
            playerTransform.position = new Vector3(
                data.playerPosition[0],
                data.playerPosition[1],
                data.playerPosition.Length >= 3 ? data.playerPosition[2] : 0f);
        }

        Debug.Log($"[LevelLoader] Loaded V2 level '{data.name}' ({data.elements?.Length ?? 0} elements)");
    }

    private void LoadV1(string json)
    {
        LevelDataV1 data = JsonUtility.FromJson<LevelDataV1>(json);
        if (data == null)
        {
            Debug.LogError("[LevelLoader] Failed to parse V1 level JSON.");
            return;
        }

        ClearContainer(groundContainer);
        ClearContainer(platformsContainer);
        ClearContainer(obstaclesContainer);

        int groundLayerIndex = GetLayerIndex(groundLayer);

        if (data.groundSegments != null)
        {
            foreach (LevelElement el in data.groundSegments)
            {
                GameObject go = SpawnElement(el, groundContainer);
                if (go != null)
                {
                    go.tag = "Ground";
                    SetLayerRecursive(go, groundLayerIndex);
                }
            }
        }

        if (data.platforms != null)
        {
            foreach (LevelElement el in data.platforms)
            {
                GameObject go = SpawnElement(el, platformsContainer);
                if (go != null)
                {
                    go.tag = "Ground";
                    SetLayerRecursive(go, groundLayerIndex);
                }
            }
        }

        if (data.obstacles != null)
        {
            foreach (LevelElement el in data.obstacles)
                SpawnElement(el, obstaclesContainer);
        }

        if (data.zones != null)
        {
            foreach (LevelElement el in data.zones)
                SpawnElement(el, groundContainer);
        }

        Debug.Log($"[LevelLoader] Loaded V1 level '{data.name}'");
    }

    private GameObject SpawnElement(LevelElement el, Transform parent)
    {
        if (parent == null)
        {
            Debug.LogWarning($"[LevelLoader] No container for element '{el.prefab}', skipping.");
            return null;
        }

        if (prefabMap == null)
            BuildPrefabMap();

        if (!prefabMap.TryGetValue(el.prefab, out GameObject prefab))
        {
            Debug.LogWarning($"[LevelLoader] Unknown prefab '{el.prefab}', skipping.");
            return null;
        }

        Vector3 pos = el.position != null && el.position.Length >= 3
            ? new Vector3(el.position[0], el.position[1], el.position[2])
            : Vector3.zero;

        Quaternion rot = Quaternion.identity;
        if (el.rotation != null && el.rotation.Length >= 3)
            rot = Quaternion.Euler(el.rotation[0], el.rotation[1], el.rotation[2]);

        GameObject go = Instantiate(prefab, pos, rot, parent);

        // Disable colliders to prevent premature collision before data is applied
        SetCollidersEnabled(go, false);

        if (el.scale != null && el.scale.Length >= 3)
            go.transform.localScale = new Vector3(el.scale[0], el.scale[1], el.scale[2]);

        ApplyComponents(go, el.components);

        if (el.splinePoints != null && el.splinePoints.Length > 0)
            ApplySplinePoints(go, el.splinePoints);

        // Re-enable colliders after all data is applied
        SetCollidersEnabled(go, true);

        return go;
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

    private static void ApplySplinePoints(GameObject go, SplinePointData[] points)
    {
        SpriteShapeController oldSsc = go.GetComponent<SpriteShapeController>();
        if (oldSsc == null) return;

        // Save settings from the prefab's SSC
        SpriteShape profile = oldSsc.spriteShape;
        int splineDetail = oldSsc.splineDetail;
        int colliderDetail = oldSsc.colliderDetail;
        bool autoUpdate = oldSsc.autoUpdateCollider;
        float colliderOffset = oldSsc.colliderOffset;

        // Destroy old SSC so its baked mesh is gone
        DestroyImmediate(oldSsc);

        // Add fresh SSC — it will bake mesh from our spline on first LateUpdate
        SpriteShapeController ssc = go.AddComponent<SpriteShapeController>();
        ssc.spriteShape = profile;
        ssc.splineDetail = splineDetail;
        ssc.colliderDetail = colliderDetail;
        ssc.autoUpdateCollider = autoUpdate;
        ssc.colliderOffset = colliderOffset;

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
    }

    private void ClearContainer(Transform container)
    {
        if (container == null) return;
        for (int i = container.childCount - 1; i >= 0; i--)
            Destroy(container.GetChild(i).gameObject);
    }

    private int GetLayerIndex(LayerMask mask)
    {
        int value = mask.value;
        for (int i = 0; i < 32; i++)
        {
            if ((value & (1 << i)) != 0)
                return i;
        }
        return 0;
    }

    private void SetLayerRecursive(GameObject go, int layer)
    {
        go.layer = layer;
        foreach (Transform child in go.transform)
            SetLayerRecursive(child.gameObject, layer);
    }

    private static void SetCollidersEnabled(GameObject go, bool enabled)
    {
        Collider2D[] colliders = go.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
            col.enabled = enabled;
    }
}
