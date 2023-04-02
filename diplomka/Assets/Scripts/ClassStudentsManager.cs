using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    
    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;
    
    private Student _delStudent;
    private bool _creatingNew;
    private void Start()
    {
        var students = APIHelper.GetStudentsInClassroom(Constants.ClassroomId);
        AddStudentsToGrid(students);
        var classroom = APIHelper.GetClassroom(Constants.ClassroomId);
        className.text = classroom.name;
        
        backButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/Classes"); 
        });
        
        float width = canvas.GetComponent<RectTransform>().rect.width;
        Vector2 newStudentSize = new Vector2((width - 120) / 3, (width - 120) / 3);
        studentsLayout.GetComponent<GridLayoutGroup>().cellSize = newStudentSize;
        
        classTasksButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassTasks"));
        classGroupsButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassGroups"));
        
        addStudent.onClick.AddListener(() => {
            saveButton.transform.Find("Text").GetComponent<Text>().text = "Vytvor";
            _creatingNew = true;
            studentPanel.SetActive(true);
        });
        
        editButton.onClick.AddListener(() =>
        {
            _creatingNew = false;
            saveButton.transform.Find("Text").GetComponent<Text>().text = "Uprav";
            studentPanel.SetActive(true);
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
        
        deleteButton.onClick.AddListener(() =>
        {
            _delStudent = APIHelper.GetStudent(Constants.StudentId);
            deletePanel.SetActive(true);
            deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = "Študent " + _delStudent.name + " " + _delStudent.lastName + "bude nevratne odstránený";
        });
        
        closeDeletePanel.onClick.AddListener(() => {
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
            Constants.StudentId = student.id;
            editPanel.SetActive(true);
        });
        s.GetComponentInChildren<Text>().text  = (student.name);
    }
}
