using AutoMapper;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.Data_AccessLayer.Entities;

namespace ToDoListInfo.API.BusinessLayer.Profiles
{
    public class ToDoListProfile : Profile
    {
        public ToDoListProfile()
        {
            CreateMap<ToDoList, ToDoListDTO>();
            CreateMap<ToDoListForInsertDTO, ToDoList>();
            CreateMap<ToDoListForUpdateDTO, ToDoList>();
            CreateMap<ToDoList, ToDoListForUpdateDTO>();
            CreateMap<ToDoList,ToDoListDTO>()
    .ForMember(ToDoListDTO => ToDoListDTO.CreatedDate, ToDoList => ToDoList.MapFrom(src => new DateTime(src.CreatedDate.Value.Year, src.CreatedDate.Value.Month, src.CreatedDate.Value.Day)));

        }
    }
}
