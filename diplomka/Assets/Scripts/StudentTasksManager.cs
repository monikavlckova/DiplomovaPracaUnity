using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StudentTasksManager : MonoBehaviour
{
    public Text headline;
    public GridLayoutGroup layout;
    public Button groupsButton;
    public Image tasksImage;
    public Button prefabItem;
    public Button back;
    public Canvas canvas;
    public Button addTasks;

    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;
    
    public GameObject tasksPanel;//TODO tasks panel zoznam existujucih
    public Button closeTasksPanel;
    public Button saveButton;
    public GridLayoutGroup tasksInStudentList;
    public GridLayoutGroup tasksNotInStudentList;
    public GameObject prefabTaskListItem;
    
    private HashSet<Taskk> _delFromStudent = new ();
    private HashSet<Taskk> _addToStudent = new ();
    
    private const string SceneName = "StudentTasks";

    private void Start()
    {
        var width = canvas.GetComponent<RectTransform>().rect.width;
        var width3 = (width - 140) / 3;
        layout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width3, width3+80);
         
        float width2 = (width - 120) / 2;
        tasksInStudentList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        tasksNotInStudentList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        
        prefabItem.transform.Find("Edit").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
        tasksImage.sprite = Constants.taskSprite;
        
        var student = Constants.Student;
        var tasks = APIHelper.GetStudentsTasks(Constants.Student.id);
        AddTasksToGrid(tasks);
        headline.text = student.name + " " + student.lastName;
        
        back.onClick.AddListener(() => SceneManager.LoadScene(Constants.LastSceneName));
        groupsButton.onClick.AddListener(() => SceneManager.LoadScene("StudentGroups"));
        closeDeletePanel.onClick.AddListener(() => deletePanel.SetActive(false));
        deletePanel.GetComponent<Button>().onClick.AddListener(() => deletePanel.SetActive(false));

        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteStudentTask(Constants.Student.id, Constants.Taskk.id);
            Constants.mySceneManager.Reload(SceneName);
        });
        
        addTasks.onClick.AddListener(SetActiveTasksPanel);

        saveButton.onClick.AddListener(() => {
            ManageTasks();
            Constants.mySceneManager.Reload(SceneName);
        });
        
        closeTasksPanel.onClick.AddListener(() => tasksPanel.SetActive(false));
    }

    public void AddTasksToGrid(List<Taskk> list)
    {
        foreach (var task in list)
        {
            var s = Instantiate(prefabItem, layout.transform);
            s.onClick.AddListener(() =>
            {
                Constants.Taskk = task;
                //Constants.LastSceneName = SceneName;
                SceneManager.LoadScene("Task");//TODO zobraz progres, riesenie ulohy ziaka
            });
            var edit = s.transform.Find("Edit").GetComponent<Button>();
            edit.onClick.AddListener(() =>
            {
                Constants.Taskk = task;
                deletePanel.SetActive(true);
                deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = Constants.GetDeleteTaskFromStudentString(task);
            });
            s.GetComponentInChildren<Text>().text  = (task.name);
        }
    }
    
    private void SetActiveTasksPanel() {
        foreach (Transform child in tasksInStudentList.transform) Destroy(child.gameObject);
        foreach (Transform child in tasksNotInStudentList.transform) Destroy(child.gameObject);
        
        tasksPanel.SetActive(true);
        var tasksInStudent = APIHelper.GetStudentsTasks(Constants.Student.id);
        var tasksNotInStudent = APIHelper.GetTasksFromTeacherNotInStudent(Constants.User.id, Constants.Student.id);
        
        AddTasksToLists(tasksInStudent, tasksNotInStudent);
        ResizeTaskStudentLists();
    }
    
    private void AddTasksToLists(List<Taskk> tasksInStudent, List<Taskk> tasksNotInStudent)
    {
        foreach (var task in tasksInStudent) AddTaskToList(task, tasksInStudentList, true); 
        foreach (var task in tasksNotInStudent) AddTaskToList(task, tasksNotInStudentList, false); 
    }
    
    private void AddTaskToList(Taskk task, GridLayoutGroup grid, bool isInStudent) {
        var addedInStudent = isInStudent;
        var taskItem = Instantiate(prefabTaskListItem, grid.transform);
        taskItem.transform.Find("Text").GetComponent<Text>().text  = (task.name);
        if (isInStudent) taskItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
        else taskItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.plusSprite;
        
        taskItem.transform.Find("Close").GetComponent<Button>().onClick.AddListener(() => {
            //Constants.Taskk = task;
            if (addedInStudent) {
                taskItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.plusSprite;
                taskItem.transform.parent = tasksNotInStudentList.transform;
                addedInStudent = false;
                if (isInStudent) _delFromStudent.Add(task);
                else _addToStudent.Remove(task);
            }
            else {
                taskItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
                taskItem.transform.parent = tasksInStudentList.transform;
                addedInStudent = true;
                if (!isInStudent) _addToStudent.Add(task);
                else _delFromStudent.Remove(task);
            }
            ResizeTaskStudentLists();
        });
    }
    
    private void ResizeTaskStudentLists() {
        var height = 95*((tasksInStudentList.transform.childCount + 1) / 2)-15;
        var height2 = 95*((tasksNotInStudentList.transform.childCount + 1) / 2)-15;
        RectTransform rt = tasksInStudentList.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2 (rt.sizeDelta.x, height);
        RectTransform rt2 = tasksNotInStudentList.GetComponent<RectTransform>();
        rt2.sizeDelta = new Vector2 (rt2.sizeDelta.x, height2);
    }
    
    private void ManageTasks() {
        foreach (var task in _addToStudent) {
            var studentTask = new StudentTask { studentId = Constants.Student.id, taskkId = task.id };
            APIHelper.CreateUpdateStudentTask(studentTask);
        }

        foreach (var task in _delFromStudent) APIHelper.DeleteStudentTask(Constants.Student.id, task.id);

        _delFromStudent = new HashSet<Taskk>();
        _addToStudent = new HashSet<Taskk>();
    }
}
