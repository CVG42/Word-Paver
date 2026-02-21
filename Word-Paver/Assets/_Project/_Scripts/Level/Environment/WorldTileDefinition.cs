using UnityEngine;

[CreateAssetMenu(fileName = "WorldTileDefinition", menuName = "Word Paver/Level/Tile Definition")]
public class WorldTileDefinition : ScriptableObject
{
    public EnvironmentTile[] TileVariants;
}
