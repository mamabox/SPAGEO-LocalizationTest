using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LanguageFile")]
public class LanguageFile : ScriptableObject
{
    public List<LocalizationItems> languageItems;
}

