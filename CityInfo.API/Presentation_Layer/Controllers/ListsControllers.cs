using System.Collections.Generic;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.BusinessLayer.Repos;
using ToDoListInfo.API.Data_AccessLayer.Entities;
using ToDoListInfo.API.Data_AccessLayer.Services;


namespace ToDoList.API.Presentation_Layer.Controllers
{
    //"DbConnectionString": "Server=BTCCLPF1PMR0J\\SQLTESTSERVER;Database=DbTest;UserId=sa;Password=BT.Cj#9628517;TrustedConnection=True;",

    [ApiController]
    //[Authorize]
    [Route("api/v{version:apiVersion}/lists")]
    //[Route("api/[controller]")]
    [Asp.Versioning.ApiVersion(3)]


    public class ListsControllers : ControllerBase
    {

        private readonly IToDoListRepo _toDoListRepo;
        private readonly IMapper _mapper;

        public ListsControllers(IToDoListRepo toDoListRepo, IMapper mapper)
        {
            _toDoListRepo = toDoListRepo ??
                throw new ArgumentNullException(nameof(toDoListRepo));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoListDTO>>> GetLists()
        {
            var listsEntities = await _toDoListRepo.GetListsAsync();

            return Ok(_mapper.Map<IEnumerable<ToDoListDTO>>(listsEntities));

        }

        [HttpGet("{ListId}", Name = "GetToDoList")]
        public async Task<ActionResult<ToDoListDTO>> GetToDoList(
            int ListId)
        {

            var toDoList = await _toDoListRepo
                .GetListAsync(ListId);

            if (toDoList == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ToDoListDTO>(toDoList));

        }

        [HttpPost]
        public async Task<ActionResult<ToDoListDTO>> InsertToDoList(ToDoListForInsertDTO toDoList)
        {
        
            if (toDoList == null)
            {
                return NotFound();
            }

            _toDoListRepo.AddList(toDoList);

            await _toDoListRepo.SaveChangesAsync();

            return NoContent();

        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> UpdateToDoList(int Id,
            ToDoListForUpdateDTO toDoList)
        {
            var toDoListEntity = await _toDoListRepo.GetListAsync(Id);

            if (toDoListEntity == null)
            {
                return NotFound();
            }

            _toDoListRepo.UpdateList(Id, toDoList);

            await _toDoListRepo.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteToDoList(int Id)
        {
            
            _toDoListRepo.DeleteList(Id);
            await _toDoListRepo.SaveChangesAsync();

            return NoContent();
        }

        //[HttpPost]
        //public async Task<ActionResult> Upload(UploadDTO file)
        //{
        //    if (file == null)
        //    {
        //        return NotFound();
        //    }

        //    _toDoListRepo.AddFile(file);

        //    await _toDoListRepo.SaveChangesAsync();

        //    return NoContent();

        //}


    }
}