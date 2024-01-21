namespace DbClasses
{
    public class Student
    {
        public int id;
        public int classroomId;
        public string name;
        public string lastName;
        public string userName;
        public string password;
        public string imagePath;
        
        public override bool Equals(object obj)
        { 
            var item = obj as Student;

            if (item == null)
            {
                return false;
            }

            return item.id == id;
        }
    }
}
