namespace ires_api
{
    public class Constants
    {
        public const string dateFormat = "MM/dd/yyyy";
        public class InterestType
        {
            public const int notCompounding = 0;
            public const int compounding = 1;
        }
        public class ReceiptType
        {
            public const int or = 1; //Official Receipt
            public const int ar = 2; //Acknowledgement Receipt
            public const int pr = 3; //Probationary Receipt

            public static string getReceiptDesc(int receiptType)
            {
                switch (receiptType)
                {
                    case or:
                        return "OR";
                    case pr:
                        return "PR";
                    default:
                        return "AR";
                }
            }
        }
        public class PaymentStatus
        {
            public const int paid = 0;
            public const int @void = 1;
            public const int refunded = 2;
        }
        public class PaymentTransType
        {
            public const string payment = "P";
            public const string creditMemo = "CN"; //Used also as discount
        }
        public class PaymentMode
        {
            public const int cash = 0;
            public const int check = 1;
            public const int bankTransfer = 2;
            public const int eWallet = 3;
        }
        public class CheckStatus
        {
            public const int floating = 0;
            public const int deposited = 1;
            public const int hold = 2;
            public const int replaced = 3;
            public const int cancelled = 4;
        }
        public class SurveyStatus
        {
            public const int pending = 0;
            public const int cancelled = 1;
            public const int surveyed = 2;
            public const int completed = 3;
        }
        public class BillStatus
        {
            public const int open = 0;
            public const int paid = 1;
            public const int cancelled = 2;
        }
        public class BillCycle
        {
            public const int monthly = 1;
            public const int yearly = 2;
        }
        public class DisbursementStatus
        {
            public const int approved = 0;
            public const int @void = 1;
        }
        public class DisbursementTransType
        {
            public const int cashin = 0;
            public const int transferout = 1;
            public const int transferin = 2;
        }
        public class ExpenseStatus
        {
            public const int approved = 0;
            public const int @void = 1;
        }
        public class ExpenseTypeCategory
        {
            public const int operating = 1;
            public const int nonoperating = 2;
        }
        public class AccountPayableStatus
        {
            public const int approved = 0;
            public const int @void = 1;
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
    }
}
