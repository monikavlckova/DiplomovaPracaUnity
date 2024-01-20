using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GroupTasksManager : MonoBehaviour
{
    public Text headline;
    public GridLayoutGroup layout;
    public Button groupStudentsButton;
    public Button prefabItem;
    public Button back;
    public Canvas canvas;
    public Button addTasks;
    
    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;
    
    public GameObject tasksPanel;//TODO tasks panel zoznam existujucih
    
    private string sceneName = "GroupTasks";

    private void Start()
    {
        var width = canvas.GetComponent<RectTransform>().rect.width;
        var width3 = (width - 120) / 3;
        layout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width3, width3+80);
        
        prefabItem.transform.Find("Edit").GetComponent<Image>().sprite = Constants.xSprite;
        
        var group = Constants.Group;
        var tasks = APIHelper.GetGroupsTasks(Constants.Group.id);
        AddTasksToGrid(tasks);
        headline.text = group.name;
        
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/ClassGroups"); 
        });
        
        groupStudentsButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/GroupStudents"));
        
        closeDeletePanel.onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });

        deletePanel.GetComponent<Button>().onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });

        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteGroupTask(Constants.Group.id, Constants.Taskk.id);
            Constants.mySceneManager.Reload(sceneName);
        });
        
        addTasks.onClick.AddListener(() =>
        {
            //TODO studentsPanel.SetActive(true);
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
                Constants.LastSceneName = sceneName;
                SceneManager.LoadScene("Scenes/Task"); //TODO zobraz progres, riesenie ulohy ziaka
            });
            var edit = s.transform.Find("Edit").GetComponent<Button>();
            edit.onClick.AddListener(() =>
            {
                Constants.Taskk = task;
                deletePanel.SetActive(true);
                deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = Constants.GetDeleteTaskFromGroupString(task);
            });
            s.GetComponentInChildren<Text>().text  = (task.name);
        }
    }
    

}
