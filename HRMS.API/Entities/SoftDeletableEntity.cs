namespace HRMS.API.Entities;

// Base entity with soft delete support
public abstract class SoftDeletableEntity : BaseEntity
{
    public bool IsDeleted { get; set; } = false;
}