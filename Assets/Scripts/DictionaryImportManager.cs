using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using System.IO.Enumeration;

/*******************************
 *  DictionaryImportManager.cs
 *  
 *  Imports dictionary from JSON files
 *  
 *  . - Variables
 *  . - Awake, Start, Update
 *  x - Public Fonctions
 *  
 *******************************/

public class DictionaryImportManager : MonoBehaviour
{
    /*******************************
    *  VARIABLES
    *******************************/

    // Strings
    private const string FILE_EXTENSION = ".json";
    private string _textFileFullPath;
    private string _fileName;
    private string _loadedJson = "";
    

    // Booleans
    private bool _isReady = false;
    private bool _isFileFound = false;

    // JSON Variables
    private Dictionary<string, string> _dictionary;
    private DictionaryData _loadedData;

    /*******************************
    * PUBLIC FUNCTIONS
    *******************************/

    public IEnumerator ImportDictionaryFromJson(string importFile)
    {
        _fileName = importFile;

#if UNITY_IOS || UNITY_ANDROID
        string _fullPathText = Path.Combine(Application.PersistentDataPath, _fileName);
#else
        string _fullPathText = Path.Combine(Application.streamingAssetsPath, _fileName);

#endif
        _textFileFullPath = _fullPathText + FILE_EXTENSION;
        //Debug.Log("fullPathText: " + _textFileFullPath) ;
        yield return StartCoroutine(LoadJsonData());
        //Debug.Log("Dictionary test: " + _dictionary["welcome"]);
    }
    public Dictionary<string, string> GetDictionary()
    {
        return _dictionary;
    }

    /*******************************
     * MAIN
     *******************************/

    // Load data from JSON data and return a dictionnary
    IEnumerator LoadJsonData()
    { 
        CheckIfFileExists();
        yield return new WaitUntil(() => _isFileFound);

        _loadedData = JsonUtility.FromJson<DictionaryData>(_loadedJson);
        _dictionary = new Dictionary<string, string>(_loadedData.items.Count);
        _loadedData.items.ForEach(item =>
        {
            try
            {
                _dictionary.Add(item.key, item.value);
                //Debug.Log("Inside LoadJsonData() key|value: " + item.key + "|"+  item.value);
            }
            catch (Exception e) 
            {
                Debug.LogException(e);
            }
        });
    }

    // If file does not exist, copy it from the web, otherwise load the file from Resources folder
    private void CheckIfFileExists()
    {
        if (!File.Exists(_textFileFullPath))
        {
            // (Option A) Copy from the Web
            // GetURLFileText();
            // StartCoroutine(CopyFileFromWeb(URL));
            // (Option B) Copy from Ressources
            CopyFileFromResources();
        }
        else
        {
            LoadFileContents();
            _isFileFound = true;
        }
    }

    // Copy the file from the Resources folder
    private void CopyFileFromResources()
    {
        TextAsset myFile = Resources.Load(_fileName) as TextAsset;
        if (myFile == null)
        {
            Debug.LogError("Make sure file " + _fileName + " is in Resources folder");
            return;
        }
        _loadedJson = myFile.ToString();
        File.WriteAllText(_textFileFullPath, _loadedJson);
        Debug.Log("Copying file from Resources folder");
        StartCoroutine(WaitForFileCreation());
    }

    // 
    IEnumerator WaitForFileCreation()
    {
        FileInfo myFile = new FileInfo(_fileName);
        float timeout = 5.0f; // To avoid infinite loop

        while (timeout < 0f && !IsFileCreated(myFile))
        {
            timeout -= Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    // Check if the file was copied
    private bool IsFileCreated(FileInfo file)
    {
        FileStream stream = null;
        try
        {
            stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None); //config shouldn't matter
        }
        catch (IOException)
        {
            _isFileFound = true;
            Debug.Log("File found: " + _fileName);
            return true;
        }
        finally
        {
            if (stream != null)
            {
                stream.Close();
            }
        }

        // If file was not found
        _isFileFound = false;
        return false;
    }

    private void LoadFileContents()
    {
        _loadedJson = File.ReadAllText(_textFileFullPath);
        _isFileFound = true;
        //Debug.Log(_loadedJsonText);
    }

}
