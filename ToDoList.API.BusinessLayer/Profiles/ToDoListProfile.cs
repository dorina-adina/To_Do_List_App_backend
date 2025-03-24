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
    //        CreateMap<ToDoList,ToDoListDTO>()
    //.ForMember(ToDoListDTO => ToDoListDTO.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString(src.CreatedDate.Day.ToString())));
            CreateMap<UploadDTO, Upload>();

        }
    }
}
