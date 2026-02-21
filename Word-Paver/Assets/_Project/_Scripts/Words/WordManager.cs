using UnityEngine;

public class WordManager : Singleton<IWordSource>, IWordSource
{
    [SerializeField] private WordDatabase _database;

    public string GetWord(float distance)
    {
        int difficulty = CalculateDifficulty(distance);
        return _database.GetRandomWord(difficulty);
    }

    public string GetWordByDifficulty(int difficulty)
    {
        return _database.GetRandomWord(difficulty);
    }

    private int CalculateDifficulty(float distance)
    {
        if (distance < 50) return 1;
        if (distance < 150) return Random.Range(1, 3);
        return Random.Range(1, 4);
    }
}

public interface IWordSource
{
    string GetWord(float distance);
    string GetWordByDifficulty(int difficulty);
}
