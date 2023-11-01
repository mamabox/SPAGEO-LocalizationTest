using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

/*******************************
 *  LocalizedText.cs
 *  
 * Added to TMP_Pro gameobject to localize text - uses LocalizationManager.cs
 *  
 *******************************/

public class LocalizedText : MonoBehaviour
{
    /*******************************
     *  VARIABLES
     *******************************/

    [SerializeField] private string _localizationKey;
    TextMeshProUGUI _textComponent;

    /*******************************
     * AWAKE, START, UPDATE
     *******************************/

    IEnumerator Start()
    {
        while (!Singleton.Instance.localizationMngr.LocalizationIsReady())
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
            //_textComponent.text = LocalizationManager.Instance.GetTextForKey(_localizationKey);
            _textComponent.text = Singleton. Instance.localizationMngr.GetTextForKey(_localizationKey);
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
        foreach (KeyValuePair<string, string> entry in Singleton.Instance.localizationMngr.ReturnDictionary())
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
