using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    
    public Button back;
    public Button save;
    public InputField firstName;
    public InputField lastName;
    public InputField userName;
    public InputField email;
    public Button changePassword;
    
    public GameObject changePasswordPanel;
    public InputField password1;
    public InputField password2;
    public Button closeEditPanel;
    public Button saveChangedPassword;

    private void Start()
    {
        var teacher = APIHelper.GetTeacher(Constants.UserId);
        firstName.text = teacher.name;
        lastName.text = teacher.lastName;
        userName.text = teacher.userName;
        email.text = teacher.email;
        
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Classes"); 
        });
        
        save.onClick.AddListener(() =>
        {
            //TODO skontroluj vstup
            teacher.name = firstName.text;
            teacher.lastName = lastName.text;
            teacher.userName = userName.text;
            teacher.email = email.text;
            APIHelper.CreateUpdateTeacher(teacher, "POST");
        });

        changePassword.onClick.AddListener(() =>
        {
            changePasswordPanel.SetActive(true);
        });
        
        saveChangedPassword.onClick.AddListener(() =>
        {
            //TODO skontroluj hesla ak su rozne vyhod upozornenie inak vyhod upozornenie uspesne zmenene heslo
            if (password1.text == password2.text)
            {
                teacher.password = password1.text;
                APIHelper.CreateUpdateTeacher(teacher, "POST");
            }

            password1.text = "";
            password2.text = "";
            changePasswordPanel.SetActive(false);
        });
        
        closeEditPanel.onClick.AddListener(() =>
        {
            changePasswordPanel.SetActive(false);
        });
    }
}
