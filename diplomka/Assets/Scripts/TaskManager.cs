using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public Button back;
    private void Start()
    {
        //var task = APIHelper.GetTask(Constants.TaskId);
        
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/"+ Constants.LastSceneName); 
        });
        
    }
    

}
