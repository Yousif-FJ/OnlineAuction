using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuctionBackend.Application.Models
{
    [Index(nameof(IdentityId), IsUnique = true)]
    public class User : EntityBase
    {
        public User(string identityId)
        {
            IdentityId = identityId ?? throw new ArgumentNullException(nameof(identityId));
            Bids = new HashSet<Bid>();
            Auctions = new HashSet<Auction>();
            Items = new HashSet<Item>();
        }
        [MaxLength(40)]
        public string IdentityId { get; private set; }

        public ICollection<Item> Items { get; private set; }
        public ICollection<Auction> Auctions { get; private set; }
        public ICollection<Bid> Bids { get; private set; }
    }
}
