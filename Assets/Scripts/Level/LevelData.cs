using System;
using UnityEngine;

[Serializable]
public class ComponentProperty
{
    public string name;
    public float value;
}

[Serializable]
public class ComponentData
{
    public string type;
    public ComponentProperty[] properties;
}

[Serializable]
public class LevelElement
{
    public string prefab;
    public float[] position;
    public float[] scale;
    public float[] rotation;
    public ComponentData[] components;
}

[Serializable]
public class PrefabEntry
{
    public string name;
    public GameObject prefab;
}

[Serializable]
public class LevelDataV1
{
    public string name;
    public LevelElement[] groundSegments;
    public LevelElement[] platforms;
    public LevelElement[] obstacles;
    public LevelElement[] zones;
}

[Serializable]
public class LevelDataV2
{
    public int version = 2;
    public string name;
    public float[] playerPosition;
    public LevelElement[] elements;
}

[Serializable]
public class VersionCheck
{
    public int version;
}
