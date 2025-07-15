using AutoMapper;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.Data_AccessLayer.Helpers;
using ToDoListInfo.API.DBLayer.Entities;

namespace ToDoListInfo.API.BusinessLayer.Profiles
{
    public class ToDoListProfile : Profile
    {
        public ToDoListProfile()
        {
            CreateMap<ToDoList, ToDoListDTO>()
            .ForMember(ToDoListDTO => ToDoListDTO.CreatedDate, opt => opt.MapFrom(src => DateHelper.DateFormat(src.CreatedDate)))
            .ForMember(ToDoListDTO => ToDoListDTO.DueDate, opt => opt.MapFrom(src => DateAndTimeHelper.DateFormat(src.DueDate)));

            CreateMap<ToDoListForInsertDTO, ToDoList>()
            .ForMember(ToDoList => ToDoList.DueDate, opt => opt.MapFrom(src => DateHelper.ReverseDate(src.DueDate)));

            CreateMap<ToDoListForUpdateDTO, ToDoList>()
             .ForMember(ToDoList => ToDoList.DueDate, opt => opt.MapFrom(src => DateHelper.ReverseDate(src.DueDate)));

            CreateMap<Upload, UploadDTO>()
            .ForMember(UploadDTO => UploadDTO.CreatedDate, opt => opt.MapFrom(src => DateHelper.DateFormat(src.CreatedDate)));
;

        }
    }
}
