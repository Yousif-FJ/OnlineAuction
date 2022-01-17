namespace OnlineAuctionBackend.Domain.Models
{
    public class User : EntityBase
    {
        public User(string identityId)
        {
            IdentityId = identityId ?? throw new ArgumentNullException(nameof(identityId));
            Bids = new HashSet<Bid>();
            Auctions = new HashSet<Auction>();
            Items = new HashSet<Item>();
        }

        public string IdentityId { get; private set; }

        public ICollection<Item> Items { get; private set; }
        public ICollection<Auction> Auctions { get; private set; }
        public ICollection<Bid> Bids { get; private set; }
    }
}
