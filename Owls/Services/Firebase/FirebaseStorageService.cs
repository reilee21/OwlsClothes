using Firebase.Storage;

namespace Owls.Services.Firebase
{
    public class FirebaseStorageService : IFirebaseStorage
    {
        private readonly IConfiguration configuration;
        private readonly FirebaseStorage firebaseStorage;

        public FirebaseStorageService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.firebaseStorage = new FirebaseStorage(this.configuration["FirebaseStorage:BusketUrl"]);
        }

        public async Task<string> UploadImageAsync(Stream stream, string fileName)
        {
            var imageUrl = await firebaseStorage
                .Child("images")
                .Child(fileName)
                .PutAsync(stream);

            return imageUrl;
        }

        public async Task<string> GetImageStreamAsync(string imageName)
        {
            return await firebaseStorage
                .Child("images")
                .Child(imageName)
                .GetDownloadUrlAsync();
        }
        public async Task RemoveImageAsync(string imageName)
        {
            await firebaseStorage
                .Child("images")
                .Child(imageName)
                .DeleteAsync();
        }
    }
}
