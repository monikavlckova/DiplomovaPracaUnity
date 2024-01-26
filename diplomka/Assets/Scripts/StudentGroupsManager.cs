using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StudentGroupsManager : MonoBehaviour
{
    public Text headline;
    public GridLayoutGroup layout;
    public Button tasksButton;
    public Image groupsImage;
    public Button prefabItem;
    public Button back;
    public Canvas canvas;
    public Button addGroups;

    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;
    
    public GameObject groupsPanel;//TODO groups panel zoznam existujucih
    public Button closeGroupsPanel;
    public Button saveButton;
    public GridLayoutGroup groupsInStudentList;
    public GridLayoutGroup groupsNotInStudentList;
    public GameObject prefabGroupListItem;
    
    private HashSet<Group> _delFromStudent = new ();
    private HashSet<Group> _addToStudent = new ();
    private const string SceneName = "StudentGroups";

    private void Start()
    {
        var width = canvas.GetComponent<RectTransform>().rect.width;
        var width3 = (width - 140) / 3;
        layout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width3, width3+80);
         
        float width2 = (width - 120) / 2;
        groupsInStudentList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        groupsNotInStudentList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        
        prefabItem.transform.Find("Edit").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
        groupsImage.sprite = Constants.groupSprite;
        
        var student = Constants.Student;
        var groups = APIHelper.GetStudentsGroups(Constants.Student.id);
        AddGroupsToGrid(groups);
        headline.text = student.name + " " + student.lastName;
        
        back.onClick.AddListener(() => SceneManager.LoadScene(Constants.LastSceneName));
        tasksButton.onClick.AddListener(() => SceneManager.LoadScene("StudentTasks"));
        closeDeletePanel.onClick.AddListener(() => deletePanel.SetActive(false));
        deletePanel.GetComponent<Button>().onClick.AddListener(() => deletePanel.SetActive(false));

        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteStudentGroup(Constants.Student.id, Constants.Group.id);
            Constants.mySceneManager.Reload(SceneName);
        });
        
        addGroups.onClick.AddListener(SetActiveGroupsPanel);

        saveButton.onClick.AddListener(() => {
            ManageGroups();
            Constants.mySceneManager.Reload(SceneName);
        });
        
        closeGroupsPanel.onClick.AddListener(() => groupsPanel.SetActive(false));
    }

    public void AddGroupsToGrid(List<Group> list)
    {
        foreach (var group in list)
        {
            var s = Instantiate(prefabItem, layout.transform);
            s.onClick.AddListener(() => 
            {
                Constants.Group = group;
                //Constants.LastSceneName = SceneName;
                SceneManager.LoadScene("GroupTasks");
            });
            var edit = s.transform.Find("Edit").GetComponent<Button>();
            edit.onClick.AddListener(() =>
            {
                Constants.Group = group;
                deletePanel.SetActive(true);
                deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = Constants.GetDeleteGroupFromStudentString(group);
            });
            s.GetComponentInChildren<Text>().text  = (group.name);
        }
    }
    
    private void SetActiveGroupsPanel() {
        foreach (Transform child in groupsInStudentList.transform) Destroy(child.gameObject);
        foreach (Transform child in groupsNotInStudentList.transform) Destroy(child.gameObject);
        
        groupsPanel.SetActive(true);
        var groupsInStudent = APIHelper.GetStudentsGroups(Constants.Student.id);
        var groupsNotInStudent = APIHelper.GetGroupsFromInClassroomNotInStudent(Constants.Classroom.id, Constants.Student.id);
        
        AddGroupsToLists(groupsInStudent, groupsNotInStudent);
        ResizeGroupStudentLists();
    }
    
    private void AddGroupsToLists(List<Group> groupsInStudent, List<Group> groupsNotInStudent)
    {
        foreach (var group in groupsInStudent) AddGroupsToList(group, groupsInStudentList, true); 
        foreach (var group in groupsNotInStudent) AddGroupsToList(group, groupsNotInStudentList, false); 
    }
    
    private void AddGroupsToList(Group group, GridLayoutGroup grid, bool isInStudent) {
        var addedInStudent = isInStudent;
        var groupItem = Instantiate(prefabGroupListItem, grid.transform);
        groupItem.transform.Find("Text").GetComponent<Text>().text  = (group.name);
        if (isInStudent) groupItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
        else groupItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.plusSprite;
        
        groupItem.transform.Find("Close").GetComponent<Button>().onClick.AddListener(() => {
            //Constants.Group = group;
            if (addedInStudent) {
                groupItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.plusSprite;
                groupItem.transform.parent = groupsNotInStudentList.transform;
                addedInStudent = false;
                if (isInStudent) _delFromStudent.Add(group);
                else _addToStudent.Remove(group);
            }
            else {
                groupItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
                groupItem.transform.parent = groupsInStudentList.transform;
                addedInStudent = true;
                if (!isInStudent) _addToStudent.Add(group);
                else _delFromStudent.Remove(group);
            }
            ResizeGroupStudentLists();
        });
    }
    
    private void ResizeGroupStudentLists() {
        var height = 95*((groupsInStudentList.transform.childCount + 1) / 2)-15;
        var height2 = 95*((groupsNotInStudentList.transform.childCount + 1) / 2)-15;
        RectTransform rt = groupsInStudentList.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2 (rt.sizeDelta.x, height);
        RectTransform rt2 = groupsNotInStudentList.GetComponent<RectTransform>();
        rt2.sizeDelta = new Vector2 (rt2.sizeDelta.x, height2);
    }
    
    private void ManageGroups() {
        foreach (var group in _addToStudent) {
            var studentGroup = new StudentGroup { studentId = Constants.Student.id, groupId = group.id };
            APIHelper.CreateUpdateStudentGroup(studentGroup);
        }

        foreach (var group in _delFromStudent) APIHelper.DeleteStudentGroup(Constants.Student.id, group.id);

        _delFromStudent = new HashSet<Group>();
        _addToStudent = new HashSet<Group>();
    }
}
