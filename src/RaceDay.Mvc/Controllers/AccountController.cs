using Microsoft.AspNetCore.Mvc; using RaceDay.Mvc.Models; using RaceDay.Mvc.Services; using System.Text.Json;
namespace RaceDay.Mvc.Controllers;
public class AccountController(ApiClient api):Controller {
 [HttpGet] public IActionResult Login()=>View(new LoginVm());
 [HttpPost] public async Task<IActionResult> Login(LoginVm vm){if(!ModelState.IsValid)return View(vm); var (ok,body)=await api.Post("api/auth/login",vm); if(!ok){ModelState.AddModelError("","Login failed. Check your details and API connection.");return View(vm);} using var doc=JsonDocument.Parse(body); var root=doc.RootElement; HttpContext.Session.SetString("Token",root.GetProperty("token").GetString()??""); var user=root.GetProperty("user"); HttpContext.Session.SetString("Role",user.GetProperty("role").GetString()??""); HttpContext.Session.SetString("Name",user.GetProperty("firstName").GetString()+" "+user.GetProperty("lastName").GetString()); return RedirectToAction("Index",HttpContext.Session.GetString("Role")=="Organiser"?"Organiser":"Participant"); }
 [HttpGet] public IActionResult Register()=>View(new RegisterVm());
 [HttpPost] public async Task<IActionResult> Register(RegisterVm vm){if(!ModelState.IsValid)return View(vm); var (ok,body)=await api.Post("api/auth/register",vm); if(!ok){ModelState.AddModelError("", "Registration failed: "+body);return View(vm);} TempData["Message"]="Account created. Please sign in."; return RedirectToAction(nameof(Login));}
 public IActionResult Logout(){HttpContext.Session.Clear();return RedirectToAction("Index","Home");}
}
