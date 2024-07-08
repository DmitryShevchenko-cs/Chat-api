using AutoMapper;
using Chat.BLL.Models;
using Chat.DAL.Entities;

namespace Chat.Web.MapperConfiguration;

public class MapperModelsConfig : Profile
{
    public MapperModelsConfig()
    {
        CreateMap<User, UserModel>().ReverseMap();
        
        CreateMap<Room, RoomModel>().ReverseMap();
        CreateMap<Message, MessageModel>().ReverseMap();
    }
}