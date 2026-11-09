using Microsoft.AspNetCore.Mvc; using RaceDay.Mvc.Services; using System.Text.Json;
namespace RaceDay.Mvc.Controllers;
public class HomeController(ApiClient api):Controller {
 public async Task<IActionResult> Index(string? eventType,DateTime? from){var query="api/events";var args=new List<string>();if(!string.IsNullOrWhiteSpace(eventType))args.Add("eventType="+Uri.EscapeDataString(eventType));if(from.HasValue)args.Add("from="+Uri.EscapeDataString(from.Value.ToString("O")));if(args.Count>0)query+="?"+string.Join("&",args);var rows=await api.Get<JsonElement[]>(query);return View(rows??Array.Empty<JsonElement>());}
 public IActionResult Error()=>View();
}
