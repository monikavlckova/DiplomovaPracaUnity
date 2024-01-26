using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public void Reload(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
}

