namespace ires_api.DTO
{
    public class EmployeeDto
    {
        public long employeeid { get; set; }
        public long? companyid { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? middlename { get; set; }
        public string gender { get; set; } = String.Empty;
        public string? mobileno { get; set; }
        public string email { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string designation { get; set; } = string.Empty;
        public bool isactive { get; set; } = true;
        public bool isappsysadmin { get; set; } = false;
    }
}
