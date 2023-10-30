using UnityEditor.PackageManager;
using UnityEngine;

public static class LocaleHelper
{
    public static string GetSupportedLanguageCode()
    {
        SystemLanguage lang = Application.systemLanguage;
    
         switch(lang)
        {
            case SystemLanguage.French:
                return ApplicationLocale.FR;
            case SystemLanguage.English:
                return ApplicationLocale.EN;
            case SystemLanguage.Spanish:
                return ApplicationLocale.ES;
            case SystemLanguage.German:
                return ApplicationLocale.DE;
            default:
                return GetDefaultSupportLangaugeCode();
        }

    }

    static string GetDefaultSupportLangaugeCode()
    {
        return ApplicationLocale.EN;
    }
}