namespace OnlineAuctionBackend.Domain.Models
{
    public class Auction : EntityBase
    {
        private Item? _item;

        public Auction(double currentPrice, DateTime creationDate, DateTime expireDate, int itemId)
        {
            CurrentPrice = currentPrice;
            CreationDate = creationDate;
            ExpireDate = expireDate;
            ItemId = itemId;
            Bids = new HashSet<Bid>();
        }

        public double CurrentPrice { get; }
        public DateTime CreationDate { get; }
        public DateTime ExpireDate { get; }
        public int ItemId { get; }
        public ICollection<Bid> Bids { get; private set; }
        public Item Item
        {
            get => _item ??
                throw new InvalidOperationException("Uninitialized property: " + nameof(Item));
        }
    }
}
