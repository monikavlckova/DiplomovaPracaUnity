using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstManager : MonoBehaviour
{
    public Button teacherButton;
    public Button studentButton;
    private void Start()
    {
        teacherButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Scenes/Login");
        });
    }
    

}
