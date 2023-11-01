using System;
using UnityEditor.PackageManager;
using UnityEngine;

/*******************************
 *  LocaleHlelper.cs
 *  
 *  Returns the supported or default Application based on the system Language
 *  
 *  . - Variables
 *  . - Awake, Start, Update
 *  x - Public Fonctions
 *  
 *******************************/

public static class LocaleHelper
{

    /*******************************
     *  PUBLIC FUNCTIONS
     *******************************/
    public static ApplicationLanguage GetSupportedSystemLanguage()
    {
        SystemLanguage lang = Application.systemLanguage;

        switch (lang)
        {
            case SystemLanguage.French:
                return ApplicationLanguage.FR;
            case SystemLanguage.English:
                return ApplicationLanguage.EN;
            case SystemLanguage.Spanish:
                return ApplicationLanguage.DE;
            case SystemLanguage.German:
                return ApplicationLanguage.IT;
            default:
                return GetDefaultLanguage();
        }

    }

    static ApplicationLanguage GetDefaultLanguage()
    {
        return ApplicationLanguage.EN;
    }

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