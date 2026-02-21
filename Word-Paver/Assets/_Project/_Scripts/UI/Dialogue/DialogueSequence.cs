using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSequence", menuName = "Word Paver/Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
    public List<string> Messages;
}
