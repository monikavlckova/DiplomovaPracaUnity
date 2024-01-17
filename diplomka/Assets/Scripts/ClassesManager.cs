using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClassesManager : MonoBehaviour
{
    public Button logoutButton;
    public VerticalLayoutGroup classroomsLayout;
    public Button prefabItem;
    public Button addClass;
    public Button profile;
    
    public GameObject editPanel;
    public Button closeEditPanel;
    public Button editButton;
    public Button deleteButton;
    
    public GameObject classPanel;
    public Button closeClassPanel;
    public Button saveButton;
    public InputField className;
    
    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;
    
    private Classroom _delEditClassroom;
    private bool _creatingNew;
    private void Start()
    {
        var classes = APIHelper.GetTeachersClassrooms(Constants.User.id); 
        AddClassroomsToGrid(classes);
        
        logoutButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Login"); 
        });
        
        addClass.onClick.AddListener(() =>
        {
            _creatingNew = true;
            saveButton.transform.Find("Text").GetComponent<Text>().text = Constants.SaveButtonTextCreate;
            classPanel.SetActive(true);
        });
        
        profile.onClick.AddListener(() =>
        {
            Constants.LastSceneName = "Classes";
            SceneManager.LoadScene("Scenes/Profile");
        });

        editButton.onClick.AddListener(() =>
        {
            _creatingNew = false;
            saveButton.transform.Find("Text").GetComponent<Text>().text = Constants.SaveButtonTextUpdate;
            className.text = _delEditClassroom.name;
            classPanel.SetActive(true);
            editPanel.SetActive(false);
        });
        
        saveButton.onClick.AddListener(() =>
        {
            if (!AreValidValues()) return;
           
            var classroom = new Classroom
            {
                name = className.text,
                teacherId = Constants.User.id
            };
            var method = "PUT";
            if (_creatingNew == false)
            {
                classroom.id = Constants.Classroom.id;
                method = "POST";
            }
            APIHelper.CreateUpdateClassroom(classroom, method);
            classPanel.SetActive(false);
            //TODO zmenit? mam nanovo nacitat? zatial ok
            SceneManager.LoadScene("Scenes/Classes");
        });
        
        closeClassPanel.onClick.AddListener(() => {
            classPanel.SetActive(false);
        });
        
        classPanel.GetComponent<Button>().onClick.AddListener(() => {
            classPanel.SetActive(false);
        });
        
        closeEditPanel.onClick.AddListener(() => {
            editPanel.SetActive(false);
        });
        
        editPanel.GetComponent<Button>().onClick.AddListener(() => {
            editPanel.SetActive(false);
        });
        
        deleteButton.onClick.AddListener(() =>
        {
            _delEditClassroom = Constants.Classroom;
            deletePanel.SetActive(true);
            editPanel.SetActive(false);
            deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = Constants.GetDeleteClassroomString(_delEditClassroom);
        });
        
        closeDeletePanel.onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });
        
        deletePanel.GetComponent<Button>().onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });
        
        confirmDelete.onClick.AddListener(() =>
        {
            SwitchClassroomTasksToStudentTasks();
            SwitchGroupTasksToStudentTasks();
            APIHelper.DeleteClassroom(Constants.Classroom.id);
            //TODO zmenit? mam nanovo nacitat? zatial ok
            SceneManager.LoadScene("Scenes/Classes");
        });
    }

    private static void SwitchClassroomTasksToStudentTasks()
    {
        var students = APIHelper.GetStudentsInClassroom(Constants.Classroom.id);
        var classroomTasks = APIHelper.GetTasksInClassroom(Constants.Classroom.id);
        foreach (var student in students) {
            foreach (var task in classroomTasks) {
                Debug.Log(student.id + " class " + task.id);
                var studentTask = new StudentTask { studentId = student.id, taskkId = task.id };
                APIHelper.CreateUpdateStudentTask(studentTask);
            }
        }
    }
    
    private static void SwitchGroupTasksToStudentTasks()
    {
        var groups = APIHelper.GetGroupsInClassroom(Constants.Classroom.id);
        foreach (var group in groups) {
            var studentsInGroup = APIHelper.GetStudentsInGroup(group.id);
            var groupTasks = APIHelper.GetGroupsTasks(group.id);
            foreach (var student in studentsInGroup) {
                foreach (var task in groupTasks) {
                    Debug.Log(student.id + " group " + task.id);
                    var studentTask = new StudentTask { studentId = student.id, taskkId = task.id };
                    APIHelper.CreateUpdateStudentTask(studentTask);
                }
            }
        }
    }

    private void AddClassroomsToGrid(List<Classroom> list)
    {
        if (list  == null) return;
        
        foreach (var classroom in list)
        {
            AddClassroomToGrid(classroom);
        }
    }

    private void AddClassroomToGrid(Classroom classroom)
    {
        var c = Instantiate(prefabItem, classroomsLayout.transform);
        c.onClick.AddListener(() => {
            Constants.Classroom = classroom;
            Constants.LastSceneName = "Classes";
            SceneManager.LoadScene("Scenes/ClassStudents"); 
        });
        var edit = c.transform.Find("Edit").GetComponent<Button>();
        edit.onClick.AddListener(() =>
        {
            _delEditClassroom = classroom;
            Constants.Classroom = classroom;
            editPanel.SetActive(true);
        });
        c.GetComponentInChildren<Text>().text  = (classroom.name);
    }
    
    private bool AreValidValues()
    {
        var nameUnderline = className.transform.Find("underline");
        nameUnderline.gameObject.SetActive(false);
        var valid = true;

        if (className.text.Length < Constants.MinimalClassroomNameLength)
        {
            nameUnderline.gameObject.SetActive(true);
            nameUnderline.GetComponent<Text>().text = Constants.WrongClassroomNameFormatMessage;
            valid = false;
        }
        
        return valid;
    }
}
