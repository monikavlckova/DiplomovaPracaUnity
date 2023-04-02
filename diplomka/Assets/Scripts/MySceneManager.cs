using System.Collections;
using UnityEngine;

public class MySceneManager : MonoBehaviour
{
    public void ChangeToScene(int sceneToChange)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToChange);
    }
    
}

