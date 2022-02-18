namespace AuctionBackend.Domain.Models
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

        public double Value { get; private set; }
        public DateTime DateTime { get; private set; }
        public int AuctionId { get; private set; }
        public Auction Auction
        {
            private set => _auction = value;
            get => _auction ??
                throw new InvalidOperationException("Uninitialized property: " + nameof(Auction));
        }
        public int UserId { get; private set; }
        public User User
        {
            private set => _user = value;
            get => _user ??
                throw new InvalidOperationException("Uninitialized property: " + nameof(User));
        }
    }
}
