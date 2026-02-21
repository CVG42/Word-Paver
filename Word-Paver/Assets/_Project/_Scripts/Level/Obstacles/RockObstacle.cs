using Pooling;
using UnityEngine;

public class RockObstacle : ObstacleBase
{
    [SerializeField] private int _wordsToDestroy = 2;

    public override void Activate(float distance)
    {
        gameObject.SetActive(true);

        _remainingWords = _wordsToDestroy;

        _word = WordManager.Source.GetWordByDifficulty(2);

        Debug.Log($"Rock word: {_word}");

        TypingController.Source.SetWord(_word);
    }
}
