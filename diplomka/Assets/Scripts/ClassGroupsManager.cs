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
    
    public GameObject deletePanel;
    public Button closeDeletePanel;
    public Button confirmDelete;

    private Group _delGroup;
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
        Vector2 newGroupSize = new Vector2((width - 100) / 2, (width - 100) / 2);
        groupsLayout.GetComponent<GridLayoutGroup>().cellSize = newGroupSize;
        
        classTasksButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassTasks"));
        classStudentsButton.onClick.AddListener(() => SceneManager.LoadScene("Scenes/ClassStudents"));

        addGroup.onClick.AddListener(() => {
            saveButton.transform.Find("Text").GetComponent<Text>().text = "Vytvor";
            _creatingNew = true;
            groupPanel.SetActive(true);
        });
        
        editButton.onClick.AddListener(() =>
        {
            _creatingNew = false;
            saveButton.transform.Find("Text").GetComponent<Text>().text = "Uprav";
            groupPanel.SetActive(true);
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
        
        deleteButton.onClick.AddListener(() =>
        {
            _delGroup = APIHelper.GetGroup(Constants.GroupId);
            deletePanel.SetActive(true);
            deletePanel.transform.Find("Panel").transform.Find("Text").GetComponent<Text>().text = "Naozaj chcete vymazat triedu " + _delGroup.name + "?";
        });
        
        closeDeletePanel.onClick.AddListener(() => {
            deletePanel.SetActive(false);
        });
        
        confirmDelete.onClick.AddListener(() =>
        {
            APIHelper.DeleteGroup(Constants.GroupId);
            //TODO zmenit? mam nanovo nacitat? zatial ok
            SceneManager.LoadScene("Scenes/ClassGroups");
        });
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
            Constants.GroupId = group.id;
            editPanel.SetActive(true);
        });
        g.GetComponentInChildren<Text>().text  = (group.name);
    }
    
}
