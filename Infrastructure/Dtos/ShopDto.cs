using Domain.Entities;

namespace Infrastructure.Dtos;

public class ShopDto
{
    public class ShopResponseDto
    {
        public long Id { get; set; }
        public Guid Uuid { get; set; }
        public string? IgId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Logo { get; set; }
        public int TotalProducts { get; set; }
        public string? IgUsername { get; set; }
        public string? IgFullName { get; set; }
        public long? IgFollowers { get; set; }
        public long? OwnerId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsValidated { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime JoinDate { get; set; }
        public required List<Category> Categories { get; set; }
    }

    public class ShopCreateDto
    {
        public string? IgId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Logo { get; set; }
        public string? IgUsername { get; set; }
        public string? IgFullName { get; set; }
        public long? IgFollowers { get; set; }
        public long? OwnerId { get; set; }
        public bool IsVerified { get; set; }
    }
}
