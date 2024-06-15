using ires.Domain.DTO.Company;

namespace ires.Domain.DTO.Employee
{
    public class EmployeeViewModel
    {
        public long employeeid { get; set; }
        public int companyid { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public string? middlename { get; set; }
        public string gender { get; set; } = string.Empty;
        public string? mobileno { get; set; }
        public string email { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string designation { get; set; } = string.Empty;
        public bool isactive { get; set; } = true;
        public bool isappsysadmin { get; set; } = false;

        public CompanyViewModel? company { get; set; }
    }
}
