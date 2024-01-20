using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public Button back;
    
    private string sceneName = "Task";
    private void Start()
    {
        //var task = Constants.Taskk;
        
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/"+ Constants.LastSceneName); 
        });
        
    }
    

}
