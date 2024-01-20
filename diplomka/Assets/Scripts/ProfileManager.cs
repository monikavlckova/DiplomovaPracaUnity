using System;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    
    public Button back;
    public Button save;
    public Button edit;

    public GameObject mainPanel;
    public GameObject name;
    public Button changePassword;


    public GameObject editPanel;
    public InputField editFirstName;
    public InputField editLastName;
    public InputField editUserName;
    public InputField editEmail;
    public Button editChangePassword;
    
    public GameObject changePasswordPanel;
    public InputField password1;
    public InputField password2;
    public Button closeEditPanel;
    public Button saveChangedPassword;
    
    private string sceneName = "Profile";

    private void Start()
    {
        //var teacher = APIHelper.GetTeacher(Constants.UserId);
        var teacher = Constants.User;
        SetValuesInEditPanel();

        name.transform.Find("Text").GetComponent<Text>().text = teacher.name + " " + teacher.lastName;

        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Classes"); 
        });
        
        save.onClick.AddListener(() =>
        {
            if(!AreValidValues()) return;
            teacher.name = editFirstName.text;
            teacher.lastName = editLastName.text;
            teacher.userName = editUserName.text;
            teacher.email = editEmail.text;
            APIHelper.CreateUpdateTeacher(teacher, "POST");
            mainPanel.SetActive(true);
            editPanel.SetActive(false);
            edit.gameObject.SetActive(true);
            save.gameObject.SetActive(false);
        });
        
        edit.onClick.AddListener(() =>
            {
                SetValuesInEditPanel();
                mainPanel.SetActive(false);
                editPanel.SetActive(true);
                edit.gameObject.SetActive(false);
                save.gameObject.SetActive(true);
            });
        
        changePassword.onClick.AddListener(() =>
        {
            changePasswordPanel.SetActive(true);
        });

        editChangePassword.onClick.AddListener(() =>
        {
            changePasswordPanel.SetActive(true);
        });

        password1.transform.Find("Show").GetComponent<Button>().onClick.AddListener(() =>
        {
            password1.contentType = password1.contentType == InputField.ContentType.Standard ? InputField.ContentType.Password : InputField.ContentType.Standard;
            password1.ForceLabelUpdate();
            
        });
        
        password2.transform.Find("Show").GetComponent<Button>().onClick.AddListener(() =>
        {
            password2.contentType = password2.contentType == InputField.ContentType.Standard ? InputField.ContentType.Password : InputField.ContentType.Standard;
            password2.ForceLabelUpdate();
        });
        
        saveChangedPassword.onClick.AddListener(() =>
        {
            if (!IsValidPassword()) return;
            
            teacher.password = password1.text;
            ClosePasswordPanel();
            APIHelper.CreateUpdateTeacher(teacher, "POST");
        });
        
        closeEditPanel.onClick.AddListener(ClosePasswordPanel);
        
        changePasswordPanel.GetComponent<Button>().onClick.AddListener(ClosePasswordPanel);
    }

    private void ClosePasswordPanel()
    {
        password1.text = "";
        password2.text = "";
        changePasswordPanel.SetActive(false);
    }

    private void SetValuesInEditPanel()
    {
        var teacher = Constants.User;
        editFirstName.text = teacher.name;
        editLastName.text = teacher.lastName;
        editUserName.text = teacher.userName;
        editEmail.text = teacher.email;
    }

    private bool AreValidValues()
    {
        var emailUnderline = editEmail.transform.Find("underline");
        var userNameUnderline = editUserName.transform.Find("underline");
        var firstNameUnderline = editFirstName.transform.Find("underline");
        var lastNameUnderline = editLastName.transform.Find("underline");
        emailUnderline.gameObject.SetActive(false);
        userNameUnderline.gameObject.SetActive(false);
        firstNameUnderline.gameObject.SetActive(false);
        lastNameUnderline.gameObject.SetActive(false);
        var valid = true;

        if (editFirstName.text.Length < Constants.MinimalFirstNameLength)
        {
            firstNameUnderline.gameObject.SetActive(true);
            firstNameUnderline.GetComponent<Text>().text = Constants.WrongFirstNameFormatMessage;
            valid = false;
        }

        if (editLastName.text.Length < Constants.MinimalLastNameLength)
        {
            lastNameUnderline.gameObject.SetActive(true);
            lastNameUnderline.GetComponent<Text>().text = Constants.WrongLastNameFormatMessage;
            valid = false;
        }

        if (editUserName.text.Length < Constants.MinimalUserNameLength)
        {
            userNameUnderline.gameObject.SetActive(true);
            userNameUnderline.GetComponent<Text>().text = Constants.WrongUserNameFormatMessage;
            valid = false;
        }
        else
        {
            var user = APIHelper.GetTeacherByUserName(editUserName.text);
            if (user is not null && user.id != Constants.User.id)
            {
                userNameUnderline.gameObject.SetActive(true);
                userNameUnderline.GetComponent<Text>().text = Constants.WrongUserNameAlreadyExistMessage;
                valid = false;
            }
        }

        try
        {
            var _ = new MailAddress(editEmail.text);
            var user = APIHelper.GetTeacherByEmail(editEmail.text);
            if (user is not null && user.id != Constants.User.id)
            {
                emailUnderline.gameObject.SetActive(true);
                emailUnderline.GetComponent<Text>().text = Constants.WrongEmailAlreadyExistMessage;
                valid = false;
            }
        }
        catch
        {
            emailUnderline.gameObject.SetActive(true);
            emailUnderline.GetComponent<Text>().text = Constants.WrongEmailFormatMessage;
            valid = false;
        }

        return valid;
    }
    
    private bool IsValidPassword()
    {
        var password1UnderLine = password1.transform.Find("underline");
        var password2UnderLine = password2.transform.Find("underline");
        password1UnderLine.gameObject.SetActive(false);
        password2UnderLine.gameObject.SetActive(false);
        var valid = true;
        

        if (password1.text != password2.text)
        {
            password2UnderLine.gameObject.SetActive(true);
            password2UnderLine.GetComponent<Text>().text = Constants.WrongPasswordsNotSameMessage;
            valid = false;
        }
        
        if(password1.text.Length < Constants.MinimalPasswordLength)
        {
            password1UnderLine.gameObject.SetActive(true);
            password1UnderLine.GetComponent<Text>().text = Constants.WrongPasswordFormatMessage;
            valid = false;
        }
        

        return valid;
    }
}
