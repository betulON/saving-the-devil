using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Narrative", menuName = "Story Narrative")]
public class NarrativeSO : ScriptableObject
{
    [TextArea(2,6)]
    [SerializeField] string narrative;
    [SerializeField] string[] choices;

    public string GetNarrative()
    {
        return narrative;
    }
}

