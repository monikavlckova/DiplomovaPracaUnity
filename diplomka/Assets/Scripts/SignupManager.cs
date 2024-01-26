using System.Net.Mail;
using DbClasses;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignupManager : MonoBehaviour
{
    public Button back;
    public Button sendButton;
    public InputField firstName;
    public InputField lastName;
    public InputField userName;
    public InputField email;
    public InputField password;
    
    private const string SceneName = "Signup";
    private void Start()
    {
        back.onClick.AddListener(() => SceneManager.LoadScene("Login"));
        
        sendButton.onClick.AddListener(() =>
        {
            if (!AreValidValues()) return;
            var teacher = new Teacher
            {
                email = email.text,
                userName = userName.text,
                name = firstName.text,
                lastName = lastName.text,
                password = password.text
            };

            var res = APIHelper.CreateUpdateTeacher(teacher);
            Constants.emailSender.SendWelcome(email.text, userName.text);
            Constants.User = JsonConvert.DeserializeObject<Teacher>(res);
            SceneManager.LoadScene("Classes");
        });
    }

    private bool AreValidValues()
    {
        var emailUnderline = email.transform.Find("underline");
        var userNameUnderline = userName.transform.Find("underline");
        var firstNameUnderline = firstName.transform.Find("underline");
        var lastNameUnderline = lastName.transform.Find("underline");
        var passwordUnderline = password.transform.Find("underline");
        emailUnderline.gameObject.SetActive(false);
        userNameUnderline.gameObject.SetActive(false);
        firstNameUnderline.gameObject.SetActive(false);
        lastNameUnderline.gameObject.SetActive(false);
        passwordUnderline.gameObject.SetActive(false);
        var valid = true; 
        
        if (firstName.text.Length < Constants.MinimalFirstNameLength) {
            firstNameUnderline.gameObject.SetActive(true);
            firstNameUnderline.GetComponent<Text>().text = Constants.WrongFirstNameFormatMessage;
            valid = false;
        }
        
        if (lastName.text.Length < Constants.MinimalLastNameLength) {
            lastNameUnderline.gameObject.SetActive(true);
            lastNameUnderline.GetComponent<Text>().text = Constants.WrongLastNameFormatMessage;
            valid = false;
        }
        
        if (password.text.Length < Constants.MinimalPasswordLength) {
            passwordUnderline.gameObject.SetActive(true);
            passwordUnderline.GetComponent<Text>().text = Constants.WrongPasswordFormatMessage;
            valid = false;
        }
        
        if (userName.text.Length < Constants.MinimalUserNameLength) {
            userNameUnderline.gameObject.SetActive(true);
            userNameUnderline.GetComponent<Text>().text = Constants.WrongUserNameFormatMessage;
            valid = false;
        }
        else {
            var user = APIHelper.GetTeacherByUserName(userName.text);
            if (user is not null) {
                userNameUnderline.gameObject.SetActive(true);
                userNameUnderline.GetComponent<Text>().text = Constants.WrongUserNameAlreadyExistMessage;
                valid = false;
            }
        }
        
        try {
            var _ = new MailAddress(email.text);
            var user = APIHelper.GetTeacherByEmail(email.text);
            if (user is not null) {
                emailUnderline.gameObject.SetActive(true);
                emailUnderline.GetComponent<Text>().text = Constants.WrongEmailAlreadyExistMessage;
                valid = false;
            }
        }
        catch {
            emailUnderline.gameObject.SetActive(true);
            emailUnderline.GetComponent<Text>().text = Constants.WrongEmailFormatMessage;
            valid = false;
        }
        
        return valid;
    }


}
