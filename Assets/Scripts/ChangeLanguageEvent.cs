using UnityEngine;

public class ChangeLanguageEvent : MonoBehaviour
{
    public void OnClickChangeLanguageRuntime(string lang)
    {
        LocalizationManager.Instance.ChangeLanguage(lang);
    }

    public void OnSelectChangeLanguageRuntime()
    {

    }



}
