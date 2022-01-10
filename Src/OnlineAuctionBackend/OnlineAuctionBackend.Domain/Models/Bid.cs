namespace OnlineAuctionBackend.Domain.Models
{
    public class Bid : EntityBase
    {
        private Auction? _auction;
        private User? _user;

        public Bid(double value, DateTime dateTime, int auctionId, int userId)
        {
            Value = value;
            DateTime = dateTime;
            AuctionId = auctionId;
            UserId = userId;
        }

        public double Value { get; }
        public DateTime DateTime { get; }
        public int AuctionId { get; }
        public Auction Auction
        {
            get => _auction ??
                throw new InvalidOperationException("Uninitialized property: " + nameof(Auction));
        }
        public int UserId { get; }
        public User User
        {
            get => _user ??
                throw new InvalidOperationException("Uninitialized property: " + nameof(User));
        }
    }
}
