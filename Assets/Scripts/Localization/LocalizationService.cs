using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LocalizationService : MonoBehaviour
{
    // Important Strings
    private const string FILE_EXTENSION = ".json";
    private string _languageFilenamePrefix = "lang-";   // TODO: Read from gameSetting.json
    private string _gamepadFilenamePrefix = "gamepad-lang-"; // TODO: Read from gameSetting.json
    private string _langTextFileFullName;
    private string _gamepadTextFileFullName;

    private ApplicationLanguage _applicationLanguage = ApplicationLanguage.FR;

    // Important Booleans
    private bool _localizationIsReady = false;
    private bool _isFileFound = false;
    private bool _isTryChangeLangRuntime = false;

    // JSON variables
    [SerializeField] private Dictionary<string, string> _languageDictionary;
    [SerializeField] private Dictionary<string, string> _shorthandDictionary;
    private LocalizationData _localizationData;


     private void Start()
    {
        //Debug.Log("enum = " + _applicationLanguage);
        //Debug.Log("(int)enum = " + (int)_applicationLanguage);
        //Debug.Log("nameof(enum) = " + nameof(_applicationLanguage));
        //Debug.Log("enum.String() = " + _applicationLanguage.ToString());

        _applicationLanguage = LocaleHelper.GetSupportedLanguageCodeENUM();
        _langTextFileFullName = _languageFilenamePrefix + _applicationLanguage.ToString().ToLower();
        _gamepadTextFileFullName = _gamepadFilenamePrefix + _applicationLanguage.ToString().ToLower();

        _languageDictionary = Singleton.Instance.dictionaryImportManager.ImportDictionaryFromJson(_langTextFileFullName);
        _shorthandDictionary = Singleton.Instance.dictionaryImportManager.ImportDictionaryFromJson(_gamepadTextFileFullName);
    }

    // PUBLIC FUNCTIONS
    public ApplicationLanguage GetApplicationLang()
    {
        return _applicationLanguage; //use (int)GetApplicationLang() or nameof(_applicationLang) or _applicationLanguage.ToString();
    }

}
