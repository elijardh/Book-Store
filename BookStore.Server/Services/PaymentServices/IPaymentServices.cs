using System;
namespace BookStore.Server.Services.PaymentServices
{
    public interface IPaymentServices
    {
        Task<(int, dynamic)> ValidatePayment(string reference);
        Task<(int, dynamic)> InitiatePayment();
    }
}

