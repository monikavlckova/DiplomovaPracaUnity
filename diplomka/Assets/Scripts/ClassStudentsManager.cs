using System;
using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Button = UnityEngine.UI.Button;

public class ClassStudentsManager : MonoBehaviour
{
    public Text className;
    public GridLayoutGroup studentsLayout;
    public Canvas canvas;
    public Button prefabItem;
    public Button backButton;
    public Button addStudent;
    
    public Button classTasksButton;
    public Button classGroupsButton;
    
    public GameObject editPanel;
    public Button closeEditPanel;
    public Button editButton;
    public Button deleteButton;
    
    public GameObject studentPanel;
    public Button closeStudentPanel;
    public Button saveButton;
    public InputField studentName;
    public InputField studentLastName;
    public InputField studentUserName;
    public InputField studentPassword;
    public TMP_Dropdown dropdown;
    
    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;
    
    private Student _delEditStudent;
    private bool _creatingNew;
    private List<Classroom> classrooms;
    private void Start()
    {
        var students = APIHelper.GetStudentsInClassroom(Constants.ClassroomId);
        AddStudentsToGrid(students);
        var classroom = APIHelper.GetClassroom(Constants.ClassroomId);
        className.text = classroom.name;
        
        classrooms = APIHelper.GetTeachersClassrooms(Constants.UserId);
        dropdown.options.Clear();
        foreach (var c in classrooms)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() {text=c.name});
        }

        backButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Classes"); 
        });
        
        float width = canvas.GetComponent<RectTransform>().rect.width;
        float width3 = (width - 120) / 3;
        studentsLayout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width3, width3+80);
        
        classTasksButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassTasks"));
        classGroupsButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassGroups"));
        
        addStudent.onClick.AddListener(() => {
            _creatingNew = true;
            dropdown.gameObject.SetActive(false);
            saveButton.transform.Find("Text").GetComponent<Text>().text = Constants.SaveButtonTextCreate;
            studentPanel.SetActive(true);
        });
        
        editButton.onClick.AddListener(() =>
        {
            _creatingNew = false;
            saveButton.transform.Find("Text").GetComponent<Text>().text = Constants.SaveButtonTextUpdate;
            studentName.text = _delEditStudent.name;
            studentLastName.text = _delEditStudent.lastName;
            studentUserName.text = _delEditStudent.userName;
            studentPassword.text = _delEditStudent.password;
            dropdown.gameObject.SetActive(true);
            dropdown.value = GetStudentCurrentClassroomIndexInList();
            studentPanel.SetActive(true);
            editPanel.SetActive(false);
        });
        
        saveButton.onClick.AddListener(() =>
        {
            //TODO skontroluj vstup
            var student = new Student
            {
                name = studentName.text,
                lastName = studentLastName.text,
                userName = studentUserName.text,
                password = studentPassword.text,
                classroomId = Constants.ClassroomId
            };

            var method = "PUT";
            if (_creatingNew == false)
            {
                student.classroomId = classrooms[dropdown.value].id;
                student.id = Constants.StudentId;
                method = "POST";
            }

            APIHelper.CreateUpdateStudent(student, method);
            SceneManager.LoadScene("Scenes/ClassStudents");
        });
        
        closeStudentPanel.onClick.AddListener(() => {
            studentPanel.SetActive(false);
        });
        
        closeEditPanel.onClick.AddListener(() => {
            editPanel.SetActive(false);
        });
        
        editPanel.GetComponent<Button>().onClick.AddListener(() => {
            editPanel.SetActive(false);
        });
        
        deleteButton.onClick.AddListener(() =>
        {
            _delEditStudent = APIHelper.GetStudent(Constants.StudentId);
            deletePanel.SetActive(true);
            editPanel.SetActive(false);
            deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = Constants.GetDeleteStudentString(_delEditStudent);
        });
        
        closeDeletePanel.onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });

        deletePanel.GetComponent<Button>().onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });

        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteStudent(Constants.StudentId);
            //TODO zmenit? mam nanovo nacitat? zatial ok
            SceneManager.LoadScene("Scenes/ClassStudents");
        });
    }
    
    private void AddStudentsToGrid(List<Student> list)
    {
        foreach (var student in list)
        {
            AddStudentToGrid(student);
        }
    }

    private void AddStudentToGrid(Student student)
    {
        var s = Instantiate(prefabItem, studentsLayout.transform);
        s.onClick.AddListener(() => {
            Constants.StudentId = student.id;
            SceneManager.LoadScene("Scenes/Student"); 
        });
        var edit = s.transform.Find("Edit").GetComponent<Button>();
        edit.onClick.AddListener(() =>
        {
            _delEditStudent = student;
            Constants.StudentId = student.id;
            editPanel.SetActive(true);
        });
        s.GetComponentInChildren<Text>().text  = (student.name);
    }

    private int GetStudentCurrentClassroomIndexInList()
    {
        for (var i = 0; i < classrooms.Count; i++)
        {
            if (classrooms[i].id == _delEditStudent.classroomId) return i;
        }
        Debug.Log(String.Format("Nenašila sa trieda s id {0}", _delEditStudent.classroomId));
        return -1;
    } 
}
