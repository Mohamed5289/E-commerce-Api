namespace E_Commerce.ModelHelpers
{
    public class ImageService
    {
        public void DeleteImage(string imageName , params string[] paths)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory() , Path.Combine(paths));

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
