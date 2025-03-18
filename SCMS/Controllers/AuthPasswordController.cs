using Microsoft.AspNetCore.Mvc;
using SCMS.Models;

public class AuthPasswordController : Controller
{
    public IActionResult Index()
    {
        if (TempData.ContainsKey("Email"))
        {
            ViewBag.Email = TempData["Email"];
            TempData.Keep("Email"); // Keep TempData for the next request
        }
        return View();
    }

    [HttpPost]
    public IActionResult AuthenticateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            ViewBag.Error = "Email is required!";
            return View("Index");
        }

        TempData["Email"] = email;

        string generatedPassword = GenerateRandomPassword();
        bool emailSent = new DBModel().SendPasswordEmail(email, generatedPassword);

        if (emailSent)
        {
            TempData["Email"] = email;  // Ensure email is passed to the next action
            return RedirectToAction("Index"); // Redirect to confirmation page
        }

        ViewBag.Error = "Failed to send the email. Please try again.";
        return View("Index");
    }

    [HttpPost]
    public IActionResult AuthenticatePassword(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "Email and Password are required!";
            return View("Index");
        }

        bool isValid = new DBModel().ValidateTempPassword(email, password);

        if (isValid)
        {
            TempData["Email"] = email; // Keep email for next step
            return RedirectToAction("Index", "ConfirmPassword"); // Redirect to ConfirmPassword
        }

        ViewBag.Error = "Invalid email or temporary password!";
        return View("Index");
    }


    public IActionResult Confirmation()
    {
        if (TempData.ContainsKey("Email"))
        {
            ViewBag.Email = TempData["Email"];
            TempData.Keep("Email"); // Keep it for another request if needed
        }

        return View();
    }

    private string GenerateRandomPassword()
    {
        var random = new Random();

        const string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numbers = "0123456789";
        const string symbols = "$#@";
        const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";

        string firstTwoLetters = new string(Enumerable.Range(0, 2).Select(_ => uppercaseLetters[random.Next(uppercaseLetters.Length)]).ToArray());
        string fourNumbers = new string(Enumerable.Range(0, 4).Select(_ => numbers[random.Next(numbers.Length)]).ToArray());
        char specialSymbol = symbols[random.Next(symbols.Length)];
        char lastLetter = lowercaseLetters[random.Next(lowercaseLetters.Length)];

        return firstTwoLetters + fourNumbers + specialSymbol + lastLetter;
    }
}
