using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedAsset : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _assets;

    private IEnumerator Start()
    {
        while (!Singleton.Instance.localizationMngr.LocalizationIsReady())
        {
            yield return null;
        }
        AttributeAsset();
    }

    public void AttributeAsset()
    {
        switch (Singleton.Instance.localizationMngr.GetLanguageChoice())
        {
            case "fr":
                this.GetComponent<SignsBehaviour>().ShowSignsByID(0);
                break;
            case "en":
                this.GetComponent<SignsBehaviour>().ShowSignsByID(1);
                break;
            default:
                this.GetComponent<SignsBehaviour>().ShowSignsByID(0);
                break;
        }
    }

    public void AttributeAssetOLD()
    {
        switch (Singleton.Instance.localizationMngr.ReturnApplicationLang())
        {
            case ApplicationLanguage.FR:
                this.GetComponent<SignsBehaviour>().ShowSignsByID(0);
                break;
            case ApplicationLanguage.EN:
                this.GetComponent<SignsBehaviour>().ShowSignsByID(1);
                break;
            default:
                this.GetComponent<SignsBehaviour>().ShowSignsByID(0);
                break;
        }
    }
}
