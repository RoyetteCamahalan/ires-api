namespace ires_api.DTO.Employee
{
    public class EmployeeRequestDto
    {

        public long employeeid { get; set; }
        public int companyid { get; set; }
        public string firstname { get; set; } = string.Empty;
        public string lastname { get; set; } = string.Empty;
        public string middlename { get; set; } = string.Empty;
        public string gender { get; set; } = string.Empty;
        public string? mobileno { get; set; }
        public string? email { get; set; }
        public bool isactive { get; set; }
        public bool isappsysadmin { get; set; }
        public string? username { get; set; }
        public string userpass { get; set; } = string.Empty;
        public string? designation { get; set; }
        public long? createdbyid { get; set; }
    }
}
