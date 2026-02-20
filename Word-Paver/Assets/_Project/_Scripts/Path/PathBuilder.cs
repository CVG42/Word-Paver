using System;
using System.Collections.Generic;
using DG.Tweening;
using Pooling;
using UnityEngine;

public class PathBuilder : MonoBehaviour
{
    [SerializeField] private GameObject _pathBlockPrefab;
    [SerializeField] private float _blockLength = 5f;
    [SerializeField] private int _maxBlocks = 10;
    [SerializeField] private int _prewarmBlocks = 10;

    private Queue<GameObject> _activeBlocks = new();

    private float _currentZ;
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
        
        var visual = block.GetComponent<PathBlockVisual>();

        Vector3 baseScale = visual.GetBaseScale();

        block.transform.localScale = Vector3.zero;

        AnimateSpawn(block.transform, baseScale);

        _activeBlocks.Enqueue(block);

        _currentZ += _blockLength;

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

    private void AnimateSpawn(Transform block, Vector3 baseScale)
    {
        block.DOScale(baseScale * 1.15f, 0.22f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                block.DOScale(baseScale, 0.08f);
            });
    }
}
