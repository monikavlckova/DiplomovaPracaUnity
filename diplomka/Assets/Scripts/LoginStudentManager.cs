using DbClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginStudentManager : MonoBehaviour
{
    public Button back;
    public Button loginButton;
    public InputField userName;
    public InputField password;
    
    private const string SceneName = "Login";
    private void Start()
    {
        loginButton.onClick.AddListener(() => 
        {
            var student = APIHelper.GetStudentByLogin(userName.text, password.text);
            if(!AreValidValues(student)) return;
            Constants.Student = student;
            SceneManager.LoadScene("StudentTasks");//TODO zmen
        });
        
        back.onClick.AddListener(() => SceneManager.LoadScene("First"));
    }

    private bool AreValidValues(Student student)
    {
        //var userNameUnderline = password.transform.Find("underline");
        var passwordUnderline = password.transform.Find("underline");
        //userNameUnderline.gameObject.SetActive(false);
        passwordUnderline.gameObject.SetActive(false);
        var valid = true;
        
        if (student is null)
        {
            passwordUnderline.gameObject.SetActive(true);
            passwordUnderline.GetComponent<Text>().text = Constants.WrongUserNameOrPasswordMessage;
            valid = false;
        }
        return valid;
    }


}
