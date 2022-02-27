namespace AuctionBackend.Application.Models
{
    public class Item : EntityBase
    {
        private User? _owner;

        public Item(string name, double startingPrice)
        {
            Name = name;
            StartingPrice = startingPrice;
        }

        public string Name { get; set; }
        public double StartingPrice { get; set; }
        public string? PhotoUrl { get; set; }
        public Auction? Auction { get; private set; }
        public int OwnerId { get; set; }
        public User Owner
        {
            set => _owner = value;
            get => _owner ??
                throw new InvalidOperationException("Uninitialized property: " + nameof(Owner));
        }
    }
}
