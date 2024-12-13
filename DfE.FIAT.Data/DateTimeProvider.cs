namespace DfE.FIAT.Data;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime Today { get; }
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime Today => DateTime.Today;
}
