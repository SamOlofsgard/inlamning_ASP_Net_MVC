namespace WebApp_Omnivus.Models
{
    public class UserProfileImage
    {
        public string FileName { get; set; }
        public string FriendlyFileName => FileName.Split("_")[1];
    }
}
