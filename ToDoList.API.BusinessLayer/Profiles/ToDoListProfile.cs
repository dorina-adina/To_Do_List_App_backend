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

        }
    }
}
