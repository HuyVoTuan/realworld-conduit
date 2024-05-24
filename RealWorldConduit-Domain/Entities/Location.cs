using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class Location : BaseEntity<Guid>
    {
        public string Slug { get; set; }
        public string Address { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
