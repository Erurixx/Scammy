//using Microsoft.AspNetCore.Mvc;
//using Scammy.Data;
//using Scammy.Models;

//[Route("api/[controller]")]
//[ApiController]
//public class ArticlesApiController : ControllerBase
//{
//    private readonly ApplicationDbContext _context;
//    private readonly IWebHostEnvironment _env;

//    public ArticlesApiController(ApplicationDbContext context, IWebHostEnvironment env)
//    {
//        _context = context;
//        _env = env;
//    }

//    [HttpPost("create")]
//    public async Task<IActionResult> CreateArticle([FromForm] Article model, IFormFile imageFile, [FromForm] string submitAction)
//    {
//        if (string.IsNullOrWhiteSpace(model.Title) ||
//            string.IsNullOrWhiteSpace(model.Excerpt) ||
//            string.IsNullOrWhiteSpace(model.Content) ||
//            string.IsNullOrWhiteSpace(model.Category))
//        {
//            return BadRequest(new { message = "Please fill in all required fields: Title, Excerpt, Content, Category." });
//        }

//        if (imageFile != null && imageFile.Length > 0)
//        {
//            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "articles");
//            Directory.CreateDirectory(uploadsFolder);

//            string uniqueFileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
//            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await imageFile.CopyToAsync(stream);
//            }

//            model.ImagePath = "/uploads/articles/" + uniqueFileName;
//        }

//        model.Author = User.Identity?.Name ?? "Jasmine";
//        model.Status = submitAction == "saveDraft" ? "draft" : "pending";
//        model.CreatedAt = DateTime.UtcNow;

//        _context.Articles.Add(model);
//        await _context.SaveChangesAsync();

//        return Ok(new { message = "Article saved successfully", status = model.Status });
//    }
//}
