using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.BusinessLayer.Repos;

namespace ToDoList.API.Presentation_Layer.Controllers
{
    [Route("api/v{version:apiVersion}/files")]
    [ApiController]
    //[Authorize]
    public class FilesController : ControllerBase
    {

        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        private readonly IToDoListRepo _toDoListRepo;
        private readonly IMapper _mapper;

        public FilesController(
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider, IToDoListRepo toDoListRepo, IMapper mapper)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
            _toDoListRepo = toDoListRepo ??
               throw new ArgumentNullException(nameof(toDoListRepo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        //[HttpGet("{fileId}")]
        //[ApiVersion(0.1, Deprecated = true)]
        //public ActionResult GetFile(string fileId)
        //{
        //    var pathToFile = "getting-started-with-rest-slides.pdf";

        //    if (!System.IO.File.Exists(pathToFile))
        //    {
        //        return NotFound();
        //    }

        //    if (!_fileExtensionContentTypeProvider.TryGetContentType(
        //        pathToFile, out var contentType))
        //    {
        //        contentType = "application/octet-stream";
        //    }

        //    var bytes = System.IO.File.ReadAllBytes(pathToFile);
        //    return File(bytes, contentType, Path.GetFileName(pathToFile));
        //}

        //[HttpPost]
        //[ApiVersion(0.1)]
        //public async Task<ActionResult> CreateFile(IFormFile file)
        //{
          
        //    if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
        //    {
        //        return BadRequest("No file or an invalid one has been inputted.");
        //    }

            
        //    var path = Path.Combine(
        //        Directory.GetCurrentDirectory(),
        //        $"uploaded_file_{Guid.NewGuid()}.pdf");

        //    using (var stream = new FileStream(path, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return Ok("Your file has been uploaded successfully.");
        //}

        [HttpPost]
        public async Task<ActionResult> Upload(UploadDTO file)
        {
            if (file == null)
            {
                return NotFound();
            }

            _toDoListRepo.AddFile(file);

            await _toDoListRepo.SaveChangesAsync();

            return NoContent();

        }
    }
}


