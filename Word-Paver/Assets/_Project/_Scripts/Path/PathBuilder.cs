using System;
using System.Collections.Generic;
using Pooling;
using UnityEngine;

public class PathBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _pathBlockPrefab;
    [SerializeField] private float _blockLength = 5f;
    [SerializeField] private int _maxBlocks = 10;
    [SerializeField] private int _prewarmBlocks = 10;

    public event Action<float> OnDistanceChanged;
    public float DistanceTravelled => _distanceTravelled;

    private Queue<GameObject> _activeBlocks = new();

    private float _currentZ;
    private float _distanceTravelled;

    private GameObject _currentPrefab;

    private void Start()
    {
        _currentPrefab = _pathBlockPrefab;

        PrewarmPrefab(_currentPrefab);
    }

    public void SpawnBlock()
    {
        GameObject block = ObjectPoolManager.Source.Borrow(_currentPrefab);

        block.transform.position = new Vector3(0, 0, _currentZ);
        block.transform.rotation = Quaternion.identity;

        _activeBlocks.Enqueue(block);

        _currentZ += _blockLength;
        _distanceTravelled += _blockLength;

        OnDistanceChanged?.Invoke(_distanceTravelled);

        HandleOverflow();
    }

    public void SetWorld(WorldDefinition world)
    {
        if (world == null || world.BlockPrefab == null) return;

        if (world.BlockPrefab == _currentPrefab) return;

        _currentPrefab = world.BlockPrefab;

        PrewarmPrefab(_currentPrefab);
    }

    private void PrewarmPrefab(GameObject prefab)
    {
        for (int i = 0; i < _prewarmBlocks; i++)
        {
            GameObject block = ObjectPoolManager.Source.Borrow(prefab);
            ObjectPoolManager.Source.Return(block);
        }
    }

    private void HandleOverflow()
    {
        if (_activeBlocks.Count <= _maxBlocks) return;

        GameObject oldestBlock = _activeBlocks.Dequeue();

        ObjectPoolManager.Source.Return(oldestBlock);
    }
}
