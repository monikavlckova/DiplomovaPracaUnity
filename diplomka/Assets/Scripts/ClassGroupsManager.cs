using System;
using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClassGroupsManager : MonoBehaviour
{
    public Text className;
    public GridLayoutGroup groupsLayout;
    public Canvas canvas;
    public Button prefabItem;
    public Button backButton;
    public Button addGroup;
    
    public Button classTasksButton;
    public Button classStudentsButton;

    public GameObject editPanel;
    public Button closeEditPanel;
    public Button editButton;
    public Button deleteButton;

    public GameObject groupPanel;
    public Button closeGroupPanel;
    public Button saveButton;
    public InputField groupName;
    public GridLayoutGroup studentsInGoupList;
    public GridLayoutGroup studentsNotInGoupList;
    public GameObject prefabStudentListItem;
    
    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;

    private Group _delEditGroup;
    private bool _creatingNew;
    
    //TODO pridavanie ziakov do skupin, prezeranie si ziakov v skupine
    private void Start()
    {
        var groups = APIHelper.GetGroupsInClassroom(Constants.ClassroomId);
        AddGroupsToGrid(groups);
        var classroom = APIHelper.GetClassroom(Constants.ClassroomId);
        className.text = classroom.name;
        
        backButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Classes"); 
        });
        
        float width = canvas.GetComponent<RectTransform>().rect.width;
        float width2 = (width - 100) / 2;
        groupsLayout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, width2+80);
        studentsInGoupList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        studentsNotInGoupList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        
        classTasksButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassTasks"));
        classStudentsButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassStudents"));

        addGroup.onClick.AddListener(() => {
            _creatingNew = true;
            saveButton.transform.Find("Text").GetComponent<Text>().text = Constants.SaveButtonTextCreate;
            SetActiveGroupPanel();
        });
        
        editButton.onClick.AddListener(() =>
        {
            _creatingNew = false;
            saveButton.transform.Find("Text").GetComponent<Text>().text = Constants.SaveButtonTextUpdate;
            groupName.text = _delEditGroup.name;
            SetActiveGroupPanel();
            editPanel.SetActive(false);
        });
        
        saveButton.onClick.AddListener(() =>
        {
            //TODO skontroluj vstup
            var group = new Group
            {
                name = groupName.text,
                //TODO zmen nech moze zmenit triedu
                classroomId = Constants.ClassroomId
            };
            var method = "PUT";
            if (_creatingNew == false)
            {
                group.id = Constants.GroupId;
                method = "POST";
            }
            APIHelper.CreateUpdateGroup(group, method);
            SceneManager.LoadScene("Scenes/ClassGroups");
        });
        
        closeGroupPanel.onClick.AddListener(() => {
            groupPanel.SetActive(false);
        });
        
        closeEditPanel.onClick.AddListener(() => {
            editPanel.SetActive(false);
        });
        
        editPanel.GetComponent<Button>().onClick.AddListener(() => {
            editPanel.SetActive(false);
        });
        
        deleteButton.onClick.AddListener(() =>
        {
            _delEditGroup = APIHelper.GetGroup(Constants.GroupId);
            deletePanel.SetActive(true);
            editPanel.SetActive(false);
            deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = Constants.GetDeleteGroupString(_delEditGroup);
        });
        
        closeDeletePanel.onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });
        
        deletePanel.GetComponent<Button>().onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });
        
        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteGroup(Constants.GroupId);
            //TODO zmenit? mam nanovo nacitat? zatial ok
            SceneManager.LoadScene("Scenes/ClassGroups");
        });
    }

    private void SetActiveGroupPanel()
    {
        groupPanel.SetActive(true);
        var studentsInGroup = APIHelper.GetStudentsInGroup(Constants.GroupId);
        var studentsNotInGroup = APIHelper.GetStudentsNotInGroup(Constants.GroupId, Constants.ClassroomId);
        AddStudentsToGrid(studentsInGroup, true);
        AddStudentsToGrid(studentsNotInGroup, false);
        var studentsCount = studentsInGroup.Count;
        var studentsCount2 = studentsNotInGroup.Count;
        var height = (studentsCount + 1) / 2;
        var height2 = (studentsCount2 + 1) / 2;
        RectTransform rt = studentsInGoupList.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2 (rt.sizeDelta.x, height);
        RectTransform rt2 = studentsNotInGoupList.GetComponent<RectTransform>();
        rt2.sizeDelta = new Vector2 (rt2.sizeDelta.x, height2);
    }

    private void AddGroupsToGrid(List<Group> list)
    {
        foreach (var group in list)
        {
            AddGroupToGrid(group);
        }
    }
    
    private void AddGroupToGrid(Group group)
    {
        var g = Instantiate(prefabItem, groupsLayout.transform);
        g.onClick.AddListener(() => {
            Constants.GroupId = group.id;
            //TODO co ked klinke na skupinu, zobraz studentov, a ulohy
        });
        var edit = g.transform.Find("Edit").GetComponent<Button>();
        edit.onClick.AddListener(() =>
        {
            _delEditGroup = group;
            Constants.GroupId = group.id;
            editPanel.SetActive(true);
        });
        g.GetComponentInChildren<Text>().text  = (group.name);
    }
    
    private void AddStudentsToGrid(List<Student> list, Boolean isInGroup)
    {
        if (isInGroup) foreach (var student in list) AddStudentToGrid(student); 
        else foreach (var student in list) AddStudentToGrid(student); 
    }
    
    private void AddStudentToGrid(Student student)
    {
        var studentItem = Instantiate(prefabStudentListItem, grid.transform);
        studentItem.transform.Find("Close").GetComponent<Button>().onClick.AddListener(() => {
            //TODO odstrani ziaka zo skupiny alebo prida
        });
        studentItem.transform.Find("Text").GetComponent<Text>().text  = (student.name + " " + student.lastName);
    }
    
}
