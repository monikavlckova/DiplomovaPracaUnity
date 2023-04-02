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
    private void Start()
    {
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Login"); 
        });
        
        sendButton.onClick.AddListener(() => {
            //TODO skontrolovat vstupy, ak chybne podat hlasenie
            Teacher teacher = new Teacher();
            teacher.userName = userName.text;
            teacher.name = firstName.text;
            teacher.lastName = lastName.text;
            teacher.email = email.text;
            teacher.password = password.text;
            var res = APIHelper.CreateUpdateTeacher(teacher);
            Constants.UserId = JsonConvert.DeserializeObject<Teacher>(res).id;
            SceneManager.LoadScene("Scenes/Classes"); 
        });
        
    }
    

}
