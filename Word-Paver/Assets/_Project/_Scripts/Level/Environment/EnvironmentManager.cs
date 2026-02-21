using System.Collections.Generic;
using Pooling;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _spawnDistance = 200f;
    [SerializeField] private float _recycleOffset = 20f;
    [SerializeField] private int _prewarmCount = 10;
    [SerializeField] private float xOffset = 2f;

    private WorldTileDefinition _definition;

    private List<EnvironmentTile> _activeTiles = new();

    private float _nextSpawnZ;

    private void Start()
    {
        if (_definition == null) return;

        Prewarm();
        InitialSpawn();
    }

    private void Update()
    {
        if (_definition == null) return;

        HandleSpawning();
        HandleRecycling();
    }

    private void Prewarm()
    {
        for (int i = 0; i < _prewarmCount; i++)
        {
            var prefab = GetRandomVariant();
            var tile = ObjectPoolManager.Source.Borrow<EnvironmentTile>(prefab.gameObject);

            ObjectPoolManager.Source.Return(tile.gameObject);
        }
    }

    private EnvironmentTile GetRandomVariant()
    {
        int index = Random.Range(0, _definition.TileVariants.Length);
        return _definition.TileVariants[index];
    }

    private void InitialSpawn()
    {
        _nextSpawnZ = 0;

        while (_nextSpawnZ < _spawnDistance)
        {
            SpawnTile();
        }
    }

    private void SpawnTile()
    {
        var prefab = GetRandomVariant();
        var tile = ObjectPoolManager.Source.Borrow<EnvironmentTile>(prefab.gameObject);

        tile.transform.position = new Vector3(xOffset, 0, _nextSpawnZ);
        tile.transform.rotation = Quaternion.identity;

        tile.Activate();

        _activeTiles.Add(tile);

        _nextSpawnZ += tile.Length;
    }

    private void HandleSpawning()
    {
        float cameraZ = _camera.position.z;

        while (cameraZ + _spawnDistance > _nextSpawnZ)
        {
            SpawnTile();
        }
    }

    private void HandleRecycling()
    {
        float cameraZ = _camera.position.z;

        for (int i = _activeTiles.Count - 1; i >= 0; i--)
        {
            var tile = _activeTiles[i];

            if (tile.transform.position.z + tile.Length < cameraZ - _recycleOffset)
            {
                RecycleTile(tile);
            }
        }
    }

    private void RecycleTile(EnvironmentTile tile)
    {
        tile.Deactivate();

        _activeTiles.Remove(tile);

        ObjectPoolManager.Source.Return(tile.gameObject);
    }

    public void SetWorld(WorldTileDefinition definition)
    {
        _definition = definition;

        ResetEnvironment();
    }

    public void ResetEnvironment()
    {
        RecycleAllTiles();

        _nextSpawnZ = 0;

        Prewarm();
        InitialSpawn();
    }

    private void RecycleAllTiles()
    {
        foreach (var tile in _activeTiles)
        {
            tile.Deactivate();
            ObjectPoolManager.Source.Return(tile.gameObject);
        }

        _activeTiles.Clear();
    }
}
