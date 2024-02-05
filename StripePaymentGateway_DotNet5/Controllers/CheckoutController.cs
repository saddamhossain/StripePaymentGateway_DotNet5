using Microsoft.AspNetCore.Mvc;
using Stripe;
using StripePaymentGateway_DotNet5.Interface;
using StripePaymentGateway_DotNet5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripePaymentGateway_DotNet5.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IBook _bookService;
        private readonly IUser _userService;
        private readonly IPayment _paymentService;

        public CheckoutController(IBook bookService, IUser userService, IPayment paymentService)
        {
            _bookService = bookService;
            _userService = userService;
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Purchase(Guid id)
        {
            var book = _bookService.GetById(id);
            ViewBag.PurchaseAmount = book.Price;
            ViewBag.PurchaseAmountWithCent = book.Price * 100;
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public IActionResult Create(string stripeToken, Guid id)
        {
            var user = new User()
            {
                FirstName = "Test",
                LastName = "Test",
                Email = "test@test.com"
            };
            _userService.InsertUser(user);
            var current_user_id = user.Id;

            var customerOptions = new CustomerCreateOptions
            {
                Email = user.Email,          
            };

            var customerService = new CustomerService();
            Customer customer = customerService.Create(customerOptions);
            
            var book = _bookService.GetById(id);

            // Charges means all the payment only. This is not all transaction. this is one kind of transaction only.
            var chargeOptions = new ChargeCreateOptions()
            {
                Amount = (long)Convert.ToDouble(book.Price * 100), // We need to multiply this value by 100 because the price is in Stripe in Cents. So for example if our books price is $30 , when we send it to stripe we need to send it 3000 cents. because that way stripe will know, price is #30.
                Currency = "usd",
                Description = string.Format("Book Projects buy! {0}", user.Email),    
                Source = stripeToken,
                Metadata = new Dictionary<string, string>()
                {
                    {"BookId",book.Id.ToString() },
                    {"BookName",book.Title },
                    {"BookAuthor",book.Author },
                    {"BookPrice",book.Price.ToString() }
                }
            };

            var ChargeService = new ChargeService();
            Charge charge = ChargeService.Create(chargeOptions);
          

            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;

                Payment payment = new Payment();
                payment.UserId = current_user_id;
                payment.BookId = id;
                payment.Quantity = 1;
                payment.Amount = book.Price;
                payment.TotalAmount = book.Price * payment.Quantity;
                payment.PaymentDate = DateTime.Now;
                payment.Currency = "usd";
                payment.StripeReference = BalanceTransactionId;
                payment.PayerId = null;
                payment.InvoiceNumber = null;
                payment.Description = book.Description;
                payment.ShippingAddress = null;
                payment.Tax = "1";
                payment.PaymentMethod = "Stripe";

                _paymentService.InsertPayment(payment);
                return View("Success");
            }
            else
            {
                return View("Failer");

            }

        }


        [HttpGet]
        public IActionResult LoadAllPlans()
        {
            var service = new PlanService();
            var allPlans = service.List().ToList();
            return View(allPlans);
        }


        public IActionResult SubscribeToPlan(string id)
        {
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = "cus_Imu3rSHtVMlLSY",
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Plan = id
                    }
                }
            };

            var service = new SubscriptionService();
            Subscription subscription = service.Create(subscriptionOptions);

            if(subscription.Created != null)
            {
                return View("Subscribed");
            }
            else
            {
                return View("NotSubscribed");
            }

        }

    }
}
