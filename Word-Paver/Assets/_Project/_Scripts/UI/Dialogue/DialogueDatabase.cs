using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDatabase", menuName = "Word Paver/Dialogue/DialogueDatabase")]
public class DialogueDatabase : ScriptableObject
{
    public List<DialogueCategoryEntry> Dialogs;

    private Dictionary<DialogueCategory, List<DialogueSequence>> _cache;

    public void Initialize()
    {
        if (_cache != null) return;

        _cache = new Dictionary<DialogueCategory, List<DialogueSequence>>();

        foreach (var entry in Dialogs)
        {
            if (!_cache.ContainsKey(entry.Category))
            {
                _cache[entry.Category] = new List<DialogueSequence>();
            }

            _cache[entry.Category].AddRange(entry.Sequences);
        }
    }

    public DialogueSequence GetRandom(DialogueCategory category)
    {
        Initialize();

        if (!_cache.TryGetValue(category, out var list) || list.Count == 0)
        {
            Debug.LogWarning($"No dialogs for category: {category}");
            return null;
        }

        return list[Random.Range(0, list.Count)];
    }

    public DialogueSequence GetFirst(DialogueCategory category)
    {
        Initialize();

        if (!_cache.TryGetValue(category, out var list) || list.Count == 0)
        {
            return null;
        }

        return list[0];
    }
}
