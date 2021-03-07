using Mapster;
using ProductmanagementCore.Models;
using ProductmanagementCore.Models.Dto;
namespace ProductmanagementCore.Common
{
    public  class TypeAdapterConfigRegister : IRegister
    {


        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Users, UserDto>()
                .Map(d => d.Id, s => s.Id)
                .Map(d => d.FullName, s => $"{s.Firstname} {s.Lastname}")
                .Map(d => d.Products, s => s.Products);//lookup 
        }
    }
}
