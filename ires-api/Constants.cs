namespace ires_api
{
    public class Constants
    {
        public const string dateFormat = "MM/dd/yyyy";
        public class AppModules
        {
            public const int survey = 15;
        }
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
    }
}
