using Pooling;
using UnityEngine;

public abstract class ObstacleBase : MonoBehaviour
{
    protected string _word;

    protected int _remainingWords;

    public abstract void Activate(float distance);

    public virtual void OnWordCompleted()
    {
        _remainingWords--;

        if (_remainingWords <= 0)
        {
            Deactivate();
        }
        else
        {
            RequestNextWord();
        }
    }

    protected virtual void RequestNextWord()
    {
        TypingController.Source.SetWord(_word);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
        ObjectPoolManager.Source.Return(gameObject);
    }
}
