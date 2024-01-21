using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using UnityEngine;
using DbClasses;
using Newtonsoft.Json;


public static class APIHelper
{
    private static string GetDataObjectJson(string path)
    {
        try
        {
            var request = (HttpWebRequest)WebRequest.Create(path);
            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var json = reader.ReadToEnd();
            return json;
        }
        catch (WebException ex)
        {
            Debug.Log(ex.Message);
        }

        return "";
    }

    private static string PutPostDataObjectJson(string path, string jsonData, string method = "PUT", int id = 0)
    {
        if (method == "POST")  path += "/" + id;
        
        var request = (HttpWebRequest)WebRequest.Create(path);
        
        var data = Encoding.UTF8.GetBytes(jsonData);
        Debug.Log(data);
        request.Method = method;
        request.ContentType = "application/json";
        request.ContentLength = data.Length;
        try
        {
            var newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }
        
        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //StreamReader reader = new StreamReader(response.GetResponseStream());
        //var json = reader.ReadToEnd();
        //return json;

        try
        {
            using var response = request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var json = reader.ReadToEnd();
            return json;
        }
        catch (WebException e)
        {
            using var response = e.Response;
            var httpResponse = (HttpWebResponse) response;
            Debug.Log("Error code: " + httpResponse.StatusCode + "++++++");
            using Stream dataa = response.GetResponseStream();
            using var reader = new StreamReader(dataa);
            var text = reader.ReadToEnd();
            Debug.Log("------------- " + text);
        }

        return "";

    }
    
    private static string DeleteDataObjectJson(string path)
    {
        var request = (HttpWebRequest)WebRequest.Create(path);
        
        request.Method = "DELETE";
        request.ContentType = "application/json";
        
        var response = (HttpWebResponse)request.GetResponse();
        var reader = new StreamReader(response.GetResponseStream());
        var json = reader.ReadToEnd();
        return json;
    }

    public static Classroom GetClassroom(int id)
    {
        return JsonUtility.FromJson<Classroom>(GetDataObjectJson(Constants.ClassroomsEndpointUrl + "/" + id));
    }

    public static List<Classroom> GetAllClassrooms()
    {
        return JsonConvert.DeserializeObject<List<Classroom>>(GetDataObjectJson(Constants.ClassroomsEndpointUrl));
    }
    
    public static List<Classroom> GetTeachersClassrooms(int teacherId)
    {
        return JsonConvert.DeserializeObject<List<Classroom>>(GetDataObjectJson(Constants.ClassroomsGetByTeacherIdEndpointUrl + "/" + teacherId));
    }
    
    public static ClassroomTask GetClassroomTask(int classroomId, int taskId)
    {
        return JsonUtility.FromJson<ClassroomTask>(GetDataObjectJson(Constants.ClassroomTaskEndpointUrl + "/" + classroomId + "/" + taskId));
    }

    public static Group GetGroup(int id)
    {
        return JsonUtility.FromJson<Group>(GetDataObjectJson(Constants.GroupsEndpointUrl + "/" + id));
    }
    
    public static List<Group> GetAllGroups()
    {
        return JsonConvert.DeserializeObject<List<Group>>(GetDataObjectJson(Constants.GroupsEndpointUrl));
    }
    
    public static List<Group> GetGroupsInClassroom(int classroomId)
    {
        return JsonConvert.DeserializeObject<List<Group>>(GetDataObjectJson(Constants.GroupsGetByClassroomIdEndpointUrl + "/" + classroomId));
    }
    
    public static List<Group> GetStudentsGroups(int studentId)
    {
        return JsonConvert.DeserializeObject<List<Group>>(GetDataObjectJson(Constants.GroupsGetByStudentIdEndpointUrl + "/" + studentId));
    }

    public static List<Group> GetGroupsFromInClassroomNotInStudent(int classroomId, int studentId)
    {
        return JsonConvert.DeserializeObject<List<Group>>(GetDataObjectJson(Constants.GroupsGetByClassroomIdNotInStudentIdEndpointUrl + "/" + classroomId + "/" + studentId));
    }
    public static GroupTask GetGroupTask(int groupId, int taskId)
    {
        return JsonUtility.FromJson<GroupTask>(GetDataObjectJson(Constants.GroupTaskEndpointUrl + "/" + groupId + "/" + taskId));
    }
    
    public static Student GetStudent(int id)
    {
        return JsonUtility.FromJson<Student>(GetDataObjectJson(Constants.StudentsEndpointUrl + "/" + id));
    }
    
    public static List<Student> GetAllStudents()
    {
        return JsonConvert.DeserializeObject<List<Student>>(GetDataObjectJson(Constants.StudentsEndpointUrl));
    }
    
    public static List<Student> GetStudentsInGroup(int groupId) 
    {
        return JsonConvert.DeserializeObject<List<Student>>(GetDataObjectJson(Constants.StudentsGetByGroupIdEndpointUrl + "/" + groupId));
    }
    
    public static List<Student> GetStudentsFromClassroomNotInGroup(int classroomId, int groupId) 
    {
        return JsonConvert.DeserializeObject<List<Student>>(GetDataObjectJson(Constants.StudentsGetByClassroomIdNotInGroupIdEndpointUrl + "/" + classroomId + "/" + groupId));
    }

    public static List<Student> GetStudentsInClassroom(int classroomId)
    {
        return JsonConvert.DeserializeObject<List<Student>>(GetDataObjectJson(Constants.StudentsGetByClassroomIdEndpointUrl + "/" + classroomId));
    }
    
    public static StudentGroup GetStudentGroup(int studentId, int groupId)
    {
        return JsonUtility.FromJson<StudentGroup>(GetDataObjectJson(Constants.StudentGroupEndpointUrl + "/" + studentId + "/" + groupId));
    }
    
    public static StudentTask GetStudentTask(int studentId, int taskId)
    {
        return JsonUtility.FromJson<StudentTask>(GetDataObjectJson(Constants.StudentTaskEndpointUrl + "/" + studentId + "/" + taskId));
    }
    
    public static Taskk GetTask(int id)
    {
        return JsonUtility.FromJson<Taskk>(GetDataObjectJson(Constants.TasksEndpointUrl + "/" + id));
    }
    
    public static List<Taskk> GetTasksInClassroom(int classroomId)
    {
        return JsonConvert.DeserializeObject<List<Taskk>>(GetDataObjectJson(Constants.TasksGetByClassroomIdEndpointUrl + "/" + classroomId));
    }

    public static List<Taskk> GetGroupsTasks(int groupId)
    {
        return JsonConvert.DeserializeObject<List<Taskk>>(GetDataObjectJson(Constants.TasksGetByGroupIdEndpointUrl + "/" + groupId));
    }

    public static List<Taskk> GetStudentsTasks(int studentId)
    {
        return JsonConvert.DeserializeObject<List<Taskk>>(GetDataObjectJson(Constants.TasksGetByStudentIdEndpointUrl + "/" + studentId));
    }

    public static List<Taskk> GetTasksFromTeacherNotInGroup(int teacherId, int groupId)
    {
        return JsonConvert.DeserializeObject<List<Taskk>>(GetDataObjectJson(Constants.TasksGetByTeacherIdNotInGroupIdEndpointUrl + "/" + teacherId + "/" + groupId));
    }
    
    public static List<Taskk> GetTasksFromTeacherNotInStudent(int teacherId, int studentId)
    {
        return JsonConvert.DeserializeObject<List<Taskk>>(GetDataObjectJson(Constants.TasksGetByTeacherIdNotInStudentIdEndpointUrl + "/" + teacherId + "/" + studentId));
    }

    public static Teacher GetTeacher(int id)
    {
        return JsonUtility.FromJson<Teacher>(GetDataObjectJson(Constants.TeachersEndpointUrl + "/" + id));
    }
    
    public static Teacher GetTeacherByEmail(string email)
    {
        return JsonUtility.FromJson<Teacher>(GetDataObjectJson(Constants.TeacherGetByEmailEndpointUrl + "/" + email));
    }
    
    public static Teacher GetTeacherByUserName(string userName)
    {
        return JsonUtility.FromJson<Teacher>(GetDataObjectJson(Constants.TeacherGetByUserNameEndpointUrl + "/" + userName));
    }
    
    public static Teacher GetTeacherByLogin(string userName, string password)
    {
        return JsonUtility.FromJson<Teacher>(GetDataObjectJson(Constants.TeacherGetByLoginEndpointUrl + "/" + userName + "/" + password));
    }
    
    public static List<Teacher> GetAllTeachers()
    {
        return JsonConvert.DeserializeObject<List<Teacher>>(GetDataObjectJson(Constants.TeachersEndpointUrl));
    }

    /*public static List<User> GetAllUsers()
    {
        return JsonConvert.DeserializeObject<List<User>>(GetDataObjectJson(Constants.UsersEndpointUrl));
    }

    public static User GetUser(int id)
    {
        return JsonUtility.FromJson<User>(GetDataObjectJson(Constants.UsersEndpointUrl + "/" + id));
    }*/


    public static string CreateUpdateClassroom(Classroom classroom, string method="PUT")
    {
        var json = JsonConvert.SerializeObject(classroom);
        return PutPostDataObjectJson(Constants.ClassroomsEndpointUrl, json, method, classroom.id);
    }
    
    public static string CreateUpdateClassroomTask(ClassroomTask classroomTask, string method="PUT")
    {
        var json = JsonConvert.SerializeObject(classroomTask);
        return PutPostDataObjectJson(Constants.ClassroomTaskEndpointUrl, json, method);
    }

    public static string CreateUpdateGroup(Group group, string method="PUT")
    {
        var json = JsonConvert.SerializeObject(group);
        return PutPostDataObjectJson(Constants.GroupsEndpointUrl, json, method, group.id);
    }
    
    public static string CreateUpdateGroupTask(GroupTask groupTask, string method="PUT")
    {
        var json = JsonConvert.SerializeObject(groupTask);
        return PutPostDataObjectJson(Constants.GroupTaskEndpointUrl, json, method); 
    }
    
    public static string CreateUpdateStudent(Student student, string method="PUT")
    {
        var json = JsonConvert.SerializeObject(student);
        return PutPostDataObjectJson(Constants.StudentsEndpointUrl, json, method, student.id);
    }
    
    public static string CreateUpdateStudentGroup(StudentGroup studentGroup, string method="PUT")
    {
        var json = JsonConvert.SerializeObject(studentGroup);
        return PutPostDataObjectJson(Constants.StudentGroupEndpointUrl, json, method);
    }
    
    public static string CreateUpdateStudentTask(StudentTask studentTask, string method="PUT")
    {
        var json = JsonConvert.SerializeObject(studentTask);
        return PutPostDataObjectJson(Constants.StudentTaskEndpointUrl, json, method);
    }
    
    public static string CreateUpdateTask(Taskk taskk, string method="PUT")
    {
        var json = JsonConvert.SerializeObject(taskk);
        return PutPostDataObjectJson(Constants.TasksEndpointUrl, json, method, taskk.id);
    }
    
    public static string CreateUpdateTeacher(Teacher teacher, string method="PUT")
    {
        var json = JsonConvert.SerializeObject(teacher);
        return PutPostDataObjectJson(Constants.TeachersEndpointUrl, json, method, teacher.id);
    }
    
    /*public static string CreateUpdateUser(User user, string method="PUT")
    {
        var json = JsonConvert.SerializeObject(user);
        return PutPostDataObjectJson(Constants.UsersEndpointUrl, json, user.id, method);
    }*/
    
    public static string DeleteClassroom(int id)
    {
        return DeleteDataObjectJson(Constants.ClassroomsEndpointUrl + "/" + id);
    }
    
    public static string DeleteClassroomTask(int classroomId, int taskId)
    {
        return DeleteDataObjectJson(Constants.ClassroomTaskEndpointUrl + "/" + classroomId + "/" + taskId);
    }

    public static string DeleteGroup(int id)
    {
        return DeleteDataObjectJson(Constants.GroupsEndpointUrl + "/" + id);
    }
    
    public static string DeleteGroupTask(int groupId, int taskId)
    {
        return DeleteDataObjectJson(Constants.GroupTaskEndpointUrl + "/" + groupId + "/" + taskId); 
    }
    
    public static string DeleteStudent(int id)
    {
        return DeleteDataObjectJson(Constants.StudentsEndpointUrl + "/" + id);
    }
    
    public static string DeleteStudentGroup(int studentId, int groupId)
    {
        return DeleteDataObjectJson(Constants.StudentGroupEndpointUrl + "/" + studentId + "/" + groupId);
    }
    
    public static string DeleteStudentTask(int studentId, int taskId)
    {
        return DeleteDataObjectJson(Constants.StudentTaskEndpointUrl + "/" + studentId + "/" + taskId);
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