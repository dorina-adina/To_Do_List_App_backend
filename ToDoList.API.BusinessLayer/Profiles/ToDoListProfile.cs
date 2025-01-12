using AutoMapper;
using ToDoList.API.BusinessLayer.Models;

namespace ToDoList.API.BusinessLayer.Profiles
{
    public class ToDoListProfile : Profile
    {
        public ToDoListProfile()
        {
            CreateMap<Data_AccessLayer.Entities.ToDoList, ToDoListDTO>();
            CreateMap<ToDoListForInsertDTO, Data_AccessLayer.Entities.ToDoList>();
            CreateMap<ToDoListForUpdateDTO, Data_AccessLayer.Entities.ToDoList>();
            CreateMap<Data_AccessLayer.Entities.ToDoList, ToDoListForUpdateDTO>();

        }
    }
}
