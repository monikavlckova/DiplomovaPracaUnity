using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GroupTasksManager : MonoBehaviour
{
    public Text headline;
    public GridLayoutGroup layout;
    public Button studentsButton;
    public Image tasksImage;
    public Button prefabItem;
    public Button back;
    public Canvas canvas;
    public Button addTasks;
    
    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;
    
    public GameObject tasksPanel;
    public Button closeTasksPanel;
    public Button saveButton;
    public GridLayoutGroup tasksInGroupList;
    public GridLayoutGroup tasksNotInGroupList;
    public GameObject prefabTaskListItem;
    
    private HashSet<Taskk> _delFromGroup = new ();
    private HashSet<Taskk> _addToGroup = new ();
    private const string SceneName = "GroupTasks";

    private void Start()
    {
        var width = canvas.GetComponent<RectTransform>().rect.width;
        var width3 = (width - 140) / 3;
        layout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width3, width3+80);
        
        float width2 = (width - 120) / 2;
        tasksInGroupList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        tasksNotInGroupList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        
        prefabItem.transform.Find("Edit").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
        tasksImage.sprite = Constants.taskSprite;
        
        var group = Constants.Group;
        var tasks = APIHelper.GetGroupsTasks(Constants.Group.id);
        AddTasksToGrid(tasks);
        headline.text = group.name;
        
        back.onClick.AddListener(() => SceneManager.LoadScene(Constants.LastSceneName));
        studentsButton.onClick.AddListener(() => SceneManager.LoadScene("GroupStudents"));
        closeDeletePanel.onClick.AddListener(() => deletePanel.SetActive(false));
        deletePanel.GetComponent<Button>().onClick.AddListener(() => deletePanel.SetActive(false));

        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteGroupTask(Constants.Group.id, Constants.Taskk.id);
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
                SceneManager.LoadScene("Task"); //TODO zobraz progres, riesenie ulohy skupiny
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
    private void SetActiveTasksPanel() {
        foreach (Transform child in tasksInGroupList.transform) Destroy(child.gameObject);
        foreach (Transform child in tasksNotInGroupList.transform) Destroy(child.gameObject);
        
        tasksPanel.SetActive(true);
        var tasksInGroup = APIHelper.GetGroupsTasks(Constants.Group.id);
        var tasksNotInGroup = APIHelper.GetTasksFromTeacherNotInGroup(Constants.User.id, Constants.Group.id);
        
        AddTasksToLists(tasksInGroup, tasksNotInGroup);
        ResizeTaskGroupLists();
    }
    
    private void AddTasksToLists(List<Taskk> tasksInGroup, List<Taskk> tasksNotInGroup)
    {
        foreach (var task in tasksInGroup) AddTaskToList(task, tasksInGroupList, true); 
        foreach (var task in tasksNotInGroup) AddTaskToList(task, tasksNotInGroupList, false); 
    }
    
    private void AddTaskToList(Taskk task, GridLayoutGroup grid, bool isInGroup) {
        var addedInGroup = isInGroup;
        var taskItem = Instantiate(prefabTaskListItem, grid.transform);
        taskItem.transform.Find("Text").GetComponent<Text>().text  = (task.name);
        if (isInGroup) taskItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
        else taskItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.plusSprite;
        
        taskItem.transform.Find("Close").GetComponent<Button>().onClick.AddListener(() => {
            //Constants.Taskk = task;
            if (addedInGroup) {
                taskItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.plusSprite;
                taskItem.transform.parent = tasksNotInGroupList.transform;
                addedInGroup = false;
                if (isInGroup) _delFromGroup.Add(task);
                else _addToGroup.Remove(task);
            }
            else {
                taskItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
                taskItem.transform.parent = tasksInGroupList.transform;
                addedInGroup = true;
                if (!isInGroup) _addToGroup.Add(task);
                else _delFromGroup.Remove(task);
            }
            ResizeTaskGroupLists();
        });
    }
    
    private void ResizeTaskGroupLists() {
        var height = 95*((tasksInGroupList.transform.childCount + 1) / 2)-15;
        var height2 = 95*((tasksNotInGroupList.transform.childCount + 1) / 2)-15;
        RectTransform rt = tasksInGroupList.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2 (rt.sizeDelta.x, height);
        RectTransform rt2 = tasksNotInGroupList.GetComponent<RectTransform>();
        rt2.sizeDelta = new Vector2 (rt2.sizeDelta.x, height2);
    }
    
    private void ManageTasks() {
        foreach (var task in _addToGroup) {
            var groupTask = new GroupTask { groupId = Constants.Group.id, taskkId = task.id };
            APIHelper.CreateUpdateGroupTask(groupTask);
        }

        foreach (var task in _delFromGroup) APIHelper.DeleteGroupTask(Constants.Group.id, task.id);

        _delFromGroup = new HashSet<Taskk>();
        _addToGroup = new HashSet<Taskk>();
    }
}
