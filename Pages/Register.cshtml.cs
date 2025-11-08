using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace THelp_Web.Pages
{
    public class RegisterModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // In a real application, you would handle user registration here
            // For now, redirect to login page
            return RedirectToPage("/Login");
        }
    }
}