namespace AuctionBackend.Api.Controllers.V1
{
    public static class Manifest
    {
        public const string Login = "/v1/authentication/login";
        public const string CreateUser = "/v1/authentication/user";
        public const string GetUser = "/v1/authenication/user";

        public const string PostItem = "/v1/item";
        public const string PatchItem = "/v1/item";
        public const string GetItem = "/v1/item";
        public const string GetAllItem = "/v1/allItems";
    }
}
