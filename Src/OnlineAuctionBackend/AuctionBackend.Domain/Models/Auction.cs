namespace AuctionBackend.Domain.Models
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

        public double CurrentPrice { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime ExpireDate { get; private set; }
        public int ItemId { get; private set; }
        public ICollection<Bid> Bids { get; private set; }
        public Item Item
        {
            private set => _item = value;
            get => _item ??
                throw new InvalidOperationException("Uninitialized property: " + nameof(Item));
        }
    }
}
