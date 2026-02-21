using UnityEngine;

[CreateAssetMenu(fileName = "WorldDefinition", menuName = "Word Paver/Level/World Definition")]
public class WorldDefinition : ScriptableObject
{
    public string Name;
    public float StartDistance;
    public GameObject BlockPrefab;

    [Header("Environment")]
    public WorldTileDefinition Environment;

    [Header("Difficulty")]
    public float DifficultyMultiplier = 1f;
}
