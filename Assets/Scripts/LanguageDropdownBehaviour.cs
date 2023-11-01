using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class LanguageDropdownBehaviour : MonoBehaviour
{
    private TMP_Dropdown m_Dropdown;

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();
        m_Dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(m_Dropdown); });
    }

    private void Start()
    {
        m_Dropdown.value = (int)Singleton.Instance.localizationService.GetApplicationLang();
    }

    private void OnDropdownValueChanged(TMP_Dropdown dropdown)
    {
        Singleton.Instance.localizationService.ChangeLanguage((ApplicationLanguage)dropdown.value);
    }

    private void OnAppplicationLanguageChanged(ApplicationLanguage lang)
    {
        m_Dropdown.value = (int)Singleton.Instance.localizationService.GetApplicationLang();
    }
}
