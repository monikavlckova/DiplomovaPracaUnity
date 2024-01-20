using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstManager : MonoBehaviour
{
    public Button teacherButton;
    public Button studentButton;
    
    private string sceneName = "First";
    private void Start()
    {
        teacherButton.onClick.AddListener(() =>
        {
            Constants.LastSceneName = sceneName;
            SceneManager.LoadScene("Scenes/Login");
        });
    }
    

}
