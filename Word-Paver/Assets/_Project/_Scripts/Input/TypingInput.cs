using UnityEngine;

public class TypingInput : MonoBehaviour
{
    [SerializeField] private TypingController _typing;

    private void Update()
    {
        foreach (char c in Input.inputString)
        {
            _typing.ProcessInput(c);
        }
    }
}
