using System;
namespace BookStore.Server.Models.ResponseModels
{
    using System;

    public class FlutterWaveResponse
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public TransactionData? Data { get; set; }
    }

    public class TransactionData
    {
        public long? Id { get; set; }
        public string? TxRef { get; set; }
        public string? FlwRef { get; set; }
        public string? DeviceFingerprint { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public decimal? ChargedAmount { get; set; }
        public decimal? AppFee { get; set; }
        public decimal? MerchantFee { get; set; }
        public string? ProcessorResponse { get; set; }
        public string? AuthModel { get; set; }
        public string? Ip { get; set; }
        public string? Narration { get; set; }
        public string? Status { get; set; }
        public string? PaymentType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? AccountId { get; set; }
        public Card? Card { get; set; }
        public object? Meta { get; set; }
        public decimal? AmountSettled { get; set; }
        public Customer? Customer { get; set; }
    }

    public class Card
    {
        public string? First6Digits { get; set; }
        public string? Last4Digits { get; set; }
        public string? Issuer { get; set; }
        public string? Country { get; set; }
        public string? Type { get; set; }
        public string? Token { get; set; }
        public string? Expiry { get; set; }
    }

    public class Customer
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

}

