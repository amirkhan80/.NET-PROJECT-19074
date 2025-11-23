using Microsoft.AspNetCore.Mvc;
using SmartServiceHub.Models;
using SmartServiceHub.Services;
using SmartServiceHub.Data;


namespace SmartServiceHub.Controllers;
public class PaymentController : Controller
{
    private readonly MongoDbContext _db;
    private readonly IPaymentService _paymentService;
    public PaymentController(MongoDbContext db, IPaymentService paymentService) { _db = db; _paymentService = paymentService; }

    [HttpGet]
    public IActionResult Pay(string bookingId, decimal amount)
    {
        ViewBag.BookingId = bookingId;
        ViewBag.Amount = amount;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PayConfirm(string bookingId, string paymentMode, decimal amount)
    {
        var payment = new Payment
        {
            BookingId = bookingId,
            Amount = amount,
            PaymentMode = paymentMode,
            TransactionRef = Guid.NewGuid().ToString().Substring(0, 8)
        };
        await _paymentService.CreateAsync(payment);
        return RedirectToAction("Receipt", new { id = payment.Id });
    }

    public async Task<IActionResult> Receipt(string id)
    {
        var pay = await _paymentService.GetByIdAsync(id);
        return View(pay);
    }
}
