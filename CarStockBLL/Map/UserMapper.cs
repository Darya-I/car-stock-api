using CarStockBLL.DTO.Account;
using CarStockBLL.DTO.Auth;
using CarStockBLL.DTO.User;
using CarStockDAL.Models;
using Riok.Mapperly.Abstractions;

namespace CarStockBLL.Map
{
    [Mapper]
    public partial class UserMapper
    {
        /// <summary>
        /// Маппинг с объекта пользователя на DTO
        /// </summary>
        /// <param name="user">Объект пользователя</param>
        /// <returns>DTO представления пользователя</returns>
        [MapProperty(nameof(user.Email), nameof(GetUserDTO.Email))]
        [MapProperty(nameof(user.UserName), nameof(GetUserDTO.UserName))]
        [MapProperty(nameof(user.Role.Name), nameof(GetUserDTO.RoleName))]
        public partial GetUserDTO UserToGetUserDto(User user);

        /// <summary>
        /// Маппинг с DTO пользователя при создании на объект
        /// </summary>
        /// <param name="userDto">DTO пользователя</param>
        /// <returns>Объект пользователя</returns>
        [MapProperty(nameof(userDto.Email), nameof(User.Email))]
        [MapProperty(nameof(userDto.Email), nameof(User.UserName))]
        [MapProperty(nameof(userDto.Password), nameof(User.PasswordHash))]
        [MapProperty(nameof(userDto.RoleId), nameof(User.RoleId))]
        public partial User UserDtoToUser(CreateUserDTO userDto);

        /// <summary>
        /// Маппинг с DTO пользователя при обновлении на объект
        /// </summary>
        /// <param name="userDto">DTO пользователя</param>
        /// <returns>Объект пользователя</returns>
        [MapProperty(nameof(userDto.Email), nameof(User.Email))]
        [MapProperty(nameof(userDto.UserName), nameof(User.UserName))]
        [MapProperty(nameof(userDto.Password), nameof(User.PasswordHash))]
        [MapProperty(nameof(userDto.RoleId), nameof(User.RoleId))]
        public partial User UpadateDtoToUser(UpdateUserDTO userDto);

        /// <summary>
        /// Маппинг с модели запроса на регистрацию пользователя на объект
        /// </summary>
        /// <param name="userRequest">Модель запроса пользователя</param>
        /// <returns>Объект пользователя</returns>
        [MapProperty(nameof(userRequest.UserName), nameof(User.UserName))]
        [MapProperty(nameof(userRequest.Email), nameof(User.Email))]
        [MapProperty(nameof(userRequest.Password), nameof(User.PasswordHash))]
        public partial User RegisterRequestToUser(RegisterRequest userRequest);

        /// <summary>
        /// Маппинг с модели запроса на редактирование пользователя на объект
        /// </summary>
        /// <param name="userRequest">Модель запроса пользователя</param>
        /// <returns>Объект пользователя</returns>
        [MapProperty(nameof(userRequest.UserName), nameof(User.UserName))]
        [MapProperty(nameof(userRequest.Email), nameof(User.Email))]
        [MapProperty(nameof(userRequest.Password), nameof(User.PasswordHash))]
        public partial User EditRequestToUser(EditRequest userRequest);

        /// <summary>
        /// Маппинг с модели запроса при аутентификации через гугл на объект пользователя
        /// </summary>
        /// <param name="userRequest">Модель запроса аутентификации</param>
        /// <returns>Объект пользователя</returns>
        [MapProperty(nameof(userRequest.Email), nameof(User.Email))]
        [MapProperty(nameof(userRequest.Email), nameof(User.UserName))]
        public partial User GoogleRequestToUser(GoogleLoginRequest userRequest);

        /// <summary>
        /// Маппинг с модели запроса логина пользователя на объект
        /// </summary>
        /// <param name="userRequest">Модель запроса на логин</param>
        /// <returns>Объект пользователя</returns>
        [MapProperty(nameof(userRequest.Email), nameof(User.Email))]
        [MapProperty(nameof(userRequest.Password), nameof(User.PasswordHash))]
        public partial User LoginRequestToUser(LoginRequest userRequest);
    }
}