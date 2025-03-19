namespace SCMS.Models
{
    public class CourseModel
    {
        public readonly string courseId;
        public readonly string courseName;
        public readonly string moduleId;
        public readonly string moduleName;
        public readonly int lecturer;


        public CourseModel(string courseId, string courseName, string moduleId, string moduleName, int lecturer)
        {
            this.courseId = courseId;
            this.courseName = courseName;
            this.moduleId = moduleId;
            this.moduleName = moduleName;
            this.lecturer = lecturer;
        }
    }
}
