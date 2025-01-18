using AutoMapper;
using EMS.Models;
using EMS.Models.DTO.UserDTO;

namespace EMS.Profiles
{
    public class MappingProfile:Profile
    {

        public MappingProfile()
        {   

            //Value of the Email to be Mapped to UserName
           CreateMap<AddUSerDTO , User>().
                ForMember(dest=>dest.UserName, u=>u.MapFrom(reg=>reg.Email));
        }
    }
}
