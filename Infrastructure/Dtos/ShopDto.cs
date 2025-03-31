namespace Infrastructure.Dtos;

public class ShopDto
{
    public class ShopCreateDto
    {
        public required string InstagramId { get; set; }
        public required string InstagramUrl { get; set; }
        public required string Name { get; set; }
        public uint Followers { get; set; }
        public byte[] Logo { get; set; }
        public uint OwnerId { get; set; }
        public bool IsVerified { get; set; }
    }
}
