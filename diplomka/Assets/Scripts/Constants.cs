public static class Constants
{
    private const string ServerUrl = "https://localhost:7193/api";

    public static readonly string ClassroomsEndpointUrl = ServerUrl +  "/Classroom";
    public static readonly string ClassroomGetGroupsEndpointUrl = ServerUrl + "/Classroom/getAllGroups";
    public static readonly string ClassroomGetStudentsEndpointUrl = ServerUrl + "/Classroom/getAllStudents";
    public static readonly string ClassroomGetTasksEndpointUrl = ServerUrl + "/Classroom/getAllTasks";
    public static readonly string GroupsEndpointUrl = ServerUrl + "/Groups";
    public static readonly string GroupGetStudentsEndpointUrl = ServerUrl + "/Groups/getAllStudents";
    public static readonly string GroupGetTasksEndpointUrl = ServerUrl + "/Groups/getAllTasks";
    public static readonly string GroupTaskEndpointUrl = ServerUrl + "/GroupsTasks";
    public static readonly string StudentsEndpointUrl = ServerUrl + "/Students";
    public static readonly string StudentGetGroupsEndpointUrl = ServerUrl + "/Students/getAllGroups";
    public static readonly string StudentGetTasksEndpointUrl = ServerUrl + "/Students/getAllTasks";
    public static readonly string ClassroomTaskEndpointUrl = ServerUrl + "/ClassroomsTasks";
    public static readonly string StudentGroupEndpointUrl = ServerUrl + "/StudentsGroups";
    public static readonly string StudentTaskEndpointUrl = ServerUrl + "/StudentsTasks";
    public static readonly string TasksEndpointUrl = ServerUrl + "/Tasks";
    public static readonly string TeachersEndpointUrl = ServerUrl + "/Teachers";
    public static readonly string TeacherGetClassroomsEndpointUrl = ServerUrl + "/Teachers/getAllClassrooms";
    public static readonly string UsersEndpointUrl = ServerUrl + "/Users";

    public static int ClassroomId {get; set; }
    public static int GroupId {get; set; }
    public static int StudentId {get; set; }
    public static int TaskId {get; set; }
    public static int UserId = 1; 
}