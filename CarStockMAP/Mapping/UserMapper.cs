using CarStockDAL.Models;
using CarStockMAP.DTO;
using CarStockMAP.Models;
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
        public partial User MapCreateUserDtoToUser(CreateUserDTO createUserDTO);


        [MapProperty(nameof(User.Email), nameof(CreateUserDTO.Email))]
        public partial CreateUserDTO MapUserToCreateUserDto(User user);


        [MapProperty(nameof(updateUserDTO.Email), nameof(User.Email))]
        [MapProperty(nameof(updateUserDTO.UserName), nameof(User.UserName))]
        [MapProperty(nameof(updateUserDTO.Password), nameof(User.PasswordHash))]
        public partial User MapUpdateUserDtoToUser(UpdateUserDTO updateUserDTO);


        [MapProperty(nameof(GetUsersDTO.UserName), nameof(user.UserName))]
        [MapProperty(nameof(GetUsersDTO.Email), nameof(user.Email))]
        public partial GetUsersDTO MapUserToGetUsersDto(User user);


        [MapProperty(nameof(LoginRequest.Email), nameof(User.Email))]
        [MapProperty(nameof(LoginRequest.Password), nameof(User.PasswordHash))]
        public partial User MapLoginRequestToUser(LoginRequest loginRequest);

        [MapProperty(nameof(User.Email), nameof(LoginTokenDto.Email))]
        [MapProperty(nameof(User.RefreshToken), nameof(LoginTokenDto.RefreshToken))]
        public partial LoginTokenDto MapUserToLoginTokenDto(User user);

    }
}
