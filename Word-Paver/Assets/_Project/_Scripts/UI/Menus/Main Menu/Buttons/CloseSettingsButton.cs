using UnityEngine;

public class CloseSettingsButton : MonoBehaviour
{
    [SerializeField] private MenuTypingController _typing;

    public void Close()
    {
        gameObject.SetActive(false);
        _typing.ResetAll();
    }
}
