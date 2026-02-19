using System;
using UnityEngine;

public class WorldManager : Singleton<IWorldSource>, IWorldSource
{
    [SerializeField] private WorldDefinition[] _worlds;

    public event Action<WorldDefinition> OnWorldChanged;
    public WorldDefinition CurrentWorld => _currentWorld;

    private WorldDefinition _currentWorld;

    protected override void Awake()
    {
        base.Awake();

        SortWorlds();
    }

    public WorldDefinition GetWorld(float distance)
    {
        if (_worlds == null || _worlds.Length == 0) return null;

        WorldDefinition selected = _worlds[0];

        for (int i = 0; i < _worlds.Length; i++)
        {
            if (distance >= _worlds[i].StartDistance)
            {
                selected = _worlds[i];
            }
            else
            {
                break;
            }
        }

        return selected;
    }

    public void EvaluateWorld(float distance)
    {
        WorldDefinition newWorld = GetWorld(distance);

        if (newWorld == _currentWorld) return;

        _currentWorld = newWorld;

        OnWorldChanged?.Invoke(newWorld);
    }

    private void SortWorlds()
    {
        if (_worlds == null) return;

        Array.Sort(_worlds, (a, b) => a.StartDistance.CompareTo(b.StartDistance));
    }
}

public interface IWorldSource
{
    event Action<WorldDefinition> OnWorldChanged;

    WorldDefinition CurrentWorld { get; }
    WorldDefinition GetWorld(float distance);

    void EvaluateWorld(float distance);
}
