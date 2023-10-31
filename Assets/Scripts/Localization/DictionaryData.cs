using System.Collections.Generic;

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
