namespace ires.Domain.Entities
{
    public abstract class BaseModel
    {
        public long createdbyid { get; set; }
        public DateTime datecreated { get; set; }
        public long? updatedbyid { get; set; }
        public DateTime? dateupdated { get; set; }
    }
}
