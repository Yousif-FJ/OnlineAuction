using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuctionBackend.Application.Models
{
    [Index(nameof(IdentityId), IsUnique = true)]
    public class User : EntityBase
    {
        public User(string identityId, string name)
        {
            IdentityId = identityId ?? throw new ArgumentNullException(nameof(identityId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Bids = new HashSet<Bid>();
            Auctions = new HashSet<Auction>();
            Items = new HashSet<Item>();
        }
        public string Name { get; private set; }
        [MaxLength(40)]
        public string IdentityId { get; private set; }

        public ICollection<Item> Items { get; private set; }
        public ICollection<Auction> Auctions { get; private set; }
        public ICollection<Bid> Bids { get; private set; }
    }
}
