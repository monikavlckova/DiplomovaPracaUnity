using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using UnityEngine;
using DbClasses;
using Newtonsoft.Json;
using Task = DbClasses.Task;


public static class APIHelper
{
    
    public static string GetDataObjectJson(string path)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return json;
    }

    
    public static string PutPostDataObjectJson(string path, string jsonData, int id = 0, string method = "PUT")
    {
        if (method == "POST")
        {
            path += "/" + id;
        }
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
        
        var data = Encoding.UTF8.GetBytes(jsonData);

        request.Method = method;
        request.ContentType = "application/json";
        request.ContentLength = data.Length;

        var newStream = request.GetRequestStream();
        newStream.Write(data, 0, data.Length);
        newStream.Close();
        
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return json;

        /*try {
            using (WebResponse response = (WebResponse)request.GetResponse())
            {
                Debug.Log("Won't get here");
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string json = reader.ReadToEnd();
                return json;
            }
        }
        catch (WebException e)
        {
            using (WebResponse response = e.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse) response;
                Debug.Log("Error code: " + httpResponse.StatusCode + "++++++");
                using (Stream dataa = response.GetResponseStream())
                using (var reader = new StreamReader(dataa))
                {
                    string text = reader.ReadToEnd();
                    Debug.Log("------------- " + text);
                }
            }
        }
        return ""*/
        
    }
    
    private static string DeleteDataObjectJson(string path)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
        
        request.Method = "DELETE";
        request.ContentType = "application/json";
        
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return json;
    }
    
    public static List<Classroom> GetAllClassrooms()
    {
        return JsonConvert.DeserializeObject<List<Classroom>>(GetDataObjectJson(Constants.ClassroomsEndpointUrl));
    }
    
    public static Classroom GetClassroom(int id)
    {
        return JsonUtility.FromJson<Classroom>(GetDataObjectJson(Constants.ClassroomsEndpointUrl + "/" + id));
    }

    public static List<Group> GetGroupsInClassroom(int id)
    {
        return JsonConvert.DeserializeObject<List<Group>>(GetDataObjectJson(Constants.ClassroomGetGroupsEndpointUrl + "/" + id));
    }

    public static List<Student> GetStudentsInClassroom(int id)
    {
        return JsonConvert.DeserializeObject<List<Student>>(GetDataObjectJson(Constants.ClassroomGetStudentsEndpointUrl + "/" + id));
    }
    
    public static List<Task> GetTasksInClassroom(int id)
    {
        return JsonConvert.DeserializeObject<List<Task>>(GetDataObjectJson(Constants.ClassroomGetTasksEndpointUrl + "/" + id));
    }

    public static List<Group> GetAllGroups()
    {
        return JsonConvert.DeserializeObject<List<Group>>(GetDataObjectJson(Constants.GroupsEndpointUrl));
    }

    public static Group GetGroup(int id)
    {
        return JsonUtility.FromJson<Group>(GetDataObjectJson(Constants.GroupsEndpointUrl + "/" + id));
    }

    public static List<Student> GetStudentsInGroup(int id) 
    {
        return JsonConvert.DeserializeObject<List<Student>>(GetDataObjectJson(Constants.GroupGetStudentsEndpointUrl + "/" + id));
    }
    
    public static List<Student> GetStudentsNotInGroup(int groupId, int classroomId) 
    {
        return JsonConvert.DeserializeObject<List<Student>>(GetDataObjectJson(Constants.GroupGetStudentsEndpointUrl + "/" + groupId + "/" + classroomId));
    }

    public static List<Task> GetGroupsTasks()
    {
        return JsonConvert.DeserializeObject<List<Task>>(GetDataObjectJson(Constants.GroupGetTasksEndpointUrl));
    }

    public static List<Student> GetAllStudents()
    {
        return JsonConvert.DeserializeObject<List<Student>>(GetDataObjectJson(Constants.StudentsEndpointUrl));
    }

    public static Student GetStudent(int id)
    {
        return JsonUtility.FromJson<Student>(GetDataObjectJson(Constants.StudentsEndpointUrl + "/" + id));
    }

    public static List<Group> GetStudentsGroups(int id)
    {
        return JsonConvert.DeserializeObject<List<Group>>(GetDataObjectJson(Constants.StudentGetGroupsEndpointUrl + "/" + id));
    }

    public static List<Task> GetStudentsTasks(int id)
    {
        return JsonConvert.DeserializeObject<List<Task>>(GetDataObjectJson(Constants.StudentGetTasksEndpointUrl + "/" + id));
    }

    public static Task GetTask(int id)
    {
        return JsonUtility.FromJson<Task>(GetDataObjectJson(Constants.TasksEndpointUrl + "/" + id));
    }

    public static List<Teacher> GetAllTeachers()
    {
        return JsonConvert.DeserializeObject<List<Teacher>>(GetDataObjectJson(Constants.TeachersEndpointUrl));
    }

    public static Teacher GetTeacher(int id)
    {
        return JsonUtility.FromJson<Teacher>(GetDataObjectJson(Constants.TeachersEndpointUrl + "/" + id));
    }

    public static List<Classroom> GetTeachersClassrooms(int id)
    {
        return JsonConvert.DeserializeObject<List<Classroom>>(GetDataObjectJson(Constants.TeacherGetClassroomsEndpointUrl + "/" + id));
    }

    public static List<User> GetAllUsers()
    {
        return JsonConvert.DeserializeObject<List<User>>(GetDataObjectJson(Constants.UsersEndpointUrl));
    }

    public static User GetUser(int id)
    {
        return JsonUtility.FromJson<User>(GetDataObjectJson(Constants.UsersEndpointUrl + "/" + id));
    }


    public static string CreateUpdateClassroom(Classroom classroom, string method="PUT")
    {
        string json = JsonConvert.SerializeObject(classroom);
        return PutPostDataObjectJson(Constants.ClassroomsEndpointUrl, json, classroom.id, method);
    }
    
    public static string CreateUpdateClassroomTask(ClassroomTask classroomTask, string method="PUT")
    {
        string json = JsonConvert.SerializeObject(classroomTask);
        return PutPostDataObjectJson(Constants.ClassroomTaskEndpointUrl, json, classroomTask.id, method);
    }

    public static string CreateUpdateGroup(Group group, string method="PUT")
    {
        string json = JsonConvert.SerializeObject(group);
        return PutPostDataObjectJson(Constants.GroupsEndpointUrl, json, group.id, method);
    }
    
    public static string CreateUpdateGroupTask(GroupTask groupTask, string method="PUT")
    {
        string json = JsonConvert.SerializeObject(groupTask);
        return PutPostDataObjectJson(Constants.GroupTaskEndpointUrl, json, groupTask.id, method); 
    }
    
    public static string CreateUpdateStudent(Student student, string method="PUT")
    {
        string json = JsonConvert.SerializeObject(student);
        return PutPostDataObjectJson(Constants.StudentsEndpointUrl, json, student.id, method);
    }
    
    public static string CreateUpdateStudentGroup(StudentGroup studentGroup, string method="PUT")
    {
        string json = JsonConvert.SerializeObject(studentGroup);
        return PutPostDataObjectJson(Constants.StudentGroupEndpointUrl, json, studentGroup.id, method);
    }
    
    public static string CreateUpdateStudentTask(StudentTask studentTask, string method="PUT")
    {
        string json = JsonConvert.SerializeObject(studentTask);
        return PutPostDataObjectJson(Constants.StudentTaskEndpointUrl, json, studentTask.id, method);
    }
    
    public static string CreateUpdateTask(Task task, string method="PUT")
    {
        string json = JsonConvert.SerializeObject(task);
        return PutPostDataObjectJson(Constants.TasksEndpointUrl, json, task.id, method);
    }
    
    public static string CreateUpdateTeacher(Teacher teacher, string method="PUT")
    {
        string json = JsonConvert.SerializeObject(teacher);
        return PutPostDataObjectJson(Constants.TeachersEndpointUrl, json, teacher.id, method);
    }
    
    public static string CreateUpdateUser(User user, string method="PUT")
    {
        string json = JsonConvert.SerializeObject(user);
        return PutPostDataObjectJson(Constants.UsersEndpointUrl, json, user.id, method);
    }
    
    public static string DeleteClassroom(int id)
    {
        return DeleteDataObjectJson(Constants.ClassroomsEndpointUrl + "/" + id);
    }
    
    public static string DeleteClassroomTask(int id)
    {
        return DeleteDataObjectJson(Constants.ClassroomTaskEndpointUrl + "/" + id);
    }

    public static string DeleteGroup(int id)
    {
        return DeleteDataObjectJson(Constants.GroupsEndpointUrl + "/" + id);
    }
    
    public static string DeleteGroupTask(int id)
    {
        return DeleteDataObjectJson(Constants.GroupTaskEndpointUrl + "/" + id); 
    }
    
    public static string DeleteStudent(int id)
    {
        return DeleteDataObjectJson(Constants.StudentsEndpointUrl + "/" + id);
    }
    
    public static string DeleteStudentGroup(int id)
    {
        return DeleteDataObjectJson(Constants.StudentGroupEndpointUrl + "/" + id);
    }
    
    public static string DeleteStudentTask(int id)
    {
        return DeleteDataObjectJson(Constants.StudentTaskEndpointUrl + "/" + id);
    }
    
    public static string DeleteTask(int id)
    {
        return DeleteDataObjectJson(Constants.TasksEndpointUrl + "/" + id);
    }
    
    public static string DeleteTeacher(int id)
    {
        return DeleteDataObjectJson(Constants.TeachersEndpointUrl + "/" + id);
    }
    
    public static string DeleteUser(int id)
    {
        return DeleteDataObjectJson(Constants.UsersEndpointUrl + "/" + id);
    }

}