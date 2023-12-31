using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/*******************************
 *  LocalizationManager.cs
 *  
 * Follows tutorial from https://www.youtube.com/watch?v=vadsss0NJcw&list=PLTv8RdpDL35oGLPdApR7Q5tub7UY34_lD&pp=iAQB
 *  
 *  . - Variables
 *  . - Awake, Start, Update
 *  x - From tutorial
 *  x - Public Fonctions
 *  
 *******************************/

public class LocalizationManager : MonoBehaviour
{
    /*******************************
     *  VARIABLES
     *******************************/

    [Header("Important Strings")]
    private const string FILE_EXTENSION = ".json";
    private string FILENAME_PREFIX = "old-lang-";
    //private const string DEFAULT_FILENAME_PREFIX_gamepad = "old-gamepad-lang-";
    private string FULL_NAME_TEXT_FILE;
    private string FULL_NAME_TEXT_FILE_gamepad;
    private string URL = "";
    private string FULL_PATH_TEXT_FILE;
    private string FULL_PATH_TEXT_FILE_gamepad;
    private string LANGUAGE_CHOICE = "FR";
    private string LOADED_JSON_TEXT = "";
    private ApplicationLanguage APPLICATION_LANG = ApplicationLanguage.FR;
    

    [Header ("Important bool")]
    private bool _isReady = false;
    private bool _isFileFound = false;
    private bool _isTryChnageLangRuntime = false;

    [Header ("JSON variables")]
    private Dictionary<string, string> _localizedDictionary;
    private LocalizationData _loadedData;
    private LocalizationData _loadedDataTest;
 


    /*******************************
     * AWAKE, START, UPDATE
     *******************************/

    IEnumerator Start()
    {
        //LANGUAGE_CHOICE = LocaleHelper.GetSupportedLanguageCode();
        SetApplicationLanguage(LANGUAGE_CHOICE);
        FULL_NAME_TEXT_FILE = FILENAME_PREFIX + LANGUAGE_CHOICE.ToLower() + FILE_EXTENSION;

#if UNITY_IOS || UNITY_ANDROID
            FULL_PATH_TEXT_FILE = Path.Combine(Application.persistentDataPath, FULL_NAME_TEXT_FILE);
#else
        FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
#endif
        //Debug.Log("FULL PATH TEXT FILE: " + FULL_PATH_TEXT_FILE);
        yield return StartCoroutine(LoadJsonLanguageData());
        //_isReady = true;

        yield return StartCoroutine(Start2());
    }



    /*******************************
     * EDITED OR ADDED
     *******************************/

    IEnumerator Start2()
    {

        /** NEW */
        //Debug.Log("In Start2");
        FILENAME_PREFIX = "old-gamepad-lang-";
        FULL_NAME_TEXT_FILE = FILENAME_PREFIX + LANGUAGE_CHOICE.ToLower() + FILE_EXTENSION;

#if UNITY_IOS || UNITY_ANDROID
            FULL_PATH_TEXT_FILE = Path.Combine(Application.persistentDataPath, FULL_NAME_TEXT_FILE);
#else
        FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
#endif
        //Debug.Log("FULL PATH TEXT FILE: " + FULL_PATH_TEXT_FILE);
        yield return StartCoroutine(LoadJsonLanguageDataNEW());
        _isReady = true;
        FILENAME_PREFIX = "old-lang-";
    }

    IEnumerator LoadJsonLanguageDataNEW()
    {
        CheckIfFileExists();
        // if (File.Exists(dataFile))

        yield return new WaitUntil(() => _isFileFound);

        _loadedData = JsonUtility.FromJson<LocalizationData>(LOADED_JSON_TEXT);
        _loadedData.items.ForEach(item =>
        {
            try
            {
                _localizedDictionary.Add(item.key, item.value);
                //_dictionaryFR.languageItems.Add(new LocalizationItems (item.key, item.value ));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    private void SetApplicationLanguage(string lang)
    {
        switch (lang)
        {
            case ("fr"):
                APPLICATION_LANG = ApplicationLanguage.FR;
                break;
            case ("en"):
                APPLICATION_LANG = ApplicationLanguage.EN;
                break;
            default:
                APPLICATION_LANG = ApplicationLanguage.FR;
                break;
        }
    }

    /*******************************
     * FROM TUTORIAL 
     *******************************/


    IEnumerator LoadJsonLanguageData()
    {
        CheckIfFileExists();
        // if (File.Exists(dataFile))

        yield return new WaitUntil(() => _isFileFound);

        _loadedData = JsonUtility.FromJson<LocalizationData>(LOADED_JSON_TEXT);
        _localizedDictionary = new Dictionary<string, string>(_loadedData.items.Count);
        _loadedData.items.ForEach(item =>
        {
            try
            {
                _localizedDictionary.Add(item.key, item.value);
                //_dictionaryFR.languageItems.Add(new LocalizationItems (item.key, item.value ));
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    private void CheckIfFileExists()
    {
        if (!File.Exists(FULL_PATH_TEXT_FILE))
        {
            GetUrlFileText();
            StartCoroutine(CopyFileFromWeb(URL));
        }
        else
        {
            //Debug.Log("File already exists.");
            LoadFileContents();
            _isFileFound = true;
        }
    }

    private void GetUrlFileText()
    {
        switch (LANGUAGE_CHOICE)
        {
            case ApplicationLocale.FR:
                URL = ""; //needs to be .txt file
                break;
            case ApplicationLocale.EN:
                URL = "";
                break;
            default:
                URL = "";
                break;
        }
    }

    public bool LocalizationIsReady()
    {
        return _isReady;
    }

    IEnumerator CopyFileFromWeb(string urlRetrieveText)
    {
        UnityWebRequest www = UnityWebRequest.Get(urlRetrieveText);
        yield return www.SendWebRequest();

        if (www.result == (UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError))
        //if (www.isNetworkError || www.isHttpError) //OBSOLETE
            {
            Debug.LogError("We cannot access the file, make sure you have an internet connection or a link to a .txt file.");
            Debug.LogWarning("We will try to copy files from the ressources folder.");
            CopyFileFromRessources();
            yield break;
        }

        LOADED_JSON_TEXT = www.downloadHandler.text;
        //Debug.Log("Copying file to LOADED_JSON_TEXT");
        File.WriteAllText(FULL_PATH_TEXT_FILE, LOADED_JSON_TEXT);
        //Debug.Log("Writing file to StreamingAsset from Web");
        StartCoroutine(WaitForFileCreation());
    }

    private void LoadFileContents()
    {
        LOADED_JSON_TEXT = File.ReadAllText(FULL_PATH_TEXT_FILE);
        _isFileFound = true;
        //Debug.Log(LOADED_JSON_TEXT);
        

    }

    private void CopyFileFromRessources()
    {
        TextAsset myFile = Resources.Load(FILENAME_PREFIX + LANGUAGE_CHOICE) as TextAsset;
        if (myFile == null)
        {
            Debug.LogError("Makse sure file " + FILENAME_PREFIX + LANGUAGE_CHOICE + FILE_EXTENSION + " is in ressource folder");
            return;
        }
        LOADED_JSON_TEXT = myFile.ToString();
        File.WriteAllText(FULL_PATH_TEXT_FILE, LOADED_JSON_TEXT);
        //Debug.Log("Writing file to StreamingAsset from Ressources folder");
        StartCoroutine(WaitForFileCreation());

    }

    IEnumerator WaitForFileCreation()
    {
        FileInfo myFile = new FileInfo(FULL_NAME_TEXT_FILE);
        float timeout = 0.0f; //to avoid being stuck in infinite loop

        while (timeout < 5.0f && !IsFileCreated(myFile)){
            timeout += Time.deltaTime;
            yield return null;
        }
        //Debug.Log("File succesfully created.");
    }

    //if it can open the file, returns true
    private bool IsFileCreated(FileInfo file)
    {
        FileStream stream = null;
        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException)
        {
            _isFileFound = true;
            //Debug.Log("File found.");
            return true;
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }

        //File was not found
        _isFileFound = false;
        return false;
    }



    IEnumerator SwitchLanguageRuntime(string langChoice)
    {
        if (!_isTryChnageLangRuntime)
        {
            _isTryChnageLangRuntime = true;
            _isFileFound = false;
            _isReady = false;
            LANGUAGE_CHOICE = langChoice;
            //SetApplicationLanguage(langChoice);

            FULL_NAME_TEXT_FILE = FILENAME_PREFIX + LANGUAGE_CHOICE.ToLower() + FILE_EXTENSION;

            #if UNITY_IOS || UNITY_ANDROID
                FULL_PATH_TEXT_FILE = Path.Combine(Application.persistentDataPath, FULL_NAME_TEXT_FILE);
            #else
                FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
            #endif
            yield return StartCoroutine(LoadJsonLanguageData());
            yield return StartCoroutine(Start2());
            _isReady = true;

            //Update LocalizedText
            LocalizedText[] arrayText = FindObjectsOfType<LocalizedText>();
            
            for (int i = 0; i < arrayText.Length; i++)
            {
                arrayText[i].AttributionText();
            }

            //Update LocalizedAsset
            //Update LocalizedText
            LocalizedAsset[] arrayAssets = FindObjectsOfType<LocalizedAsset>();

            for (int i = 0; i < arrayAssets.Length; i++)
            {
                arrayAssets[i].AttributeAsset();
            }

            _isTryChnageLangRuntime = false;
        }
    }

    public void ChangeLanguage(string lang)
    {
        StartCoroutine(SwitchLanguageRuntime(lang));
    }



    /*******************************
     * PUBLIC FUNCTIONS (ADDED)
     *******************************/
    public string GetTextForKey(string localizationKey)
    {
        if (_localizedDictionary.ContainsKey(localizationKey))
        {
            return _localizedDictionary[localizationKey];
        }
        else
        {
            return "Error, no key matching with " + localizationKey;
        }
    }

    public ApplicationLanguage ReturnApplicationLang()
    {
        return APPLICATION_LANG;
    }

    public string GetLanguageChoice()
    {
        return LANGUAGE_CHOICE;
    }

    public int GetLanguageChoiceID()
    {
        switch (LANGUAGE_CHOICE)
        {
            case "fr":
                return 0;

            case "en":
                return 1;

            default:
                return 0;

        }
    }
    public Dictionary<string, string> ReturnDictionary()
    {
        return _localizedDictionary;
    }
}