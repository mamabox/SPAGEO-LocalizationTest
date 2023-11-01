using System.Collections.Generic;

/*******************************
 *  DictionaryData.cs
 *  
 *  Used for LanguageDictionary and ShorthandDictionary
 *  
 *******************************/

public class DictionaryData
{
    public List<DictionaryItem> items;
}

[System.Serializable]
public class DictionaryItem
{
    public string key;
    public string value;

}
