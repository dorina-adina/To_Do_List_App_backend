using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.Data_AccessLayer.Repos;


namespace ToDoList.API.Presentation_Layer.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/lists")]
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


        // se obtin task-urile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoListDTO>>> GetLists()
        {
            var listsEntities = await _toDoListRepo.GetLists();

            return Ok(_mapper.Map<IEnumerable<ToDoListInfo.API.DBLayer.Entities.ToDoList>, IEnumerable<ToDoListDTO>>(listsEntities));


        }

        // se obtine un task dupa id-ul dat
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

        // se obtine un task dupa id-ul user-ului dat
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

        // se adauga un task
        [HttpPost]
        public async Task<ActionResult> InsertToDoList(ToDoListForInsertDTO toDoList)
        {

            try
            {

                if (toDoList == null)
                {
                    return NotFound("List is null");
                }

                var listForInsert = _mapper.Map<ToDoListForInsertDTO, ToDoListInfo.API.DBLayer.Entities.ToDoList>(toDoList);

                var item = await _toDoListRepo.AddList(listForInsert);

                return Ok(toDoList);
            }


            catch (Exception ex)
            {
                return StatusCode(500, "Server error");
            }


        }

        // se modifica un task
        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateToDoList(int Id,
            ToDoListForUpdateDTO toDoList)
        {

            try
            {
                var toDoListEntity = await _toDoListRepo.GetListAsync(Id);

                if (toDoListEntity == null)
                {
                    return NotFound("List is null");
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

        // se sterge un task
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteToDoList(int Id)
        {
            try
            {
                var toDoListEntity = await _toDoListRepo.GetListAsync(Id);

                if (toDoListEntity == null)
                {
                    return NotFound("List is null");
                }

                _toDoListRepo.DeleteList(Id);

                return Ok("Succes!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error");
            }
        }

        // se incarca un fisier
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, int idOwner, string emailOwner, string text, int idTask)
        {
            try
            {
                if (file == null || file.Length == 0 || file.Length > 20971520)
                {
                    return BadRequest("No file uploaded!");
                }

                string[] permittedExtensions = { "image/img", "image/jpg", "image/jpeg", "image/gif", "image/bmp", "image/jfif", "image/png" }; 

                var ext = file.ContentType.ToLower();

                if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))  // verificare extensie fisier
                {
                    return BadRequest("Wrong file type uploaded");
                }

                var fileName = Path.GetFileName(file.FileName);  // nume fisier
                var filePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    $"_{fileName}");  // cale fisier

                Bitmap bitmap;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    ms.Position = 0;
                    bitmap = new System.Drawing.Bitmap(ms);
                }

                text += "#END";  // se adauga la finalul mesajului de decodificat pentru a putea obtine mai usor mesajul la decodificare 

                int index = 0;  // se va parcurge mesajul
                byte R, G, B;  // canalele de culoare, rosu, verde, albastru
                char c = '\0';
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        if(index >= text.Length)
                        {
                            break;
                        }

                        Color pixel = bitmap.GetPixel(i, j);  // se obtine pixelul, imaginea e parcursa de sus in jos, de la stanga spre dreapta

                        R = pixel.R;
                        G = pixel.G;
                        B = pixel.B;

                        if (index < text.Length)
                        {
                            c = (char)text[index];  // caracterul c din text
                        } 
                        R = (byte)((R >> 3) << 3);  // se pregatesc ultimii biti, se fac 0 ultimii 3, 3, respectiv 2 biti
                        G = (byte)((G >> 3) << 3);
                        B = (byte)((B >> 2) << 2); 

                        R += (byte)((c & 0b11100000) >> 5);  // se adauga informatia din caracterul c, informatia de pe bitii 7, 6, 5, shift la dreapta cu 5
                        G += (byte)((c & 0b00011100) >> 2);  // informatia de pe bitii 4, 3, 2
                        B += (byte)(c & 0b00000011);  //informatia de pe bitii 1, 0
                        index++;

                        Color newPixel = Color.FromArgb(R, G, B);  //noul pixel
                        bitmap.SetPixel(i, j, newPixel);  //imaginea e actualizata cu noul pixel

                    }
                }

                using (MemoryStream mss = new MemoryStream())  //se converteste in format png
                {
                    bitmap.Save(filePath, ImageFormat.Png);
                    mss.Seek(0, SeekOrigin.Begin);
                }
                

                var fileUpload = await _toDoListRepo.AddFileAsync(fileName, filePath, idOwner, emailOwner, text, idTask);  //se salveaza fisierul

                _logger.LogInformation($"File {fileName} was uploaded succesfully!");

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

        [HttpPost("download")]
        public async Task<string> DownloadFile(IFormFile file)
        {
       
            Bitmap bitmap;

            using (var ms = new MemoryStream())
            {
                file.CopyToAsync(ms);
                ms.Position = 0;
                bitmap = new System.Drawing.Bitmap(ms);
            }
            string message = "";  // variabila in care se va salva mesajul
            int index = 0;  
            byte R, G, B;
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {

                        Color pixel = bitmap.GetPixel(i, j);  // se obtine pixelul

                        R = pixel.R;
                        G = pixel.G;
                        B = pixel.B;

                        R = (byte)(R & 0b00000111);  // se pastreaza ultimii 3, 3, respectiv 2 biti
                        G = (byte)(G & 0b00000111);
                        B = (byte)(B & 0b00000011);

                        char c = (char)((R << 5) + (G << 2) + B);  // se formeaza caracterul
                        if (c == 0 || c.CompareTo('#') == 0)  // verificare conditii
                        {
                            return message;
                        }
                        message += c;  // caracterul se adauga mesajului
                    Color newPixel = Color.FromArgb(R, G, B);  
                        bitmap.SetPixel(i, j, newPixel);  // se seteaza noul pixel

                    }
                }

            return message;  // mesajul obtinut se returneaza

        }

        // se obtin fisierele
        [HttpGet("files")]
        public async Task<ActionResult<IEnumerable<UploadDTO>>> GetFiles()
        {
            var files = await _toDoListRepo.GetFiles();

            return Ok(_mapper.Map<IEnumerable<ToDoListInfo.API.DBLayer.Entities.Upload>, IEnumerable<UploadDTO>>(files));


        }

        // se obtin fisierele dupa id0ul user-ului
        [HttpGet("files/ownerFiles/{idOwner}", Name = "GetFilesByOwner")]
        public async Task<ActionResult<IEnumerable<UploadDTO>>> GetFilesByOwner(
            int idOwner)
        {
            try
            {
                var files = await _toDoListRepo
                    .GetFilesCreatedByAsync(idOwner);

                if (files == null)
                {
                    return NotFound($"Files with ID {idOwner} not exist");
                }

                return Ok(_mapper.Map<IEnumerable<UploadDTO>>(files));

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error");
            }
        }
        
        // se obtine fisierul dupa id
        [HttpGet("files/{fileId}", Name = "GetFileById")]
        public async Task<ActionResult> GetFileById(int fileId)
        {

            try
            {
                var result = await _toDoListRepo
                    .GetFileById(fileId);

                if (result == null)
                {
                    return NotFound($"File with ID {fileId} not exist");
                }


                var file = _mapper.Map<UploadDTO>(result);

                var path = file.Path;

                if (!System.IO.File.Exists(path))
                    return NotFound();

                string MimeType = "image/jpg";

                var fileName = file.Name;


                var fileBytes = System.IO.File.ReadAllBytes(path);

                return File(fileBytes, MimeType, fileName);  // va fi trimis sub forma de blob

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error");
            }

            }
        }
}