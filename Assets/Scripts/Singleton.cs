using Unity.VisualScripting;
using UnityEngine;

/*
 * This Singleton acts as a Service/Manager locator and give read-only access to the application's managers
 * 
 * */

public class Singleton : MonoBehaviour
{
    // Instance of the Singleton
    public static Singleton Instance { get; private set; }

    // Managers used in game that will be accessed via Singleton
    public LocalizationManager localizationMngr { get; private set; }

    private void Awake()
    {
        // If there is already an instance of the Singleton, delete myself
        if (Instance != null & Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            // Do not destroy the game object when loading different scenes
            DontDestroyOnLoad(gameObject);
            SetupReferences();
        }
    }

    private void SetupReferences()
    {
        localizationMngr = GetComponentInChildren<LocalizationManager>();
    }
}
