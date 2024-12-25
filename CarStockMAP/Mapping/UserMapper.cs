using CarStockDAL.Models;
using CarStockMAP.DTO;
using Microsoft.AspNetCore.Identity;
using Riok.Mapperly.Abstractions;

namespace CarStockMAP.Mapping
{
    [Mapper]
    public partial class UserMapper
    {
        [MapProperty(nameof(CreateUserDTO.Email), nameof(User.Email))]
        [MapProperty(nameof(CreateUserDTO.Password), nameof(User.PasswordHash))]
        [MapProperty(nameof(CreateUserDTO.Email), nameof(User.UserName))]
        public partial User MapToUser(CreateUserDTO createUserDTO);


        [MapProperty(nameof(User.Email), nameof(CreateUserDTO.Email))]
        public partial CreateUserDTO MapToCreateUserDto(User user);

        [MapProperty(nameof(updateUserDTO.Email), nameof(User.Email))]
        [MapProperty(nameof(updateUserDTO.UserName), nameof(User.UserName))]
        [MapProperty(nameof(updateUserDTO.Password), nameof(User.PasswordHash))]
        public partial User MapToUser(UpdateUserDTO updateUserDTO);

        //[MapProperty(nameof(User.Email), nameof(getUsersDTO.Email))]
        //[MapProperty(nameof(User.UserName), nameof(getUsersDTO.UserName))]
        //public partial User MapToUsersDto(GetUsersDTO getUsersDTO);

        [MapProperty(nameof(GetUsersDTO.UserName), nameof(user.UserName))]
        [MapProperty(nameof(GetUsersDTO.Email), nameof(user.Email))]
        public partial GetUsersDTO MapToUsersDto(User user);

    }
}
