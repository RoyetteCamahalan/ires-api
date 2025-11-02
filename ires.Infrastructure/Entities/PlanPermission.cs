using System.ComponentModel.DataAnnotations.Schema;

namespace ires.Infrastructure.Entities
{
    [Table("planpermissions")]
    public class PlanPermission
    {
        public int PlanId { get; set; }
        public int PermissionGroupId { get; set; }

        public SubscriptionPlan? SubscriptionPlan { get; set; }
        public PermissionGroup? PermissionGroup { get; set; }
    }
}
