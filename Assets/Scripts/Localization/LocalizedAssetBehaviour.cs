using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedAssetBehaviour : MonoBehaviour
{
    [Tooltip("0 = french, 1 = english, 2 = germand, 3 = italian")]
    [TextArea] private string notes = "This is a comment";
    [SerializeField] private List<GameObject> _localizedAssets;

    private SignsBehaviour _signsBehaviour;

    private void Awake()
    {
        _signsBehaviour = GetComponent<SignsBehaviour>();
    }

    private IEnumerator Start()
    {
        while (!Singleton.Instance.localizationService.LocalizationIsReady())
        {
            yield return null;
        }
        SetAsset();
    }

    public void SetAsset()
    {
       _signsBehaviour.ShowSignsByID((int)Singleton.Instance.localizationService.GetApplicationLang());
    }
}
