using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelJson;
    public PrefabEntry[] prefabRegistry;

    public Transform levelContainer;
    public Transform groundContainer;
    public Transform platformsContainer;
    public Transform obstaclesContainer;

    public LayerMask groundLayer;

    private Dictionary<string, GameObject> prefabMap;

    private static readonly HashSet<string> GroundPrefabs = new HashSet<string> { "Square", "Platform" };

    private void Start()
    {
        BuildPrefabMap();
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
        VersionCheck vc = JsonUtility.FromJson<VersionCheck>(json);

        if (vc != null && vc.version == 2)
            LoadV2(json);
        else
            LoadV1(json);
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

        if (data.elements != null)
        {
            foreach (LevelElement el in data.elements)
            {
                GameObject go = SpawnElement(el, levelContainer);
                if (go != null && GroundPrefabs.Contains(el.prefab))
                {
                    go.tag = "Ground";
                    SetLayerRecursive(go, groundLayerIndex);
                }
            }
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

        if (el.scale != null && el.scale.Length >= 3)
            go.transform.localScale = new Vector3(el.scale[0], el.scale[1], el.scale[2]);

        return go;
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
}
