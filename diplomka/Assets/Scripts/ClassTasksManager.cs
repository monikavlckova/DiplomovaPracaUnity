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
    
    private Task _delEditTask;
    private bool _creatingNew;
    

    private void Start()
    {
        var tasks = APIHelper.GetTasksInClassroom(Constants.ClassroomId);
        AddTasksToGrid(tasks);
        var classroom = APIHelper.GetClassroom(Constants.ClassroomId);
        className.text = classroom.name;
        
        backButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Classes"); 
        });
        
        float width = canvas.GetComponent<RectTransform>().rect.width;
        Vector2 newTaskSize = new Vector2((width - 100) / 2, (width - 100) / 2);
        tasksLayout.GetComponent<GridLayoutGroup>().cellSize = newTaskSize;
        
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
            _delEditTask = APIHelper.GetTask(Constants.TaskId);
            deletePanel.SetActive(true);
            deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = Constants.GetDeleteTaskString(_delEditTask);
        });
        
        closeDeletePanel.onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });
        
        deletePanel.GetComponent<Button>().onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });
        
        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteTask(Constants.TaskId);
            //TODO zmenit? mam nanovo nacitat? zatial ok
            SceneManager.LoadScene("Scenes/ClassTasks");
        });
    }

    private void AddTasksToGrid(List<Task> list)
    {
        foreach (var task in list)
        {
            AddTaskToGrid(task);
        }
    }

    private void AddTaskToGrid(Task task)
    {
        var t = Instantiate(prefabItem, tasksLayout.transform);
        t.onClick.AddListener(() => {
            Constants.StudentId = task.id;
            SceneManager.LoadScene("Scenes/Task"); 
        });
        var edit = t.transform.Find("Edit").GetComponent<Button>();
        edit.onClick.AddListener(() =>
        {
            //TODO edit panel
        });
        t.GetComponentInChildren<Text>().text  = (task.name);
    }
}
