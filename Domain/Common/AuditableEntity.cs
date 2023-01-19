namespace Domain.Common;

public abstract class AuditableEntity
{
    public string? ModifiedBy { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModificationDate { get; set; }
}
