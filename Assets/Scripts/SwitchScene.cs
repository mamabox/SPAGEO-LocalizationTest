using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void onClickSwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
