using System.Collections.Generic;
using Pooling;
using UnityEngine;

public class ObstacleManager : Singleton<IObstacleSource>, IObstacleSource
{
    [Header("Obstacle Pool")]
    [SerializeField] private List<GameObject> _obstaclePrefabs;

    [Header("Rules")]
    [SerializeField] private float _startDistance = 200f;
    [SerializeField] private float _spawnChance = 0.35f;
    [SerializeField] private float _minDistanceBetweenSpawns = 25f;

    private float _lastSpawnDistance;
    private ObstacleBase _activeObstacle;

    public bool HasActiveObstacle => _activeObstacle != null;

    public bool TrySpawnObstacle(float distance)
    {
        if (GameManager.Source.CurrentGameState != GameState.OnPlay) return false;

        if (distance < _startDistance) return false;
        if (_activeObstacle != null) return true;

        if (distance - _lastSpawnDistance < _minDistanceBetweenSpawns) return false;

        if (Random.value > _spawnChance) return false;

        SpawnObstacle(distance);
        return true;
    }

    private void SpawnObstacle(float distance)
    {
        GameObject prefab = SelectObstaclePrefab();

        GameObject obstacleGO = ObjectPoolManager.Source.Borrow(prefab);

        obstacleGO.transform.position = new Vector3(0, .35f, GameManager.Source.DistanceTravelled + 2);
        obstacleGO.transform.rotation = Quaternion.identity;

        _activeObstacle = obstacleGO.GetComponent<ObstacleBase>();

        _lastSpawnDistance = distance;

        _activeObstacle.Activate(distance);
    }

    private GameObject SelectObstaclePrefab()
    {
        int index = Random.Range(0, _obstaclePrefabs.Count);
        return _obstaclePrefabs[index];
    }

    public void NotifyWordCompleted()
    {
        if (_activeObstacle == null) return;

        _activeObstacle.OnWordCompleted();

        if (!_activeObstacle.gameObject.activeSelf)
        {
            _activeObstacle = null;
        }
    }

    public void ResetObstacles()
    {
        if (_activeObstacle != null)
        {
            ObjectPoolManager.Source.Return(_activeObstacle.gameObject);
            _activeObstacle = null;
        }

        _lastSpawnDistance = 0;
    }
}

public interface IObstacleSource
{
    bool HasActiveObstacle { get; }
    bool TrySpawnObstacle(float distance);
    void NotifyWordCompleted();
    void ResetObstacles();
}
