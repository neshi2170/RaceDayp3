using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace RaceDay.Mvc.Services;
public class ApiClient(IHttpClientFactory factory)
{
    private readonly JsonSerializerOptions json = new(JsonSerializerDefaults.Web) { PropertyNameCaseInsensitive = true };
    private HttpClient Client() => factory.CreateClient("RaceDayApi");
    public async Task<T?> Get<T>(string path, string? token = null) { using var r = await Send(HttpMethod.Get,path,null,token); if(!r.IsSuccessStatusCode) return default; return await r.Content.ReadFromJsonAsync<T>(json); }
    public async Task<(bool Ok,string Body)> Post(string path, object data, string? token = null) => await SendResult(HttpMethod.Post,path,data,token);
    public async Task<(bool Ok,string Body)> Put(string path, object data, string? token = null) => await SendResult(HttpMethod.Put,path,data,token);
    public async Task<(bool Ok,string Body)> Delete(string path, string? token = null) => await SendResult(HttpMethod.Delete,path,null,token);
    public async Task<(bool Ok,string Body)> Upload(string path, IFormFile file, string? token)
    { using var content = new MultipartFormDataContent(); await using var s=file.OpenReadStream(); var part=new StreamContent(s); part.Headers.ContentType=new MediaTypeHeaderValue(file.ContentType); content.Add(part,"file",Path.GetFileName(file.FileName)); using var req=new HttpRequestMessage(HttpMethod.Post,path){Content=content}; if(!string.IsNullOrEmpty(token)) req.Headers.Authorization=new AuthenticationHeaderValue("Bearer",token); using var res=await Client().SendAsync(req); return (res.IsSuccessStatusCode,await res.Content.ReadAsStringAsync()); }
    private async Task<(bool Ok,string Body)> SendResult(HttpMethod method,string path,object? data,string? token){using var r=await Send(method,path,data,token); return(r.IsSuccessStatusCode,await r.Content.ReadAsStringAsync());}
    private async Task<HttpResponseMessage> Send(HttpMethod method,string path,object? data,string? token){var req=new HttpRequestMessage(method,path); if(data!=null) req.Content=new StringContent(JsonSerializer.Serialize(data,json),Encoding.UTF8,"application/json"); if(!string.IsNullOrEmpty(token)) req.Headers.Authorization=new AuthenticationHeaderValue("Bearer",token); return await Client().SendAsync(req);}
}
