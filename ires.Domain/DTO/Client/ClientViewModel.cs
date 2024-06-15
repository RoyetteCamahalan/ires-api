namespace ires.Domain.DTO.Client
{
    public class ClientViewModel
    {
        public long custid { get; set; }
        public int companyid { get; set; }
        public string lname { get; set; } = string.Empty;
        public string fname { get; set; } = string.Empty;
        public string mname { get; set; } = string.Empty;
        public string fullname
        {
            get => fname + ' ' + lname;
        }
        public DateTime? birthdate { get; set; }
        public string address { get; set; } = string.Empty;
        public string contactno { get; set; } = string.Empty;
        public string tinnumber { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public long createdbyid { get; set; }
        public long updatedbyid { get; set; }
    }
}
