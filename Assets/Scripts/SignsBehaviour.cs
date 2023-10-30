using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignsBehaviour : MonoBehaviour
{
    [SerializeField] List<GameObject> signs;

    private void Awake()
    {
        HideAllSigns();
        //ShowSignsByID(1);
    }

    public void ShowSignsByID(int ID)
    {
        Debug.Log("ShowSignsByID() " + ID);
        for (int i = 0; i < signs.Count; i++)
        {
            if (signs[i] != null)
            {
                if (i == ID)
                {
                    signs[i].gameObject.SetActive(true);
                }
                else
                {
                    signs[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private void HideAllSigns()
    {
        Debug.Log("HideAllSigns()");
        for (int i = 0; i < signs.Count; i++)
        {
            signs[i].gameObject.SetActive(false);
        }
    }
}
