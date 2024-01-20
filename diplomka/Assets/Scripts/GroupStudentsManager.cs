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
    
    public GameObject studentsPanel;//TODO students panel zoznam existujucih
    public Button closeStudentsPanel;
    public Button saveButton;
    public GridLayoutGroup studentsInGroupList;
    public GridLayoutGroup studentsNotInGroupList;
    public GameObject prefabStudentListItem;
    
    private string sceneName = "GroupStudents";

    private void Start()
    {
        var width = canvas.GetComponent<RectTransform>().rect.width;
        var width3 = (width - 120) / 3;
        layout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width3, width3+80);

        prefabItem.transform.Find("Edit").GetComponent<Image>().sprite = Constants.xSprite;
        
        var group = Constants.Group;
        var students = APIHelper.GetStudentsInGroup(Constants.Group.id);
        AddStudentsToGrid(students);
        headline.text = group.name;
        
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/ClassGroups"); 
        });
        
        groupTasksButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/GroupTasks"));
        
        closeDeletePanel.onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });

        deletePanel.GetComponent<Button>().onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });

        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteStudentGroup(Constants.Student.id, Constants.Group.id);
            Constants.mySceneManager.Reload(sceneName);
        });
        
        addStudents.onClick.AddListener(() =>
        {
            //TODO studentsPanel.SetActive(true);
        });
    }
    
    public void AddStudentsToGrid(List<Student> list)
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
    

}
