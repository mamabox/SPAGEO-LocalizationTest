using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*******************************
 *  LocalizedTextBehaviour.cs
 *  
 * Added to TMP_Pro gameobject to localize text - uses LocalizatioService.cs
 *  
 *******************************/

public class LocalizedTextBehaviour : MonoBehaviour
{
    /*******************************
     *  VARIABLES
     *******************************/

    [SerializeField] private string _localizationKey;
    [SerializeField] TextMeshProUGUI _textComponent;

    /*******************************
     * AWAKE, START, UPDATE
     *******************************/

    IEnumerator Start()
    {
        while (!Singleton.Instance.localizationService.LocalizationIsReady())
        {
            yield return null;
        }
        SetText();
    }

    [ContextMenu("Set Text")]
    public void SetText()
    {
        if (_textComponent == null)
        {
            _textComponent = gameObject.GetComponent<TextMeshProUGUI>();
        }
        try
        {
           _textComponent.text = Singleton.Instance.localizationService.GetTextForLanguageKey(_localizationKey);
           _textComponent.text = ParseShortHand(_textComponent.text);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private string ParseShortHand(string stringToParse)
    {
        var _dictionary = Singleton.Instance.localizationService.GetShorthandDictionary();

        // 1. Parse shorthand keys
        foreach (KeyValuePair<string, string> entry in _dictionary)
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
