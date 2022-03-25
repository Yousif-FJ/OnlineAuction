namespace AuctionBackend.Application.Services
{
    public interface IPictureProvider
    {
        public Task<PhotoUploadResult> SavePicture(Stream pictureStream);
        public Task DeletePictureAsync(string publicId);
    }

    public record PhotoUploadResult(string PublicId, string Url);
}
