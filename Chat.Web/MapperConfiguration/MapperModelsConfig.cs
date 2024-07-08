using AutoMapper;
using Chat.BLL.Models;
using Chat.DAL.Entities;
using Chat.Web.Models;

namespace Chat.Web.MapperConfiguration;

public class MapperModelsConfig : Profile
{
    public MapperModelsConfig()
    {
        CreateMap<User, UserModel>().ReverseMap();
        CreateMap<UserModel, UserViewModel>()
            .ForMember(r => r.JoinedRooms, opt => opt.MapFrom(r => r.JoinedRooms.Select(i => i.Name)))
            .ForMember(r => r.CreatedRooms, opt => opt.MapFrom(r => r.CreatedRooms.Select(i => i.Name)))
            .ReverseMap();
        
        CreateMap<Room, RoomModel>().ReverseMap();
        CreateMap<RoomViewModel, RoomModel>().ReverseMap();
        
        CreateMap<Message, MessageModel>().ReverseMap();
        CreateMap<MessageModel, MessageViewModel>()
            .ForMember(r => r.User, opt => opt.MapFrom(r => r.User.FullName))
            .ForMember(r => r.Room, opt => opt.MapFrom(r => r.Room.Name))
            .ReverseMap();
    }
}