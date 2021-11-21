using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Story : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI narrativeText;
    [SerializeField] StoryTextSO storyText;
    [SerializeField] GameObject[] choiceButtons;

    // Start is called before the first frame update
    void Start()
    {
        narrativeText.text = storyText.GetStoryText();

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = storyText.GetChoiceText(i);
        }
    }
}
