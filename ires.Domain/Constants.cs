using ires.Domain.Enumerations;

namespace ires.Domain
{
    public class Constants
    {
        public const string dateFormat = "MM/dd/yyyy";

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
        public class BookingStatus
        {
            public const int pending = 0;
            public const int confirmed = 1;
            public const int cancelled = 2;
        }
        public class BookingRateType
        {
            public const int daily = 0;
            public const int hourly = 1;
            public const int weekly = 2;
            public const int monthly = 3;
            public const int fixrate = 4;
        }
        public class MailSection
        {
            public const string noReply = "NoReply";
        }
    }
}
