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

public class TextBehaviour : MonoBehaviour
{
    /*******************************
     *  VARIABLES
     *******************************/

    TextMeshProUGUI _textComponent;

    /*******************************
     * AWAKE, START, UPDATE
     *******************************/
    void Awake()
    {
        _textComponent = gameObject.GetComponent<TextMeshProUGUI>();
    }

    IEnumerator Start()
    {
        while (!Singleton.Instance.localizationService.LocalizationIsReady())
        {
            yield return null;
        }
        ParseShortHand();
    }


    public void ParseShortHand()
    {
        var _dictionary = Singleton.Instance.localizationService.GetShorthandDictionary();
        string stringToParse = _textComponent.text;

        // 1. Parse shorthand keys
        foreach (KeyValuePair<string, string> entry in _dictionary)
        {
            if (stringToParse.Contains(entry.Key))
            {
                stringToParse = stringToParse.Replace(entry.Key, entry.Value);
            }
        }

        // 2. Parse NewLine
        _textComponent.text = stringToParse.Replace("|", System.Environment.NewLine);
    }
}
