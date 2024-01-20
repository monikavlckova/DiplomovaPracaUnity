using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StudentGroupsManager : MonoBehaviour
{
    public Text headline;
    public GridLayoutGroup layout;
    public Button studentsTasksButton;
    public Button prefabItem;
    public Button back;
    public Canvas canvas;
    public Button addGroups;

    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;
    
    public GameObject groupsPanel;//TODO groups panel zoznam existujucih
    
    private string sceneName = "StudentGroups";

    private void Start()
    {
        var width = canvas.GetComponent<RectTransform>().rect.width;
        var width3 = (width - 120) / 3;
        layout.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width3, width3+80);
        
        prefabItem.transform.Find("Edit").GetComponent<Image>().sprite = Constants.xSprite;
        
        var student = Constants.Student;
        var groups = APIHelper.GetStudentsGroups(Constants.Student.id);
        AddGroupsToGrid(groups);
        headline.text = student.name + " " + student.lastName;
        
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/ClassStudents"); 
        });
        
        studentsTasksButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/StudentTasks"));
        
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
        
        addGroups.onClick.AddListener(() =>
        {
            //TODO studentsPanel.SetActive(true);
        });
    }

    public void AddGroupsToGrid(List<Group> list)
    {
        foreach (var group in list)
        {
            var s = Instantiate(prefabItem, layout.transform);
            s.onClick.AddListener(() => 
            {
                Constants.Group = group;
                Constants.LastSceneName = sceneName;
                SceneManager.LoadScene("Scenes/GroupTasks"); 
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
    

}
