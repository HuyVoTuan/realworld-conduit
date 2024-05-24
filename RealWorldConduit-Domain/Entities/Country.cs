using RealWorldConduit_Domain.Commons;

namespace RealWorldConduit_Domain.Entities
{
    public class Country : BaseEntity
    {
        public string Code { get; set; }
        public string Language { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
    }
}
