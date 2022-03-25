using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AuctionBackend.Application.Services
{
    public class PictureProvider : IPictureProvider
    {
        private const string FolderName = "Auction";
        private readonly Cloudinary cloudinary;

        public PictureProvider(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<PhotoUploadResult> SavePicture(Stream pictureStream)
        {
            var publicId = $"{FolderName}/{Guid.NewGuid()}_{DateTime.Now.Ticks}";

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(publicId, pictureStream),
                PublicId = publicId
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error is not null)
            {
                throw new Exception($"Unable to upload picture, error : {uploadResult.Error}");
            }
            var result = new PhotoUploadResult(publicId, uploadResult.SecureUrl.AbsoluteUri);
            return result;
        }

        public async Task DeletePictureAsync(string publicId)
        {
            var result = await cloudinary.DeleteResourcesAsync(publicId);
            if (result.Error is not null)
            {
                throw new Exception($"Unable to upload picture, error : {result.Error}");
            }
        }
    }
}
