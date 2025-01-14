using CarStockDAL.Models;
using CarStockMAP.DTO.Auth;
using CarStockMAP.DTO.User;
using Microsoft.AspNetCore.Identity;
using Riok.Mapperly.Abstractions;

namespace CarStockMAP.Mapping
{
    [Mapper]
    public partial class UserMapper
    {
        /// <summary>
        /// Маппинг из CreateUserDTO в User
        /// </summary>
        [MapProperty(nameof(CreateUserDTO.Email), nameof(User.Email))]
        [MapProperty(nameof(CreateUserDTO.Password), nameof(User.PasswordHash))]
        [MapProperty(nameof(CreateUserDTO.Email), nameof(User.UserName))]
        public partial User MapCreateUserDtoToUser(CreateUserDTO createUserDTO);

        /// <summary>
        /// Маппинг из User в CreateUserDTO
        /// </summary>
        [MapProperty(nameof(User.Email), nameof(CreateUserDTO.Email))]
        public partial CreateUserDTO MapUserToCreateUserDto(User user);

        /// <summary>
        /// Маппинг из UpdateUserDTO в User
        /// </summary>
        [MapProperty(nameof(updateUserDTO.Email), nameof(User.Email))]
        [MapProperty(nameof(updateUserDTO.UserName), nameof(User.UserName))]
        [MapProperty(nameof(updateUserDTO.Password), nameof(User.PasswordHash))]
        public partial User MapUpdateUserDtoToUser(UpdateUserDTO updateUserDTO);

        /// <summary>
        /// Маппинг из User в запрашеваемого пользователя (роли прикрепляются отдельно)
        /// </summary>
        [MapProperty(nameof(GetUsersDTO.UserName), nameof(user.UserName))]
        [MapProperty(nameof(GetUsersDTO.Email), nameof(user.Email))]
        public partial GetUsersDTO MapUserToGetUsersDto(User user);

        /// <summary>
        /// Маппинг с данных для входа в User
        /// </summary>
        [MapProperty(nameof(LoginRequestDTO.Email), nameof(User.Email))]
        [MapProperty(nameof(LoginRequestDTO.Password), nameof(User.PasswordHash))]
        public partial User MapLoginRequestToUser(LoginRequestDTO loginRequest);

        /// <summary>
        /// Маппинг User в DTO токена входа
        /// </summary>
        [MapProperty(nameof(User.Email), nameof(LoginTokenDto.Email))]
        [MapProperty(nameof(User.RefreshToken), nameof(LoginTokenDto.RefreshToken))]
        public partial LoginTokenDto MapUserToLoginTokenDto(User user);

        /// <summary>
        /// Маппинг данных входа через Google в User
        /// </summary>
        [MapProperty(nameof(GoogleLoginRequestDTO.Email), nameof(User.Email))]
        [MapProperty(nameof(GoogleLoginRequestDTO.Email), nameof(User.UserName))]
        public partial User MapGoogleLoginRequestToUser(GoogleLoginRequestDTO googleLoginRequest);

        /// <summary>
        /// Маппинг User в GoogleUserDTO
        /// </summary>
        [MapProperty(nameof(User.Email), nameof(GoogleUserDTO.Email))]
        [MapProperty(nameof(User.UserName), nameof(GoogleUserDTO.Email))]
        public partial GoogleUserDTO MapGoogleToUser(User user);

    }
}
