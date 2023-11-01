using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.VisualScripting;

/*******************************
 *  LocalizationService.cs
 *  
 *  @start, configures localization based on SystemLangauge
 *  Handles localization changes during runtime
 *  
 *  . - Variables
 *  . - Awake, Start, Update
 *  x - Public Fonctions
 *  
 *******************************/

public class LocalizationService : MonoBehaviour
{
    /*******************************
     *  VARIABLES
     *******************************/

    // Important Strings
    private const string FILE_EXTENSION = ".json";
    private string _languageFilenamePrefix = "old-lang-";   // TODO: Read from gameSetting.json
    private string _gamepadFilenamePrefix = "old-gamepad-lang-"; // TODO: Read from gameSetting.json
    private string _langTextFileFullName;
    private string _gamepadTextFileFullName;

    private ApplicationLanguage _applicationLanguage = ApplicationLanguage.FR;

    // Important Booleans
    private bool _isLocalizationReady = false;
    private bool _isFileFound = false;
    private bool _isTryChangeLangRuntime = false;

    // JSON variables
    [SerializeField] private Dictionary<string, string> _languageDictionary;
    [SerializeField] private Dictionary<string, string> _shorthandDictionary;

    private DictionaryData _dic1;
    private DictionaryData _dic2;

    /*******************************
     * AWAKE, START, UPDATE
     *******************************/
    IEnumerator Start()
    {
        //Debug.Log("enum = " + _applicationLanguage);
        //Debug.Log("(int)enum = " + (int)_applicationLanguage);
        //Debug.Log("nameof(enum) = " + nameof(_applicationLanguage));
        //Debug.Log("enum.String() = " + _applicationLanguage.ToString());

        //_applicationLanguage = LocaleHelper.GetSupportedSystemLanguage();
        _langTextFileFullName = _languageFilenamePrefix + _applicationLanguage.ToString().ToLower();
        _gamepadTextFileFullName = _gamepadFilenamePrefix + _applicationLanguage.ToString().ToLower();

        yield return Singleton.Instance.dictionaryImportManager.ImportDictionaryFromJson(_langTextFileFullName);
        _languageDictionary = Singleton.Instance.dictionaryImportManager.GetDictionary();

        yield return Singleton.Instance.dictionaryImportManager.ImportDictionaryFromJson(_gamepadTextFileFullName);
        _shorthandDictionary = Singleton.Instance.dictionaryImportManager.GetDictionary(); ;
 
        _isLocalizationReady = true;
    }

    /*******************************
     * PUBLIC FUNCTIONS
     *******************************/
    public bool LocalizationIsReady()
    {
        return _isLocalizationReady;
    }

    public string GetTextForLanguageKey(string key)
    {
        if (_languageDictionary.ContainsKey(key))
        {
            Debug.Log("key is : " + _languageDictionary[key]);
            return _languageDictionary[key];
        }
        else
        {
            Debug.Log("Error: Key [" + key + "] not found.");
            return "Error: Key [" + key + "] not found.";
        }
    }

    public Dictionary<string, string> GetLangDictionary()
    {
        return _languageDictionary;
    }

    public Dictionary<string, string> GetShorthandDictionary()
    {
        return _shorthandDictionary;
    }

    public ApplicationLanguage GetApplicationLang()
    {
        return _applicationLanguage; //use (int)GetApplicationLang() or nameof(_applicationLang) or _applicationLanguage.ToString();
    }
}
