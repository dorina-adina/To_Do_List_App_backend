using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
