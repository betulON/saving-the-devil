using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Story", menuName = "Story Text")]
public class StoryTextSO : ScriptableObject
{
    [TextArea(2, 6)]
    [SerializeField] string storyText;
    [SerializeField] string[] choices;

    public string GetStoryText()
    {
        return storyText;
    }

    public string GetChoiceText(int index)
    {
        return choices[index];
    }
}


