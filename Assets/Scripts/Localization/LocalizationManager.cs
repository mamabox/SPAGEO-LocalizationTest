using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class LocalizationManager : MonoBehaviour
{
    [Header("Important Strings")]
    private const string FILE_EXTENSION = ".json";
    private const string FILENAME_PREFIX = "lang-";
    private string FULL_NAME_TEXT_FILE;
    private string URL = "";
    private string FULL_PATH_TEXT_FILE;
    private string LANGUAGE_CHOICE = "FR";
    private string LOADED_JSON_TEXT = "";
    private ApplicationLanguage _language = ApplicationLanguage.FR;
    

    [Header ("Important bool")]
    private bool _isReady = false;
    private bool _isFileFound = false;
    private bool _isTryChnageLangRuntime = false;

    [Header ("JSON variables")]
    private Dictionary<string, string> _localizedDictionary;
    private LocalizationData _loadedData;

    [SerializeField]
    LanguageFile _dictionaryFR;

    #region Instance Function
    private static LocalizationManager LocalizationManagerInstance;

    public static LocalizationManager Instance
    {
        get
        {
            if(LocalizationManagerInstance == null)
            {
                LocalizationManagerInstance = FindObjectOfType(typeof(LocalizationManager)) as LocalizationManager;
            }
            return LocalizationManagerInstance;
        }
    }
    #endregion Instance Function

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); //For scene loading
    }

    IEnumerator Start()
    {
        //LANGUAGE_CHOICE = LocaleHelper.GetSupportedLanguageCode();
        FULL_NAME_TEXT_FILE = FILENAME_PREFIX + LANGUAGE_CHOICE.ToLower() + FILE_EXTENSION;

        #if UNITY_IOS || UNITY_ANDROID
            FULL_PATH_TEXT_FILE = Path.Combine(Application.persistentDataPath, FULL_NAME_TEXT_FILE);
        #else 
            FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
        #endif
        yield return StartCoroutine(LoadJsonLanguageData());
       _isReady = true;

    }

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
            Debug.Log("File already exists.");
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
        Debug.Log("Copying file to LOADED_JSON_TEXT");
        File.WriteAllText(FULL_PATH_TEXT_FILE, LOADED_JSON_TEXT);
        Debug.Log("Writing file to StreamingAsset from Web");
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
        Debug.Log("Writing file to StreamingAsset from Ressources folder");
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
        Debug.Log("File succesfully created.");
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
            Debug.Log("File found.");
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

    IEnumerator SwitchLanguageRuntime(string langChoice)
    {
        if (!_isTryChnageLangRuntime)
        {
            _isTryChnageLangRuntime = true;
            _isFileFound = false;
            _isReady = false;
            LANGUAGE_CHOICE = langChoice;

            FULL_NAME_TEXT_FILE = FILENAME_PREFIX + LANGUAGE_CHOICE.ToLower() + FILE_EXTENSION;

            #if UNITY_IOS || UNITY_ANDROID
                FULL_PATH_TEXT_FILE = Path.Combine(Application.persistentDataPath, FULL_NAME_TEXT_FILE);
            #else
                FULL_PATH_TEXT_FILE = Path.Combine(Application.streamingAssetsPath, FULL_NAME_TEXT_FILE);
            #endif
            yield return StartCoroutine(LoadJsonLanguageData());
            _isReady = true;
            LocalizedText[] arrayText = FindObjectsOfType<LocalizedText>();
            
            for (int i = 0; i < arrayText.Length; i++)
            {
                arrayText[i].AttributionText();
            }
            _isTryChnageLangRuntime = false;
        }
    }

    public void ChangeLanguage(string lang)
    {
        StartCoroutine(SwitchLanguageRuntime(lang));
    }
}