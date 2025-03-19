namespace SCMS.Models
{
    public class ClassRoomModel
    {
        public readonly string id;
        public readonly string name;
        public readonly int seats;

        public ClassRoomModel(string id, string name, int seats)
        {
            this.id = id;
            this.name = name;
            this.seats = seats;
        }
    }
}
