using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
namespace AspDotNetIdentity.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            if (Credential.UserName == "admin" && Credential.Password == "password")
            {
                //creating the security context
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name,"admin"),
                    new Claim(ClaimTypes.Email,"admin@mywebsite.com"),
                    new Claim("Department","HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", "2023-04-01")
                };
                var identity = new ClaimsIdentity(claims,"MyCookieAuth");

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.RememberMe
                };
                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal, authProperties);
                return RedirectToPage("/Index");
            }
            return(Page());
        }
    }
    public class Credential
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public Boolean RememberMe { get; set; }
    }
}
