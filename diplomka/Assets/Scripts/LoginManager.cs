using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public GameObject forgottenPasswordPanel;
    public Button closeForgottenPasswordPanel;
    public Button sendNewPasswordButton;
    [FormerlySerializedAs("emailFP")] public InputField email;
    public Button back;
    public Button loginButton;
    public Button signupButton;
    [FormerlySerializedAs("forgotPasswordButton")] public Button forgottenPasswordButton;
    public InputField userName;
    public InputField password;
    private void Start()
    {
        loginButton.onClick.AddListener(() => 
        {
            //TODO skonroluj meno heslo
            //Constants.UserId = id;
            SceneManager.LoadScene("Scenes/Classes"); 
        });
        
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/First"); 
        });
        
        signupButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Signup"); 
        });
        
        closeForgottenPasswordPanel.onClick.AddListener(() => {
            forgottenPasswordPanel.SetActive(false);
        });
        
        forgottenPasswordButton.onClick.AddListener(() => {
            forgottenPasswordPanel.SetActive(true);
        });
        
        sendNewPasswordButton.onClick.AddListener(() => {
            //TODO odosli email
            forgottenPasswordPanel.SetActive(false);
        });
    }
    

}
