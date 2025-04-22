namespace RO.DevTest.Domain.Abstract;

public abstract class BaseEntity {
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
    
    public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;
}
