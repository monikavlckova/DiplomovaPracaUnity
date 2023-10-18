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
        var classes = APIHelper.GetTeachersClassrooms(Constants.UserId); 
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
            //TODO skontroluj vstup
            var classroom = new Classroom
            {
                name = className.text,
                teacherId = Constants.UserId
            };
            var method = "PUT";
            if (_creatingNew == false)
            {
                classroom.id = Constants.ClassroomId;
                method = "POST";
            }
            APIHelper.CreateUpdateClassroom(classroom, method);
            classPanel.SetActive(false);
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
            _delEditClassroom = APIHelper.GetClassroom(Constants.ClassroomId);
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
            APIHelper.DeleteClassroom(Constants.ClassroomId);
            //TODO zmenit? mam nanovo nacitat? zatial ok
            SceneManager.LoadScene("Scenes/Classes");
        });
    }

    private void AddClassroomsToGrid(List<Classroom> list)
    {
        foreach (var classroom in list)
        {
            AddClassroomToGrid(classroom);
        }
    }

    private void AddClassroomToGrid(Classroom classroom)
    {
        var c = Instantiate(prefabItem, classroomsLayout.transform);
        c.onClick.AddListener(() => {
            Constants.ClassroomId = classroom.id;
            SceneManager.LoadScene("Scenes/ClassStudents"); 
        });
        var edit = c.transform.Find("Edit").GetComponent<Button>();
        edit.onClick.AddListener(() =>
        {
            _delEditClassroom = classroom;
            Constants.ClassroomId = classroom.id;
            editPanel.SetActive(true);
        });
        c.GetComponentInChildren<Text>().text  = (classroom.name);
    }
}
