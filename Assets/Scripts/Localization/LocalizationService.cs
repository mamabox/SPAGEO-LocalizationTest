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

    [SerializeField] private ApplicationLanguage _applicationLanguage = ApplicationLanguage.FR;

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
        //_applicationLanguage = LocaleHelper.GetSupportedSystemLanguage();
        yield return StartCoroutine(FetchDictionaries());
 
        _isLocalizationReady = true;
    }

    /*******************************
     * MAIN
     *******************************/
    IEnumerator FetchDictionaries()
    {
        _langTextFileFullName = _languageFilenamePrefix + _applicationLanguage.ToString().ToLower();
        _gamepadTextFileFullName = _gamepadFilenamePrefix + _applicationLanguage.ToString().ToLower();

        // Fetch language dictionary
        yield return Singleton.Instance.dictionaryImportManager.ImportDictionaryFromJson(_langTextFileFullName);
        _languageDictionary = Singleton.Instance.dictionaryImportManager.GetDictionary();

        // Fetch shorthand dictionary
        yield return Singleton.Instance.dictionaryImportManager.ImportDictionaryFromJson(_gamepadTextFileFullName);
        _shorthandDictionary = Singleton.Instance.dictionaryImportManager.GetDictionary();
    }

    IEnumerator SwitchLanguageAtRuntime(ApplicationLanguage lang)
    {
        if (!_isTryChangeLangRuntime)
        {
            _isTryChangeLangRuntime = true;
            _isLocalizationReady = false;
            _applicationLanguage = lang;
        }

        yield return StartCoroutine(FetchDictionaries());
        _isLocalizationReady = true;
        UpdateLocalizedText();
        UpdateLocalizedAssets();
        _isTryChangeLangRuntime = false;
    }
    // TODO: Replace by event handling
    private void UpdateLocalizedText()
    {
        LocalizedTextBehaviour[] arrayText = FindObjectsOfType<LocalizedTextBehaviour>();

        for (int i = 0; i < arrayText.Length; i++)
        {
            arrayText[i].SetText();
        }
    }

    // TODO: Replace by event handling
    private void UpdateLocalizedAssets()
    {
        LocalizedAssetBehaviour[] arrayText = FindObjectsOfType<LocalizedAssetBehaviour>();

        for (int i = 0; i < arrayText.Length; i++)
        {
            arrayText[i].SetAsset();
        }
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
            //Debug.Log("key is : " + _languageDictionary[key]);
            return _languageDictionary[key];
        }
        else
        {
            Debug.Log("Error: Key [" + key + "] not found in "+ _langTextFileFullName);
            return "Error: Key [" + key + "] not found in.";
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

    public void ChangeLanguage(ApplicationLanguage lang)
    {
        Debug.Log("ChangeLanguage() to " + lang);
        StartCoroutine(SwitchLanguageAtRuntime(lang));
    }
}
