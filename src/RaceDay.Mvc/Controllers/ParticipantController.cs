using Microsoft.AspNetCore.Mvc; using RaceDay.Mvc.Services; using System.Text.Json;
namespace RaceDay.Mvc.Controllers;
public class ParticipantController(ApiClient api):Controller {
 private string? Token=>HttpContext.Session.GetString("Token"); private bool Guard()=>Token!=null&&HttpContext.Session.GetString("Role")=="Participant";
 public async Task<IActionResult> Index(){if(!Guard())return RedirectToAction("Login","Account");ViewBag.Enrolments=await api.Get<JsonElement[]>("api/enrolments/me",Token)??Array.Empty<JsonElement>();return View(await api.Get<JsonElement[]>("api/events",Token)??Array.Empty<JsonElement>());}
 public async Task<IActionResult> Results(){if(!Guard())return RedirectToAction("Login","Account");return View(await api.Get<JsonElement[]>("api/results/me",Token)??Array.Empty<JsonElement>());}
}
