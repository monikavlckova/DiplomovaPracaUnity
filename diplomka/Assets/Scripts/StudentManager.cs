using System.Collections.Generic;
using DbClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StudentManager : MonoBehaviour
{
    public Text headline;
    public GridLayoutGroup layout;
    public Button prefabItem;
    public Button back;

    private void Start()
    {
        //var student = APIHelper.GetStudent(Constants.StudentId);
        var student = Constants.Student;
        var tasks = APIHelper.GetStudentsTasks(Constants.Student.id);
        AddTasksToGrid(tasks);
        headline.text = student.name + " " + student.lastName;
        
        back.onClick.AddListener(() => {
            SceneManager.LoadScene("Scenes/ClassStudents"); 
        });
    }

    public void AddTasksToGrid(List<Taskk> list)
    {
        foreach (var task in list)
        {
            var s = Instantiate(prefabItem, layout.transform);
            s.onClick.AddListener(() => 
            {
                Constants.Taskk = task;
                Constants.LastSceneName = "Student";
                SceneManager.LoadScene("Scenes/Task"); 
            });
            s.GetComponentInChildren<Text>().text  = (task.name);
        }
    }
    

}
