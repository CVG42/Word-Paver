using System;

[Serializable]
public class WordData
{
    public string Word;
    public int Difficulty;

    public WordData(string word, int difficulty)
    {
        Word = word;
        Difficulty = difficulty;
    }
}
