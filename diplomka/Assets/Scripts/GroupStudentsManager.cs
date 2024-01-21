using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GroupStudentsManager : MonoBehaviour
{
    public Text headline;
    public GridLayoutGroup layout;
    public Button groupTasksButton;
    public Button prefabItem;
    public Button back;
    public Canvas canvas;
    public Button addStudents;

    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;
    
    public GameObject studentsPanel;
    public Button closeStudentsPanel;
    public Button saveButton;
    public GridLayoutGroup studentsInGroupList;
    public GridLayoutGroup studentsNotInGroupList;
    public GameObject prefabStudentListItem;
    
    private HashSet<Student> _delFromGroup = new ();
    private HashSet<Student> _addToGroup = new ();
    private string sceneName = "GroupStudents";

    private void Start()
    {
        var width = canvas.GetComponent<RectTransform>().rect.width;
        var width3 = (width - 120) / 3;
        layout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width3, width3+80);
        
        float width2 = (width - 100) / 2;
        studentsInGroupList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);
        studentsNotInGroupList.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width2, 80);

        prefabItem.transform.Find("Edit").GetComponent<Image>().sprite = Constants.xSprite;
        
        var group = Constants.Group;
        var students = APIHelper.GetStudentsInGroup(Constants.Group.id);
        AddStudentsToGrid(students);
        headline.text = group.name;
        
        back.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassGroups"));
        groupTasksButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/GroupTasks"));
        closeDeletePanel.onClick.AddListener(() => deletePanel.SetActive(false));
        deletePanel.GetComponent<Button>().onClick.AddListener(() => deletePanel.SetActive(false));

        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteStudentGroup(Constants.Student.id, Constants.Group.id);
            Constants.mySceneManager.Reload(sceneName);
        });
        
        addStudents.onClick.AddListener(SetActiveStudentsPanel);
        
        saveButton.onClick.AddListener(() => {
            ManageStudents();
            Constants.mySceneManager.Reload(sceneName);
        });
        
        closeStudentsPanel.onClick.AddListener(() => studentsPanel.SetActive(false));
    }
    
    private void AddStudentsToGrid(List<Student> list)
    {
        foreach (var student in list)
        {
            var s = Instantiate(prefabItem, layout.transform);
            s.onClick.AddListener(() => 
            {
                Constants.Student = student;
                Constants.LastSceneName = sceneName;
                SceneManager.LoadScene("Scenes/StudentTasks"); 
            });
            var edit = s.transform.Find("Edit").GetComponent<Button>();
            edit.onClick.AddListener(() =>
            {
                Constants.Student = student;
                deletePanel.SetActive(true);
                deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = Constants.GetDeleteStudentFromGroupString(student);
            });
            s.GetComponentInChildren<Text>().text  = (student.name);
        }
    }
    
    private void SetActiveStudentsPanel() {
        foreach (Transform child in studentsInGroupList.transform) Destroy(child.gameObject);

        foreach (Transform child in studentsNotInGroupList.transform) Destroy(child.gameObject);
        
        studentsPanel.SetActive(true);
        var studentsInGroup = APIHelper.GetStudentsInGroup(Constants.Group.id);
        var studentsNotInGroup = APIHelper.GetStudentsFromClassroomNotInGroup(Constants.Classroom.id, Constants.Group.id);
        
        AddStudentsToLists(studentsInGroup, studentsNotInGroup);
        ResizeStudentGroupLists();
    }
    
    private void AddStudentsToLists(List<Student> studentsInGroup, List<Student> studentsNotInGroup)
    {
        foreach (var student in studentsInGroup) AddStudentToList(student, studentsInGroupList, true); 
        foreach (var student in studentsNotInGroup) AddStudentToList(student, studentsNotInGroupList, false); 
    }
    
    private void AddStudentToList(Student student, GridLayoutGroup grid, bool isInGroup) {
        var addedInGroup = isInGroup;
        var studentItem = Instantiate(prefabStudentListItem, grid.transform);
        studentItem.transform.Find("Text").GetComponent<Text>().text  = (student.name + " " + student.lastName);
        if (isInGroup) studentItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
        else studentItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.plusSprite;
        
        studentItem.transform.Find("Close").GetComponent<Button>().onClick.AddListener(() => {
            //Constants.Student = student;
            if (addedInGroup) {
                studentItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.plusSprite;
                studentItem.transform.parent = studentsNotInGroupList.transform;
                addedInGroup = false;
                if (isInGroup) _delFromGroup.Add(student);
                else _addToGroup.Remove(student);
            }
            else {
                studentItem.transform.Find("Close").transform.Find("Image").GetComponent<Image>().sprite = Constants.xSprite;
                studentItem.transform.parent = studentsInGroupList.transform;
                addedInGroup = true;
                if (!isInGroup) _addToGroup.Add(student);
                else _delFromGroup.Remove(student);
            }
            ResizeStudentGroupLists();
        });
    }
    
    private void ResizeStudentGroupLists() {
        var height = 95*((studentsInGroupList.transform.childCount + 1) / 2)-15;
        var height2 = 95*((studentsNotInGroupList.transform.childCount + 1) / 2)-15;
        RectTransform rt = studentsInGroupList.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2 (rt.sizeDelta.x, height);
        RectTransform rt2 = studentsNotInGroupList.GetComponent<RectTransform>();
        rt2.sizeDelta = new Vector2 (rt2.sizeDelta.x, height2);
    }
    
    private void ManageStudents() {
        foreach (var student in _addToGroup) {
            var studentGroup = new StudentGroup { groupId = Constants.Group.id, studentId = student.id };
            APIHelper.CreateUpdateStudentGroup(studentGroup);
        }

        foreach (var student in _delFromGroup) APIHelper.DeleteStudentGroup(student.id, Constants.Group.id);

        _delFromGroup = new HashSet<Student>();
        _addToGroup = new HashSet<Student>();
    }
}
