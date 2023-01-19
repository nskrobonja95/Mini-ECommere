namespace Domain.Models;

public class MeasureUnit
{

    public MeasureUnit()
    {
        Name = String.Empty;
        ShortName = String.Empty;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public List<Product>? Products { get; set; }

}
