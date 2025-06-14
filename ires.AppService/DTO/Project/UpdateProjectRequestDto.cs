using ires.Domain.Enumerations;
<<<<<<<< HEAD:ires.AppService/DTO/Project/UpdateProjectRequestDto.cs
using System.ComponentModel.DataAnnotations;

namespace ires.AppService.DTO.Project
{
    public class UpdateProjectRequestDto
========
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ires.Core.ViewModels
{
    public class ProjectViewModel
>>>>>>>> 090e3d99a96e33c05fd96f785e67690f1e519b23:ires.Core/ViewModels/ProjectViewModel.cs
    {
        [Required]
        public long propertyid { get; set; }
<<<<<<<< HEAD:ires.AppService/DTO/Project/UpdateProjectRequestDto.cs
        [Required]
========
>>>>>>>> 090e3d99a96e33c05fd96f785e67690f1e519b23:ires.Core/ViewModels/ProjectViewModel.cs
        public string propertyname { get; set; } = string.Empty;
        [Required]
        public string address { get; set; } = string.Empty;
        public string alias { get; set; } = string.Empty;
        public decimal area { get; set; }
        public Boolean isjointventure { get; set; }
        public int computationtype { get; set; }
        public decimal defaultcommission { get; set; }
        public decimal com_percentage { get; set; }
        public decimal compercentageoverterm { get; set; }
        public int paymentterm { get; set; }
        public decimal interest { get; set; }
        public int commissionterm { get; set; }
        public int paymentextension { get; set; }
        public int allow_straight_monthly { get; set; }
        public decimal withholding { get; set; }
        public int interesttype { get; set; }
        public decimal addoninterestpermonth { get; set; }
        public ProjectType projectypeid { get; set; }
    }
}
