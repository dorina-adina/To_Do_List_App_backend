using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.Data_AccessLayer.Repos;
using System.Drawing;
using System.Drawing.Imaging;



namespace ToDoList.API.Presentation_Layer.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/v{version:apiVersion}/lists")]
    //[Route("api/[controller]")]
    [Asp.Versioning.ApiVersion(1)]


    public class ListsControllers : ControllerBase
    {

        private readonly IToDoListRepo _toDoListRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<ListsControllers> _logger;


        public ListsControllers(IToDoListRepo toDoListRepo, IMapper mapper, ILogger<ListsControllers> logger)
        {
            _toDoListRepo = toDoListRepo ??
                throw new ArgumentNullException(nameof(toDoListRepo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoListDTO>>> GetLists()
        {
            var listsEntities = await _toDoListRepo.GetLists();

            return Ok(_mapper.Map<IEnumerable<ToDoListInfo.API.DBLayer.Entities.ToDoList>, IEnumerable<ToDoListDTO>>(listsEntities));


        }

        [HttpGet("{ListId}", Name = "GetToDoList")]
        public async Task<ActionResult<ToDoListDTO>> GetToDoList(
            int ListId)
        {
            try
            {
                var toDoList = await _toDoListRepo
                    .GetListAsync(ListId);

                if (toDoList == null)
                {
                    return NotFound($"List with ID {ListId} not exist");
                }

                return Ok(_mapper.Map<ToDoListDTO>(toDoList));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error");
            }

        }


        [HttpGet("owner/{idOwner}", Name = "GetToDoListByOwner")]
        public async Task<ActionResult<IEnumerable<ToDoListDTO>>> GetToDoListByOwner(
            int idOwner)
        {

            try {
                var toDoLists = await _toDoListRepo
                    .GetListCreatedByAsync(idOwner);

                if (toDoLists == null)
                {
                    return NotFound($"List with ID {idOwner} not exist");
                }

                return Ok(_mapper.Map<IEnumerable<ToDoListDTO>>(toDoLists));

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertToDoList(ToDoListForInsertDTO toDoList)
        {

            try
            {

                if (toDoList == null)
                {
                    return NotFound($"List is null");
                }

                //var listForInsert = _mapper.Map<ToDoListForInsertDTO, ToDoListInfo.API.DBLayer.Entities.ToDoList>(toDoList);

                //var item = await _toDoListRepo.AddList(listForInsert);

                //return NoContent();

                var listForInsert = _mapper.Map<ToDoListForInsertDTO, ToDoListInfo.API.DBLayer.Entities.ToDoList>(toDoList);

                var item = await _toDoListRepo.AddList(listForInsert);

                return Ok(toDoList);
            }


            catch (Exception ex)
            {
                return StatusCode(500, "Server error");
            }


        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateToDoList(int Id,
            ToDoListForUpdateDTO toDoList)
        {
            //var toDoListEntity = await _toDoListRepo.GetListAsync(Id);

            //if (toDoListEntity == null)
            //{
            //    return NotFound();
            //}

            //var updatedList = _mapper.Map<ToDoListForUpdateDTO, ToDoListInfo.API.DBLayer.Entities.ToDoList>(toDoList);

            //var item = await _toDoListRepo.UpdateList(Id, updatedList);

            //return NoContent();

            try
            {
                var toDoListEntity = await _toDoListRepo.GetListAsync(Id);

                if (toDoListEntity == null)
                {
                    return NotFound($"List is null");
                }

                var updatedList = _mapper.Map<ToDoListForUpdateDTO, ToDoListInfo.API.DBLayer.Entities.ToDoList>(toDoList);

                var item = await _toDoListRepo.UpdateList(Id, updatedList);

                return Ok(item);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Server error");

            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteToDoList(int Id)
        {
            try
            {
                var toDoListEntity = await _toDoListRepo.GetListAsync(Id);

                if (toDoListEntity == null)
                {
                    return NotFound($"List is null");
                }

                _toDoListRepo.DeleteList(Id);

                return Ok("Succes!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error");
            }
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, int idOwner, string emailOwner, string text)
        {
            try
            {
                if (file == null || file.Length == 0 || file.Length > 20971520)
                {
                    return BadRequest("No file uploaded!");
                }



                string[] permittedExtensions = { "image/img", "image/jpg", "image/jpeg", "image/gif", "image/bmp", "image/jfif" };

                //var ext = Path.GetExtension(file.ContentType).ToLowerInvariant();
                var ext = file.ContentType.ToLower();

                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
                {
                    return BadRequest("Wrong file type uploaded!");
                }

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    $"_{fileName}");

                //byte[] bytes = Encoding.ASCII.GetBytes(text);

                byte[] bytes = Encoding.UTF8.GetBytes(text);

                //string binary = string.Join("", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

                string binary = "";

                binary += bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'));


                //string binary = "";
                //foreach (char item in text)
                //{
                //    //text
                //    byte[] bytes = Encoding.UTF8.GetBytes(text);
                //    binary += bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));
                //}


                //var bitmap = new Bitmap(fileName);

                Bitmap bitmap;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    ms.Position = 0;
                    bitmap = new System.Drawing.Bitmap(ms);
                }

                int index = 0;
                byte R, G, B;
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if(index >= binary.Length)
                        {
                            break;
                        }

                        Color pixel = bitmap.GetPixel(i, j);

                        R = pixel.R;
                        G = pixel.G;
                        B = pixel.B;

                        if (index < binary.Length)
                        {
                            R = (byte)((R & 0xFE) | (binary[index++] - '0'));
                        }
                        if (index < binary.Length)
                        {
                            G = (byte)((G & 0xFE) | (binary[index++] - '0'));
                        }
                        if (index < binary.Length)
                        {
                            B = (byte)((B & 0xFE) | (binary[index++] - '0'));
                        }

                        Color newPixel = Color.FromArgb(R, G, B);
                        bitmap.SetPixel(i, j, newPixel);

                    }
                }

                using (MemoryStream mss = new MemoryStream())
                {
                    bitmap.Save(filePath, ImageFormat.Jpeg);
                    mss.Seek(0, SeekOrigin.Begin);
                    return Ok();
                }

                //var mss = new MemoryStream();
                //bitmap.Save(mss, ImageFormat.Png);
                //mss.Seek(0, SeekOrigin.Begin);
                

                var fileUpload = await _toDoListRepo.AddFileAsync(fileName, filePath, idOwner, emailOwner, text);

                //return Ok(bitmap.GetPixel(0,0));


                //var fileName = Path.GetFileName(file.FileName);
                //var filePath = Path.Combine(
                //    Directory.GetCurrentDirectory(),
                //    $"_{fileName}");

                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await file.CopyToAsync(stream);
                //}

                //var fileUpload = await _toDoListRepo.AddFileAsync(fileName, filePath, idOwner, emailOwner, text);

                _logger.LogInformation("File {FileName} was uploaded succesfully!", fileName);

                return Ok(new { fileUpload.Name, fileUpload.Path });
            }

            catch (FileNotFoundException ex)
            {
                return StatusCode(404, "File not found");
            }
            catch (FileLoadException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}