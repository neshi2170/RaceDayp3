using Microsoft.AspNetCore.Mvc;using RaceDay.Mvc.Services;using RaceDay.Mvc.Models;using System.Text.Json;
namespace RaceDay.Mvc.Controllers;
public class ProfileController(ApiClient api):Controller {
 private string? Token=>HttpContext.Session.GetString("Token");
 [HttpGet] public async Task<IActionResult> Index(){if(Token==null)return RedirectToAction("Login","Account");var p=await api.Get<JsonElement>("api/users/profile",Token);return View(new ProfileVm{FirstName=p.GetProperty("firstName").GetString()??"",LastName=p.GetProperty("lastName").GetString()??"",Email=p.GetProperty("email").GetString(),Role=p.GetProperty("role").GetString(),Phone=p.TryGetProperty("phone",out var ph)?ph.GetString():null,ProfileImageUrl=p.TryGetProperty("profileImageUrl",out var pi)?pi.GetString():null});}
 [HttpPost] public async Task<IActionResult> Upload(IFormFile file){if(Token==null)return RedirectToAction("Login","Account");var(ok,body)=await api.Upload("api/storage/profile",file,Token);TempData[ok?"Message":"Error"]=ok?"Profile image uploaded.":body;return RedirectToAction(nameof(Index));}
 [HttpPost] public async Task<IActionResult> Index(ProfileVm vm){if(Token==null)return RedirectToAction("Login","Account");var(ok,body)=await api.Put("api/users/profile",new{vm.FirstName,vm.LastName,vm.Phone},Token);TempData[ok?"Message":"Error"]=ok?"Profile updated.":body;return RedirectToAction(nameof(Index));}
}
