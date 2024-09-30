using Microsoft.AspNetCore.Mvc;
using Stripe;
using StripePaymentGateway_DotNet5.ViewModel;
using System.Linq;

namespace StripePaymentGateway_DotNet5.Controllers
{
    public class StripeDashboardController : Controller
    {
        public IActionResult Index()
        {
            // Note: Transaction and charges are different things.

            var response = new StripeDashboardViewModel();

            var balanceService = new BalanceService();
            var balanaceResult = balanceService.Get();
            response.Balance = balanaceResult;


            var productService = new ProductService();
            var produtResult = productService.List().ToList();
            response.Products = produtResult;


            var transactionService = new BalanceTransactionService();
            var transactionResult = transactionService.List().ToList();
            response.BalanceTransactions = transactionResult;


            var customerService = new CustomerService();
            var customerResult = customerService.List().ToList();
            response.Customers = customerResult;


            var chargeService = new ChargeService();
            var chargeResult = chargeService.List().ToList();
            response.Chages = chargeResult;


            var disputeService = new DisputeService();
            var disputResult = disputeService.List().ToList();
            response.Disputs = disputResult;


            var refundService = new RefundService();
            var refundResult = refundService.List().ToList();
            response.Refunds = refundResult;

            return View(response);
        }
    }
}
