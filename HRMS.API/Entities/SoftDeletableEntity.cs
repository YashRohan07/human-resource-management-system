namespace HRMS.API.Entities;

// Used for entities that support soft delete
public abstract class SoftDeletableEntity : BaseEntity
{
    public bool IsDeleted { get; set; } = false;
}