using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ires.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PermissionSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" 
                INSERT INTO permissiongroups (Id, Name, ParentGroupId) VALUES 
                (1, 'Survey', null),
                (2, 'Attachment', 1),
                (3, 'Rental Contract', null),
                (4, 'Attachment', 3),
                (5, 'Building', null),
                (6, 'Room/Door', 5),
                (7, 'Payment', null),
                (8, 'Auto Cash In', 7),
                (9, 'Credit Memo', null),
                (10, 'Client', null),
                (11, 'Expense', null),
                (12, 'Petty Cash', null),
                (13, 'Banks/e-Wallets', null),
                (14, 'Bank Account', null),
                (15, 'Credit Types', 9),
                (16, 'Expense Types', 11),
                (17, 'Office', null),
                (18, 'Account History', 17),
                (19, 'Other Fees', null),
                (20, 'User', null),
                (21, 'Vendor', null),
                (22, 'Subscription Billing', null),
                (23, 'Company Profile', null);");

            migrationBuilder.Sql(@" 
                INSERT INTO permissions (Id, Name, PermissionGroupId) VALUES 
                (1, 'Survey:View', 1),
                (2, 'Survey:Create', 1),
                (3, 'Survey:Update', 1),
                (4, 'Survey:Delete', 1),
                (5, 'SurveyAttachment:Manage', 2),
                (6, 'RentalContract:View', 3),
                (7, 'RentalContract:Create', 3),
                (8, 'RentalContract:Update', 3),
                (9, 'RentalContract:Delete', 3),
                (10, 'RentalContractAttachment:Manage', 4),
                (11, 'Building:View', 5),
                (12, 'Building:Create', 5),
                (13, 'Building:Update', 5),
                (14, 'Building:Delete', 5),
                (15, 'Room:View', 6),
                (16, 'Room:Create', 6),
                (17, 'Room:Update', 6),
                (18, 'Room:Delete', 6),
                (19, 'Payment:View', 7),
                (20, 'Payment:Create', 7),
                (21, 'Payment:Void', 7),
                (22, 'PaymentAutoCashIn:Manage', 8)
                (23, 'CreditMemo:View', 9),
                (24, 'CreditMemo:Create', 9),
                (25, 'CreditMemo:Void', 9),
                (26, 'Client:View', 10),
                (27, 'Client:Create', 10),
                (28, 'Client:Update', 10),
                (29, 'Expense:View', 11),
                (30, 'Expense:Create', 11),
                (31, 'Expense:Update', 11),
                (32, 'Expense:Void', 11),
                (33, 'PettyCash:View', 12),
                (34, 'PettyCash:Create', 12),
                (35, 'PettyCash:Void', 12),
                (36, 'Bank:View', 13),
                (37, 'Bank:Create', 13),
                (38, 'Bank:Update', 13),
                (39, 'BankAccount:View', 14),
                (40, 'BankAccount:Create', 14),
                (41, 'BankAccount:Update', 14),
                (42, 'CreditType:View', 15),
                (43, 'CreditType:Create', 15),
                (44, 'CreditType:Update', 15),
                (45, 'ExpenseType:View', 16),
                (46, 'ExpenseType:Create', 16),
                (47, 'ExpenseType:Update', 16),
                (48, 'Office:View', 17),
                (49, 'Office:Create', 17),
                (50, 'Office:Update', 17),
                (51, 'AccountHistory:View', 18),
                (52, 'OtherFee:View', 19),
                (53, 'OtherFee:Create', 19),
                (54, 'OtherFee:Update', 19),
                (55, 'User:View', 20),
                (56, 'User:Create', 20),
                (57, 'User:Update', 20),
                (58, 'Vendor:View', 21),
                (59, 'Vendor:Create', 21),
                (60, 'Vendor:Update', 21),
                (61, 'SubscriptionBilling:Manage', 22),
                (62, 'Company:Manage', 23);");

            migrationBuilder.Sql(@" 
                INSERT INTO planpermissions (PlanId, PermissionGroupId) VALUES 
                (1, 1), (1, 7), (1, 9), (1, 10), (1, 11), (1, 12), (1, 13), (1, 14), (1, 17), (1, 19), (1, 20), (1, 21), (1, 22), (1, 23), 
                (2, 1), (2, 7), (2, 9), (2, 10), (2, 11), (2, 12), (2, 13), (2, 14), (2, 17), (2, 19), (2, 20), (2, 21), (2, 22), (2, 23), 
                (3, 1), (3, 7), (3, 9), (3, 10), (3, 11), (3, 12), (3, 13), (3, 14), (3, 17), (3, 19), (3, 20), (3, 21), (3, 22), (3, 23),
                (4, 11), (4, 12), (4, 17), (4, 20), (4, 21), (4, 22), (4, 23),
                (5, 11), (5, 12), (5, 17), (5, 20), (5, 21), (5, 22), (5, 23),
                (6, 3), (6, 5), (6, 7), (6, 9), (6, 10), (6, 11), (6, 12), (6, 13), (6, 14), (6, 17), (6, 19), (6, 20), (6, 21), (6, 22), (6, 23),
                (7, 3), (7, 5), (7, 7), (7, 9), (7, 10), (7, 11), (7, 12), (7, 13), (7, 14), (7, 17), (7, 19), (7, 20), (7, 21), (7, 22), (7, 23),
                (8, 3), (8, 5), (8, 7), (8, 9), (8, 10), (8, 11), (8, 12), (8, 13), (8, 14), (8, 17), (8, 19), (8, 20), (8, 21), (8, 22), (8, 23),
                (9, 1), (9, 3), (9, 5), (9, 7), (9, 9), (9, 10), (9, 11), (9, 12), (9, 13), (9, 14), (9, 17), (9, 19), (9, 20), (9, 21), (9, 22), (9, 23);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
