namespace API.Models.Entities
{
    public class UserLike
    {
        public int SourceUserId { get; set; }
        public int LikedUserId { get; set; }
    }
}
