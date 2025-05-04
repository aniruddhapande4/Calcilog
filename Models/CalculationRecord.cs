namespace CalciLog.Models;

public class CalculationRecord
{
    public int Id { get; set; }
    public string Expression { get; set; } = string.Empty;
    public string? Result { get; set; }
    public DateTime Timestamp { get; set; }

    public int ConsumerId { get; set; }
    public Consumer Consumer { get; set; }=null!;
}
