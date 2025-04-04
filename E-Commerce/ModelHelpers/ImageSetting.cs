namespace E_Commerce.ModelHelpers
{
    public class ImageSetting
    {
        public ICollection<string> AllowedExtensions { get; set; }
        public int MaxSize { get; set; }
    }
}
