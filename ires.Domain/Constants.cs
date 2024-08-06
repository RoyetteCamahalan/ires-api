using ires.Domain.Enumerations;

namespace ires.Domain
{
    public class Constants
    {
        public const string dateFormat = "MM/dd/yyyy";
        public const string moneyFormat = "N2";

        public const int BillExtension = 10; //No of days before expired

        public static string getReceiptDesc(ReceiptType receiptType)
        {
            switch (receiptType)
            {
                case ReceiptType.or:
                    return "OR";
                case ReceiptType.pr:
                    return "PR";
                default:
                    return "AR";
            }
        }
        public class InterestType
        {
            public const int notCompounding = 0;
            public const int compounding = 1;
        }
        public class PaymentTransType
        {
            public const string payment = "P";
            public const string creditMemo = "CN"; //Used also as discount
        }
        public class ExpenseTypeCategory
        {
            public const int operating = 1;
            public const int nonoperating = 2;
        }
        public class MailSection
        {
            public const string noReply = "NoReply";
        }
    }
}
