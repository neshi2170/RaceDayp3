using Microsoft.AspNetCore.Mvc; using RaceDay.Mvc.Services; using RaceDay.Mvc.Models; using System.Text.Json;
namespace RaceDay.Mvc.Controllers;
public class EventsController(ApiClient api):Controller {
 public async Task<IActionResult> Details(int id){var e=await api.Get<JsonElement>($"api/events/{id}"); if(e.ValueKind==JsonValueKind.Undefined)return NotFound();return View(e);}
 [HttpPost] public async Task<IActionResult> Enrol(EnrolVm vm){var token=HttpContext.Session.GetString("Token");if(token==null)return RedirectToAction("Login","Account");var (ok,body)=await api.Post("api/enrolments",vm,token);TempData[ok?"Message":"Error"]=ok?"You are enrolled!":body;return RedirectToAction("Index","Participant");}
}
