using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LocalizedText : MonoBehaviour
{
    [SerializeField]
    private string _localizationKey;

    TextMeshProUGUI _textComponent;



IEnumerator Start()
    {
        while (!LocalizationManager.Instance.LocalizationIsReady())
        {
            yield return null;
        }
        AttributionText();
    }

    public void AttributionText()
    {
        if(_textComponent == null)
        {
            _textComponent = gameObject.GetComponent<TextMeshProUGUI>();
        }

        try
        {
            _textComponent.text = LocalizationManager.Instance.GetTextForKey(_localizationKey);
            _textComponent.text = ParseShortHand(_textComponent.text);
        }

        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private string ParseShortHand(string stringToParse)
    {
        // 1. Parse dictionary keys
        foreach (KeyValuePair<string, string> entry in LocalizationManager.Instance.ReturnDictionary())
        {
            if (stringToParse.Contains(entry.Key))
            {
                stringToParse = stringToParse.Replace(entry.Key, entry.Value);
            }
        }

        // 2. Parse NewLine
        stringToParse = stringToParse.Replace("|", System.Environment.NewLine);

        return stringToParse;
    }
}
