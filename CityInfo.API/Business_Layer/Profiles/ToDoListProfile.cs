using AutoMapper;
using CityInfo.API.Business_Layer.Models;
using CityInfo.API.Data_Access_Layer.Entities;

namespace CityInfo.API.Business_Layer.Profiles
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
