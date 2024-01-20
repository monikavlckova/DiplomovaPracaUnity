using System;
using DbClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public GameObject forgottenPasswordPanel;
    public Button closeForgottenPasswordPanel;
    public Button sendNewPasswordButton;
    public InputField email;
    public Button back;
    public Button loginButton;
    public Button signupButton;
    public Button forgottenPasswordButton;
    public InputField userName;
    public InputField password;
    
    private string sceneName = "Login";
    private void Start()
    {
        loginButton.onClick.AddListener(() => 
        {
            var teacher = APIHelper.GetTeacherByLogin(userName.text, password.text);
            if(!AreValidValues(teacher)) return;
            Constants.User = teacher;
            SceneManager.LoadScene("Scenes/Classes");
        });
        
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/First"); 
        });
        
        signupButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Signup"); 
        });
        
        closeForgottenPasswordPanel.onClick.AddListener(CloseForgottenPasswordPanel);

        forgottenPasswordPanel.GetComponent<Button>().onClick.AddListener(CloseForgottenPasswordPanel);

        forgottenPasswordButton.onClick.AddListener(() => {
            forgottenPasswordPanel.SetActive(true);
        });
        
        sendNewPasswordButton.onClick.AddListener(() =>
        {
            email.transform.Find("underline").gameObject.SetActive(false);
            var user = APIHelper.GetTeacherByEmail(email.text);
            if (user is null) email.transform.Find("underline").gameObject.SetActive(true);
            else
            {
                var newPassword = GenerateNewPassword();
                user.password = newPassword;
                APIHelper.CreateUpdateTeacher(user, "POST");
                Constants.emailSender.SendPassword(email.text, user.userName, newPassword);
                forgottenPasswordPanel.SetActive(false);
            }
        });
    }

    private void CloseForgottenPasswordPanel()
    {
        forgottenPasswordPanel.SetActive(false);
        email.text = "";
        email.transform.Find("underline").gameObject.SetActive(false);
    }

    private string GenerateNewPassword()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);
    }

    private bool AreValidValues(Teacher teacher)
    {
        //var userNameUnderline = password.transform.Find("underline");
        var passwordUnderline = password.transform.Find("underline");
        //userNameUnderline.gameObject.SetActive(false);
        passwordUnderline.gameObject.SetActive(false);
        var valid = true;
        
        if (teacher is null)
        {
            passwordUnderline.gameObject.SetActive(true);
            passwordUnderline.GetComponent<Text>().text = Constants.WrongUserNameOrPasswordMessage;
            valid = false;
        }
        return valid;
    }


}
