namespace ReportWebApi.Models;

public class ReportModel
{
    public DateTime Time { get; set; }

    public string Name { get; set; }

    public string Battery { get; set; }

    public string Status { get; set; }

    public ReportModel(DateTime time, string name, string battery, string status)
    {
        Time = time;
        Name = name;
        Battery = battery;
        Status = status;
    }
}