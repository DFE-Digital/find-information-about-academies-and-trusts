namespace DfE.FIAT.Data;

public record AgeRange(int Minimum, int Maximum)
{
    public AgeRange(string Minimum, string Maximum) : this(int.Parse(Minimum), int.Parse(Maximum))
    {
    }
}
