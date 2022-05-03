using System.Linq.Expressions;

namespace AuctionBackend.Application.Models
{
    public class Auction : EntityBase
    {
        private Item? _item;

        public Auction(DateTime creationDate, DateTime expireDate, int itemId)
        {
            CreationDate = creationDate;
            ExpireDate = expireDate;
            ItemId = itemId;
            Bids = new HashSet<Bid>();
        }

        public double CurrentPrice { get; set; }
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

        public bool HasEnded { get => HasEndedFunc.Invoke(this); }

        public static readonly Expression<Func<Auction, bool>> HasEndedExpression =
            (Auction) => Auction.ExpireDate < DateTime.UtcNow;

        private static readonly Func<Auction, bool> HasEndedFunc = HasEndedExpression.Compile();
    }
}
