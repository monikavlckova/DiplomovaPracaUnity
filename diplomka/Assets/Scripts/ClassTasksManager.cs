using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClassTasksManager : MonoBehaviour
{
    public Text className;
    public GridLayoutGroup tasksLayout;
    public Canvas canvas;
    public Button prefabItem;
    public Button backButton;
    public Button addTask;
    
    public Button classStudentsButton;
    public Button classGroupsButton;
    
    public GameObject editPanel;
    public Button closeEditPanel;
    public Button editButton;
    public Button deleteButton;
    
    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;
    
    private Taskk _delEditTaskk;
    private bool _creatingNew;
    private string sceneName = "ClassTasks";
    

    private void Start()
    {
        float width = canvas.GetComponent<RectTransform>().rect.width;
        float width3 = (width - 120) / 3;
        tasksLayout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width3, width3+80);
        
        prefabItem.transform.Find("Edit").GetComponent<Image>().sprite = Constants.dotsSprite;

        var tasks = APIHelper.GetTasksInClassroom(Constants.Classroom.id);
        AddTasksToGrid(tasks);
        var classroom = Constants.Classroom;
        className.text = classroom.name;
        
        backButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Classes"); 
        });

        classStudentsButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassStudents"));
        classGroupsButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassGroups"));
        
        addTask.onClick.AddListener(() => {
            _creatingNew = true;
            //TODO zobraz panel na vytvaranie a upravu ulohy
        });
        
        editButton.onClick.AddListener(() =>
        {
            _creatingNew = false;
            editPanel.SetActive(false);
            //TODO zobraz panel na vytvaranie a upravu ulohy
        });
        
        
        closeEditPanel.onClick.AddListener(() => {
            editPanel.SetActive(false);
        });
        
        editPanel.GetComponent<Button>().onClick.AddListener(() => {
            editPanel.SetActive(false);
        });
        
        deleteButton.onClick.AddListener(() =>
        {
            _delEditTaskk = Constants.Taskk;
            deletePanel.SetActive(true);
            deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = Constants.GetDeleteTaskString(_delEditTaskk);
        });
        
        closeDeletePanel.onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });
        
        deletePanel.GetComponent<Button>().onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });
        
        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteTask(Constants.Taskk.id);
            Constants.mySceneManager.Reload(sceneName);
        });
    }

    private void AddTasksToGrid(List<Taskk> list)
    {
        foreach (var task in list)
        {
            AddTaskToGrid(task);
        }
    }

    private void AddTaskToGrid(Taskk taskk)
    {
        var t = Instantiate(prefabItem, tasksLayout.transform);
        t.onClick.AddListener(() => {
            Constants.Taskk = taskk;
            Constants.LastSceneName = sceneName;
            SceneManager.LoadScene("Scenes/Task"); 
        });
        var edit = t.transform.Find("Edit").GetComponent<Button>();
        edit.onClick.AddListener(() =>
        {
            //TODO edit panel
        });
        t.GetComponentInChildren<Text>().text  = (taskk.name);
    }
}
