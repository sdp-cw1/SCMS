public class ScheduleModel
{
    public readonly string Id;
    public readonly string EventId;
    public readonly string EventName;
    public readonly DateTime StartDateTime;
    public readonly DateTime EndDateTime;
    public readonly string Location;
    public readonly string Category;

    public ScheduleModel(string Id, string EventId, string EventName, DateTime StartDateTime, DateTime EndDateTime, string Location, string Category)
    {
        this.Id = Id;
        this.EventId = EventId;
        this.EventName = EventName;
        this.StartDateTime = StartDateTime;
        this.EndDateTime = EndDateTime;
        this.Location = Location;
        this.Category = Category;
    }
}
