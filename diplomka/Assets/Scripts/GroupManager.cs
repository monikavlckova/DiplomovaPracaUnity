using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GroupManager : MonoBehaviour
{
    public Text headline;
    public GridLayoutGroup layout;
    public Button prefabItem;
    public Button back;

    private void Start()
    {
        var group = Constants.Group;
        var tasks = APIHelper.GetGroupsTasks(Constants.Group.id);
        AddTasksToGrid(tasks);
        headline.text = group.name;
        
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/ClassGroups"); 
        });
    }

    public void AddTasksToGrid(List<Taskk> list)
    {
        foreach (var task in list)
        {
            var s = Instantiate(prefabItem, layout.transform);
            s.onClick.AddListener(() => 
            {
                Constants.Taskk = task;
                Constants.LastSceneName = "Group";
                SceneManager.LoadScene("Scenes/Task"); 
            });
            s.GetComponentInChildren<Text>().text  = (task.name);
        }
    }
    

}
