using AutoMapper;
using ToDoListInfo.API.BusinessLayer.Models;
using ToDoListInfo.API.DBLayer.Entities;

namespace ToDoListInfo.API.BusinessLayer.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Users, UserDTO>();
            CreateMap<UserInsertDTO, Users>();

        }
    }

}
