namespace CalciLog.Models;

public class Consumer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; }=null!;

    public List<CalculationRecord> Records { get; set; } = new();
}
