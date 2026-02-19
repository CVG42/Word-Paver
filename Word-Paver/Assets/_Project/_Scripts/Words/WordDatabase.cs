using System.Collections.Generic;
using UnityEngine;

public class WordDatabase : MonoBehaviour
{
    [SerializeField] private TextAsset _csvFile;

    private Dictionary<int, List<string>> _wordsByDifficulty = new();

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        string[] lines = _csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] columns = lines[i].Split(',');

            string Word = columns[0].Trim();
            int Difficulty = int.Parse(columns[1]);

            if (!_wordsByDifficulty.ContainsKey(Difficulty))
            {
                _wordsByDifficulty[Difficulty] = new List<string>();
            }

            _wordsByDifficulty[Difficulty].Add(Word);
        }
    }

    public string GetRandomWord(int difficulty)
    {
        if (!_wordsByDifficulty.ContainsKey(difficulty))
        {
            difficulty = 1;
        }

        var list = _wordsByDifficulty[difficulty];
        return list[Random.Range(0, list.Count)];
    }
}
