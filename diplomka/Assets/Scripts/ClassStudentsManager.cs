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
    private string sceneName = "ClassStudents";
    
    private void Start()
    {
        float width = canvas.GetComponent<RectTransform>().rect.width;
        float width3 = (width - 120) / 3;
        studentsLayout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width3, width3+80);
        
        prefabItem.transform.Find("Edit").GetComponent<Image>().sprite = Constants.dotsSprite;
        
        var students = APIHelper.GetStudentsInClassroom(Constants.Classroom.id);
        AddStudentsToGrid(students);
        var classroom = Constants.Classroom;
        className.text = classroom.name;
        
        classrooms = APIHelper.GetTeachersClassrooms(Constants.User.id);
        dropdown.options.Clear();
        foreach (var c in classrooms)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData() {text=c.name});
        }

        backButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Classes"); 
        });

        classTasksButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassTasks"));
        classGroupsButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassGroups"));
        
        addStudent.onClick.AddListener(() => {
            _creatingNew = true;
            dropdown.gameObject.SetActive(false);
            saveButton.transform.Find("Text").GetComponent<Text>().text = Constants.SaveButtonTextCreate;
            studentPanel.SetActive(true);
            studentName.text = "";
            studentLastName.text = "";
            studentUserName.text = "";
            studentPassword.text = "";
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
            if(!AreValidValues()) return;
            var student = new Student
            {
                classroomId = Constants.Classroom.id,
                name = studentName.text,
                lastName = studentLastName.text,
                userName = studentUserName.text,
                password = studentPassword.text
            };

            var method = "PUT";
            if (_creatingNew == false)
            {
                student.classroomId = classrooms[dropdown.value].id;
                student.id = Constants.Student.id;
                method = "POST";
            }

            if (classrooms[dropdown.value].id != Constants.Classroom.id)
            {
                DeleteStudentFromGroups(Constants.Student.id);
            }

            APIHelper.CreateUpdateStudent(student, method);
            Constants.mySceneManager.Reload(sceneName);
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
            //_delEditStudent = APIHelper.GetStudent(Constants.StudentId);
            _delEditStudent = Constants.Student;
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
            APIHelper.DeleteStudent(Constants.Student.id);
            Constants.mySceneManager.Reload(sceneName);
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
        if (student.imagePath is not null) { s.transform.Find("Image").GetComponent<Image>().sprite = Constants.GetSprite(student.imagePath); }
        s.onClick.AddListener(() => {
            Constants.Student = student;
            Constants.LastSceneName = sceneName;
            SceneManager.LoadScene("Scenes/StudentTasks"); 
        });
        var edit = s.transform.Find("Edit").GetComponent<Button>();
        edit.onClick.AddListener(() =>
        {
            _delEditStudent = student;
            Constants.Student = student;
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

    private void DeleteStudentFromGroups(int studentId)
    {
        var groups = APIHelper.GetStudentsGroups(studentId);
        foreach (var group in groups)
        {
            APIHelper.DeleteStudentGroup(studentId, group.id);
        }
    }
    
    private bool AreValidValues()
    {
        var userNameUnderline = studentUserName.transform.Find("underline");
        var firstNameUnderline = studentName.transform.Find("underline");
        var lastNameUnderline = studentLastName.transform.Find("underline");
        var passwordUnderline = studentPassword.transform.Find("underline");
        userNameUnderline.gameObject.SetActive(false);
        firstNameUnderline.gameObject.SetActive(false);
        lastNameUnderline.gameObject.SetActive(false);
        passwordUnderline.gameObject.SetActive(false);
        var valid = true;

        if (studentName.text.Length < Constants.MinimalFirstNameLength)
        {
            firstNameUnderline.gameObject.SetActive(true);
            firstNameUnderline.GetComponent<Text>().text = Constants.WrongFirstNameFormatMessage;
            valid = false;
        }

        if (studentLastName.text.Length < Constants.MinimalLastNameLength)
        {
            lastNameUnderline.gameObject.SetActive(true);
            lastNameUnderline.GetComponent<Text>().text = Constants.WrongLastNameFormatMessage;
            valid = false;
        }

        if (studentUserName.text.Length < Constants.MinimalUserNameLength)
        {
            userNameUnderline.gameObject.SetActive(true);
            userNameUnderline.GetComponent<Text>().text = Constants.WrongUserNameFormatMessage;
            valid = false;
        }
        else
        {
            var user = APIHelper.GetTeacherByUserName(studentUserName.text);
            if (user is not null && user.id != Constants.Student.id)
            {
                userNameUnderline.gameObject.SetActive(true);
                userNameUnderline.GetComponent<Text>().text = Constants.WrongUserNameAlreadyExistMessage;
                valid = false;
            }
        }
        
        if (studentPassword.text.Length < Constants.MinimalPasswordLength)
        {
            passwordUnderline.gameObject.SetActive(true);
            passwordUnderline.GetComponent<Text>().text = Constants.WrongPasswordFormatMessage;
            valid = false;
        }

        return valid;
    }
}
