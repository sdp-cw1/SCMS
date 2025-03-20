namespace SCMS.Models
{
    public class ModuleModel
    {
        public readonly string moduleId;
        public readonly string moduleName;
        public readonly int lecturer;


        public ModuleModel(string moduleId, string moduleName, int lecturer)
        {
            this.moduleId = moduleId;
            this.moduleName = moduleName;
            this.lecturer = lecturer;
        }
    }
}
