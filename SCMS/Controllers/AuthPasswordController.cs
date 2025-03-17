using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using SCMS.Models;

public class AuthPasswordController : Controller
{
    public IActionResult Index()
    {
        // Retrieve the stored email and make it persist for another request
        if (TempData.ContainsKey("Email"))
        {
            ViewBag.Email = TempData["Email"];
            TempData.Keep("Email"); // Keep TempData for the next request
        }
        return View();
    }

    [HttpPost]
    public IActionResult Authenticate(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            ViewBag.Error = "Email is required!";
            return View("Index");
        }

        TempData["Email"] = email; // Keep email for further use

        // Generate a random password
        string generatedPassword = GenerateRandomPassword();

        // Send the password to the email
        bool emailSent = new DBModel().SendPasswordEmail(email, generatedPassword);

        if (emailSent)
        {
            // Keep TempData for redirect action
            TempData["Email"] = email;  // Ensure email is passed to the next action
            return RedirectToAction("Confirmation"); // Redirect to confirmation page
        }

        ViewBag.Error = "Failed to send the email. Please try again.";
        return View("Index");
    }

    private string GenerateRandomPassword()
    {
        var random = new Random();

        // Define character sets
        const string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numbers = "0123456789";
        const string symbols = "$#@";
        const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";

        // Generate each required part of the password
        string firstTwoLetters = new string(Enumerable.Range(0, 2).Select(_ => uppercaseLetters[random.Next(uppercaseLetters.Length)]).ToArray());
        string fourNumbers = new string(Enumerable.Range(0, 4).Select(_ => numbers[random.Next(numbers.Length)]).ToArray());
        char specialSymbol = symbols[random.Next(symbols.Length)];
        char lastLetter = lowercaseLetters[random.Next(lowercaseLetters.Length)];

        // Concatenate to form the final password
        string password = firstTwoLetters + fourNumbers + specialSymbol + lastLetter;

        return password;
    }


    public IActionResult Confirmation()
    {
        // Make sure Email is available in TempData or ViewBag
        if (TempData.ContainsKey("Email"))
        {
            ViewBag.Email = TempData["Email"];
            TempData.Keep("Email"); // Keep it for another request if needed
        }

        // Confirmation page where you can show a success message
        return View();
    }
}
