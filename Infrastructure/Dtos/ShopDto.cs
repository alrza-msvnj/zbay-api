using Domain.Entities;

namespace Infrastructure.Dtos;

public class ShopDto
{
    public class ShopResponseDto
    {
        public uint Id { get; set; }
        public Guid Uuid { get; set; }
        public required string InstagramId { get; set; }
        public required string InstagramUrl { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public uint Followers { get; set; }
        public required string Logo { get; set; }
        public ushort TotalProducts { get; set; }
        public uint OwnerId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsValidated { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime JoinDate { get; set; }
        public required List<Category> Categories { get; set; }
    }

    public class ShopCreateDto
    {
        public string? IgId { get; set; }
        public required string InstagramId { get; set; }
        public required string InstagramUrl { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public uint Followers { get; set; }
        public required string Logo { get; set; }
        public uint OwnerId { get; set; }
        public bool IsVerified { get; set; }
    }
}
