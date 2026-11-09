using System.Security.Claims;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
namespace RaceDay.Api.Controllers;
[ApiController, Authorize]
[Route("api/storage")]
public class StorageController(IConfiguration config, RaceDayDbContext db) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private async Task<string> Save(IFormFile file, string containerName)
    {
        if (file.Length == 0 || file.Length > 5 * 1024 * 1024) throw new ArgumentException("Choose an image smaller than 5 MB.");
        var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowed.Contains(file.ContentType.ToLowerInvariant())) throw new ArgumentException("Only JPG, PNG and WebP images are accepted.");
        var connection = config["AzureBlobStorage:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connection)) throw new InvalidOperationException("Azure Blob Storage is not configured. Set AzureBlobStorage__ConnectionString.");
        var service = new BlobServiceClient(connection); var container = service.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        var ext = Path.GetExtension(Path.GetFileName(file.FileName)); var name = $"{Guid.NewGuid():N}{ext}";
        var blob = container.GetBlobClient(name); await using var stream = file.OpenReadStream();
        await blob.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobHttpHeaders { ContentType = file.ContentType });
        return blob.Uri.ToString();
    }
    [HttpPost("events/{eventId:int}/banner"), Authorize(Roles="Organiser")]
    public async Task<IActionResult> EventBanner(int eventId, IFormFile file)
    {
        var item = await db.Events.FindAsync(eventId); if(item is null) return NotFound(); if(item.OrganiserId != UserId) return Forbid();
        try { item.BannerUrl = await Save(file,"event-banners"); await db.SaveChangesAsync(); return Ok(new { bannerUrl=item.BannerUrl }); }
        catch(ArgumentException ex){return BadRequest(new{message=ex.Message});} catch(InvalidOperationException ex){return StatusCode(503,new{message=ex.Message});}
    }
    [HttpPost("profile"), Authorize]
    public async Task<IActionResult> ProfileImage(IFormFile file)
    {
        var user=await db.Users.FindAsync(UserId); if(user is null)return NotFound();
        try { user.ProfileImageUrl=await Save(file,"profile-images"); await db.SaveChangesAsync(); return Ok(new{profileImageUrl=user.ProfileImageUrl}); }
        catch(ArgumentException ex){return BadRequest(new{message=ex.Message});} catch(InvalidOperationException ex){return StatusCode(503,new{message=ex.Message});}
    }
}
