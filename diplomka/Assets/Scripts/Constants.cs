﻿using DbClasses;
using UnityEngine;

public static class Constants
{
    private const string ServerUrl = "https://localhost:7193/api";

    public const string ClassroomsEndpointUrl = ServerUrl + "/Classroom";
    public const string ClassroomsGetByTeacherIdEndpointUrl = ServerUrl + "/Classroom/getByTeacherId";

    public const string ClassroomTaskEndpointUrl = ServerUrl + "/ClassroomsTasks";

    public const string GroupsEndpointUrl = ServerUrl + "/Groups";
    public const string GroupsGetByClassroomIdEndpointUrl = ServerUrl + "/Groups/getByClassroomId";
    public const string GroupsGetByStudentIdEndpointUrl = ServerUrl + "/Groups/getByStudentId";

    public const string GroupTaskEndpointUrl = ServerUrl + "/GroupsTasks";

    public const string StudentsEndpointUrl = ServerUrl + "/Students";
    public const string StudentsGetByClassroomIdEndpointUrl = ServerUrl + "/Students/getByClassroomId";
    public const string StudentsGetByGroupIdEndpointUrl = ServerUrl + "/Students/getByGroupId";

    public const string StudentsGetByClassroomIdNotInGroupIdEndpointUrl =
        ServerUrl + "/Students/getByClassroomIdNotInGroupId";

    public const string StudentGroupEndpointUrl = ServerUrl + "/StudentsGroups";

    public const string GetAllClassroomStudentsGroupsEndpointUrl =
        ServerUrl + "/StudentsGroups/getAllClassroomStudentsGroups";

    public const string StudentTaskEndpointUrl = ServerUrl + "/StudentsTasks";

    public const string TasksEndpointUrl = ServerUrl + "/Tasks";
    public const string TasksGetByClassroomIdEndpointUrl = ServerUrl + "/Tasks/getByClassroomId";
    public const string TasksGetByGroupIdEndpointUrl = ServerUrl + "/Tasks/getByGroupId";
    public const string TasksGetByStudentIdEndpointUrl = ServerUrl + "/Tasks/getByStudentId";

    public const string TeachersEndpointUrl = ServerUrl + "/Teachers";
    public const string TeacherGetByEmailEndpointUrl = ServerUrl + "/Teachers/getByEmail";
    public const string TeacherGetByUserNameEndpointUrl = ServerUrl + "/Teachers/getByUserName";
    public const string TeacherGetByLoginEndpointUrl = ServerUrl + "/Teachers/getByLogin";

    public const string UsersEndpointUrl = ServerUrl + "/Users";

    public const string SaveButtonTextUpdate = "Uložiť";
    public const string SaveButtonTextCreate = "Vytvoriť";
    public const string WrongFirstNameFormatMessage = "Musí obsahovať aspoň 2 znaky!";
    public const string WrongLastNameFormatMessage = "Musí obsahovať aspoň 2 znaky!";
    public const string WrongUserNameFormatMessage = "Musí obsahovať aspoň 4 znaky!";
    public const string WrongUserNameAlreadyExistMessage = "Obsadený login!";
    public const string WrongEmailFormatMessage = "Nesprávny formát!";
    public const string WrongEmailAlreadyExistMessage = "Konto s e-mailom uz existuje!";
    public const string WrongPasswordFormatMessage = "Musí obsahovať aspoň 4 znaky!";
    public const string WrongPasswordsNotSameMessage = "Heslá sa nezhodujú!";
    public const string WrongClassroomNameFormatMessage = "Musí obsahovať aspoň 2 znaky!";
    public const string WrongGroupNameFormatMessage = "Musí obsahovať aspoň 2 znaky!";
    public const string WrongTaskNameFormatMessage = "Musí obsahovať aspoň 2 znaky!";
    public const string WrongUserNameOrPasswordMessage = "Nesprávny login alebo heslo!";

    public const int MinimalFirstNameLength = 2;
    public const int MinimalLastNameLength = 2;
    public const int MinimalUserNameLength = 4;
    public const int MinimalPasswordLength = 4;
    public const int MinimalClassroomNameLength = 2;
    public const int MinimalGroupNameLength = 2;
    public const int MinimalTaskNameLength = 2;

    public static string LastSceneName = "First";

    public static string GetDeleteClassroomString(Classroom classroom)
    {
        return "Odstrániť triedu " + classroom.name + "?";
    }

    public static string GetDeleteStudentString(Student student)
    {
        return "Odstrániť žiaka " + student.name + " " + student.lastName + "?";
    }

    public static string GetDeleteGroupString(Group group)
    {
        return "Odstrániť skupinu " + group.name + "?";
    }

    public static string GetDeleteTaskString(Taskk taskk)
    {
        return "Odstrániť úlohu " + taskk.name + "?";
    }

    public static string GetDeleteStudentFromGroupString(Student student)
    {
        return "Odstrániť žiaka " + student.name + " " + student.lastName + " zo skupiny?";
    }

    public static string GetDeleteGroupFromStudentString(Group group)
    {
        return "Odstrániť žiaka zo skupiny " + group.name + "?";
    }

    public static string GetDeleteTaskFromGroupString(Taskk task)
    {
        return "Odstrániť úlohu " + task.name + " zo skupiny?";
    }

    public static string GetDeleteTaskFromStudentString(Taskk task)
    {
        return "Odstrániť žiakovi úlohu " + task.name + "?";
    }

    public static Sprite GetSprite(string path) {return Resources.Load<Sprite>(path);}

    public static Sprite xSprite =  Resources.Load <Sprite>("Sprites/close");
    public static Sprite dotsSprite =  Resources.Load <Sprite>("Sprites/more");
    public static Sprite plusSprite =  Resources.Load <Sprite>("Sprites/more");
    
    public static EmailSender emailSender = new EmailSender();
    public static MySceneManager mySceneManager = new MySceneManager();
    
    public static Classroom Classroom {get; set; }
    public static Group Group {get; set; }
    public static Student Student {get; set; }
    public static Taskk Taskk {get; set; }
    public static Teacher User {get; set; }
}