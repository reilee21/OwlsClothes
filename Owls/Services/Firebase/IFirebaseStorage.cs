namespace Owls.Services.Firebase
{
    public interface IFirebaseStorage
    {
        Task<string> UploadImageAsync(Stream stream, string fileName);
        Task<string> GetImageStreamAsync(string imageName);
    }
}
