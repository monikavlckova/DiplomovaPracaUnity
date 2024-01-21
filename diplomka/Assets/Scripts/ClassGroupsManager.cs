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
    public GridLayoutGroup studentsInGroupList;
    public GridLayoutGroup studentsNotInGroupList;
    public GameObject prefabStudentListItem;
    
    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;

    private Group _delEditGroup;
    private bool _creatingNewGroup;
    private HashSet<Student> _delFromGroup = new ();
    private HashSet<Student> _addToGroup = new ();
    private string sceneName = "ClassGroups";
    
    private void Start() {
        float width = canvas.GetComponent<RectTransform>().rect.width;
        float width2 = (width - 100) / 2;
        groupsLayout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, width2+80);
        
        studentsInGroupList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        studentsNotInGroupList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        
        prefabItem.transform.Find("Edit").GetComponent<Image>().sprite = Constants.dotsSprite;
        
        var groups = APIHelper.GetGroupsInClassroom(Constants.Classroom.id);
        AddGroupsToGrid(groups);
        var classroom = Constants.Classroom;
        className.text = classroom.name;
        
        backButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Classes"); 
        });
        
        classTasksButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassTasks"));
        classStudentsButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassStudents"));

        addGroup.onClick.AddListener(() => {
            _creatingNewGroup = true;
            groupName.text = "";
            saveButton.transform.Find("Text").GetComponent<Text>().text = Constants.SaveButtonTextCreate;
            SetActiveGroupPanel();
        });
        
        editButton.onClick.AddListener(() => {
            _creatingNewGroup = false;
            saveButton.transform.Find("Text").GetComponent<Text>().text = Constants.SaveButtonTextUpdate;
            groupName.text = _delEditGroup.name;
            SetActiveGroupPanel();
            editPanel.SetActive(false);
        });
        
        saveButton.onClick.AddListener(() => {
            if (!AreValidValues()) return;
            var group = new Group {
                name = groupName.text,
                classroomId = Constants.Classroom.id
            };
            var method = "PUT";
            if (_creatingNewGroup == false) {
                group.id = Constants.Group.id;
                method = "POST";
            }
            APIHelper.CreateUpdateGroup(group, method);
            ManageStudents();
            Constants.mySceneManager.Reload(sceneName);
        });
        
        closeGroupPanel.onClick.AddListener(CloseGroupPanel);
        closeEditPanel.onClick.AddListener(() => editPanel.SetActive(false));
        closeDeletePanel.onClick.AddListener(() => deletePanel.SetActive(false));
        deletePanel.GetComponent<Button>().onClick.AddListener(() => deletePanel.SetActive(false));
        editPanel.GetComponent<Button>().onClick.AddListener(() => editPanel.SetActive(false));

        deleteButton.onClick.AddListener(() => {
            _delEditGroup = Constants.Group;
            deletePanel.SetActive(true);
            editPanel.SetActive(false);
            deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = Constants.GetDeleteGroupString(_delEditGroup);
        });
        
        confirmDelete.onClick.AddListener(() => {
            APIHelper.DeleteGroup(Constants.Group.id);
            Constants.mySceneManager.Reload(sceneName);
        });
    }

    private void CloseGroupPanel()
    {
        groupPanel.SetActive(false);
        Constants.Group = null;
    }

    private void SetActiveGroupPanel() {
        foreach (Transform child in studentsInGroupList.transform) {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in studentsNotInGroupList.transform) {
            Destroy(child.gameObject);
        }
        
        groupPanel.SetActive(true);
        var studentsInGroup = new List<Student>();
        var studentsNotInGroup = APIHelper.GetStudentsInClassroom(Constants.Classroom.id);
        if (!_creatingNewGroup)
        {
            studentsInGroup = APIHelper.GetStudentsInGroup(Constants.Group.id);
            studentsNotInGroup = APIHelper.GetStudentsFromClassroomNotInGroup(Constants.Classroom.id, Constants.Group.id);
        }

        AddStudentsToLists(studentsInGroup, studentsNotInGroup);
        ResizeStudentGroupLists();
    }

    private void ResizeStudentGroupLists() {
        var height = 95*((studentsInGroupList.transform.childCount + 1) / 2)-15;
        var height2 = 95*((studentsNotInGroupList.transform.childCount + 1) / 2)-15;
        RectTransform rt = studentsInGroupList.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2 (rt.sizeDelta.x, height);
        RectTransform rt2 = studentsNotInGroupList.GetComponent<RectTransform>();
        rt2.sizeDelta = new Vector2 (rt2.sizeDelta.x, height2);
    }

    private void AddGroupsToGrid(List<Group> list)
    {
        foreach (var group in list) AddGroupToGrid(group);
    }
    
    private void AddGroupToGrid(Group group) {
        var g = Instantiate(prefabItem, groupsLayout.transform);
        g.onClick.AddListener(() => {
            Constants.Group = group;
            Constants.LastSceneName = sceneName;
            SceneManager.LoadScene("Scenes/GroupStudents"); 
        });
        var edit = g.transform.Find("Edit").GetComponent<Button>();
        edit.onClick.AddListener(() => {
            _delEditGroup = group;
            Constants.Group = group;
            editPanel.SetActive(true);
        });
        g.GetComponentInChildren<Text>().text  = (group.name);
    }
    
    private void AddStudentsToLists(List<Student> studentsInGroup, List<Student> studentsNotInGroup) {
        foreach (var student in studentsInGroup) AddStudentToList(student, studentsInGroupList, true); 
        foreach (var student in studentsNotInGroup) AddStudentToList(student, studentsNotInGroupList, false); 
    }
    
    private void AddStudentToList(Student student, GridLayoutGroup grid, bool isInGroup) {
        var addedInGroup = isInGroup;
        var studentItem = Instantiate(prefabStudentListItem, grid.transform);
        studentItem.transform.Find("Text").GetComponent<Text>().text  = (student.name + " " + student.lastName);
        
        studentItem.transform.Find("Close").GetComponent<Button>().onClick.AddListener(() => {
            //Constants.Student = student;
            if (addedInGroup) {
                studentItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
                studentItem.transform.parent = studentsNotInGroupList.transform;
                addedInGroup = false;
                if (isInGroup) _delFromGroup.Add(student);
                else _addToGroup.Remove(student);
            }
            else {
                studentItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.plusSprite;
                studentItem.transform.parent = studentsInGroupList.transform;
                addedInGroup = true;
                if (!isInGroup) _addToGroup.Add(student);
                else _delFromGroup.Remove(student);
            }
            ResizeStudentGroupLists();
        });
    }

    private void ManageStudents() {
        foreach (var student in _addToGroup) {
            StudentGroup studentGroup = new StudentGroup { groupId = Constants.Group.id, studentId = student.id };
            APIHelper.CreateUpdateStudentGroup(studentGroup);
        }

        foreach (var student in _delFromGroup) APIHelper.DeleteStudentGroup(student.id, Constants.Group.id);
        
        _delFromGroup = new HashSet<Student>();
        _addToGroup = new HashSet<Student>();
    }
    
    private bool AreValidValues()
    {
        var nameUnderline = groupName.transform.Find("underline");
        nameUnderline.gameObject.SetActive(false);
        var valid = true;

        if (groupName.text.Length < Constants.MinimalGroupNameLength)
        {
            nameUnderline.gameObject.SetActive(true);
            nameUnderline.GetComponent<Text>().text = Constants.WrongGroupNameFormatMessage;
            valid = false;
        }
        
        return valid;
    }
    
}
