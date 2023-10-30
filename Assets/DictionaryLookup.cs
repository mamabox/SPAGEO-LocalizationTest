using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;

public class DictionaryLookup : MonoBehaviour
{
    private Dictionary<string, string> _shortHandDictionnary;
    private string myTestString = "blah blah {key1} but also {key2} |and {key3}";
    TextMeshProUGUI _textComponent;

    private void Awake()
    {
        _shortHandDictionnary = new Dictionary<string, string>()
        {
            { "{key1}", "key1 replacement"},
            { "{key2}", "key2 replacement"},
            { "{key3}", "key3 replacement"}
        };

        _textComponent = gameObject.GetComponent<TextMeshProUGUI>();

    }
    void Start()
    {
        string outputString;

        //_textComponent.text = myTestString;
        outputString = ParseShortHand(myTestString);

        //Debug.Log(ParseShortHand("test line 1|test line 2"));
        //Debug.Log(ParseShortHand(outputString));
        _textComponent.text = outputString;

    }

    private string ParseShortHand (string stringToParse)
    {
        // 1. Parse dictionary keys
        foreach(KeyValuePair<string, string> entry in _shortHandDictionnary)
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

    private string ParseNewLine (string stringToParse)
    {
        
        return stringToParse;
    }
}
