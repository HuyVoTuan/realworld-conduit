namespace RealWorldConduit_Domain.Commons
{
    public interface IAuditEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
