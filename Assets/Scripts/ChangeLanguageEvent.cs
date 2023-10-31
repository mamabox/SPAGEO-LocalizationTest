using UnityEngine;

public class ChangeLanguageEvent : MonoBehaviour
{
    public void OnClickChangeLanguageRuntime(string lang)
    {
        Singleton.Instance.localizationMngr.ChangeLanguage(lang);
    }

    public void OnSelectChangeLanguageRuntime()
    {

    }



}
