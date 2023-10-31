using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageDropdown : MonoBehaviour
{
    private TMP_Dropdown m_Dropdown;

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();
        m_Dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(m_Dropdown); });


        //Debug.Log("Dropdown default : " + m_Dropdown.value);
    }
    private void Start()
    {
        Singleton.Instance.localizationMngr.GetLanguageChoice();
        m_Dropdown.value = Singleton.Instance.localizationMngr.GetLanguageChoiceID();
    }

    private void OnDropdownValueChanged(TMP_Dropdown change)
    {
        string languageChoice;

        switch (change.value)
        {
            case 0:
                languageChoice = "fr";
                break;
            case 1:
                languageChoice = "en";
                break;
            default:
                languageChoice = "fr";
                break;
        }

        Singleton.Instance.localizationMngr.ChangeLanguage(languageChoice);
        //Debug.Log("Language choice set to " + languageChoice);
    }
}
