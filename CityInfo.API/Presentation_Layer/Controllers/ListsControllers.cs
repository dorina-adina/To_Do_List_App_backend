using Asp.Versioning;
using AutoMapper;
using CityInfo.API.Business_Layer.Models;
using CityInfo.API.Business_Layer.Services;
using CityInfo.API.Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Mvc;


namespace CityInfo.API.Presentation_Layer.Controllers
{

    [ApiController]
    //[Authorize]
    [Route("api/v{version:apiVersion}/lists")]
    //[Route("api/[controller]")]
    [ApiVersion(3)]


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

        [HttpPost]
        public async Task<ActionResult<ToDoListDTO>> InsertToDoList(
            ToDoListForInsertDTO toDoList)
        {
            // nu merge :'(
            var finalToDoList = _mapper.Map<ToDoList>(toDoList);

            var createdToDoList = _mapper.Map<ToDoListDTO>(finalToDoList);

            return CreatedAtRoute("GetToDoList",
                 new
                 {
                     toDoListId = finalToDoList.Id + 1
                 },
                 createdToDoList);
        }

        //[HttpPut("{todolistId}")]
        //public async Task<ActionResult> UpdateToDoList (int todolistId,
        //    ToDoListForUpdateDTO toDoList)
        //{

        //}

        [HttpDelete("{todolistId}")]
        public async Task<ActionResult> DeleteToDoList(int todolistId)
        {
            //if (!await _toDoListRepo.ListExistsAsync(todolistId))
            //{

            //    return NotFound();
            //}

            var listaa = await _toDoListRepo.GetListAsync(todolistId);

            if (listaa == null)
            {
                return NotFound();
            }

            _toDoListRepo.DeleteList(listaa);
            await _toDoListRepo.SaveChangesAsync();

            return NoContent;
        }

    }
}
