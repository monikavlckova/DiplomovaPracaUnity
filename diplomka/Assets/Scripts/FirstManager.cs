using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstManager : MonoBehaviour
{
    public Button teacherButton;
    public Button studentButton;
    
    private const string SceneName = "First";
    private void Start()
    {
        Constants.LastSceneName = SceneName;
        
        teacherButton.onClick.AddListener(() => SceneManager.LoadScene("Login"));
        studentButton.onClick.AddListener(() => SceneManager.LoadScene("LoginStudent"));
    }
    

}
