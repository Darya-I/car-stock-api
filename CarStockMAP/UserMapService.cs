using CarStockBLL.CustomException;
using CarStockBLL.Interfaces;
using CarStockMAP.DTO.Auth;
using CarStockMAP.DTO.User;
using CarStockMAP.Mapping;


namespace CarStockMAP
{
    /// <summary>
    /// Сервис для маппинга пользователей
    /// </summary>
    public class UserMapService
    {
        /// <summary>
        /// Сервис операций над пользователями
        /// </summary>
        private readonly IUserService _userService;
        private readonly IAuthorizeUserService _authorizeUserService;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserMapService"/>.
        /// </summary>
        /// <param name="userService">Сервис операций над пользователями</param>
        public UserMapService(IUserService userService, IAuthorizeUserService authorizeUserService)
        {
            _userService = userService;
            _authorizeUserService = authorizeUserService;
        }

        ///<summary>
        /// Маппим клеймс пользователя от Гугла на User
        /// </summary>
        /// <param name="googleLoginRequestDTO"> Клеймс пользователя </param>
        /// <returns>Токен пользователя</returns>
        public async Task<string> MapGoogle(GoogleLoginRequestDTO googleLoginRequestDTO)
        {
            try
            {
                if (googleLoginRequestDTO == null)
                {
                    throw new ValidationErrorException("Invalid data");
                }
                var mapper = new UserMapper();
                var user = mapper.MapGoogleLoginRequestToUser(googleLoginRequestDTO);
                var token = await _authorizeUserService.ProcessGoogle(user);
                return token;
            }
            catch (Exception) 
            {
                throw;
            }
        }

        /// <summary>
        /// Метод маппит данные от входа пользователя на User, передает их UserService,
        /// результат от UserService маппит на токены и почту
        /// </summary>
        /// <param name="loginRequest">Данные пользователя со входа</param>
        /// <returns>Пользователь и его токен</returns>
        public async Task<LoginTokenDTO> MapUserLogin(LoginRequestDTO loginRequest)
        {
            try
            {
                if (loginRequest == null)
                {
                    throw new ValidationErrorException("Invalid data");
                }
                var mapper = new UserMapper();
                var user = mapper.MapLoginRequestToUser(loginRequest);
                // Метод возвращает пользователя и access token в метод маппинга
                var (authenticatedUser, accessToken) = await _authorizeUserService.Authenticate(user);
                var loginResult = mapper.MapUserToLoginTokenDto(authenticatedUser);
                loginResult.Token = accessToken;
                return loginResult;
            }
            catch (Exception) 
            {
                throw; 
            }
        }

        /// <summary>
        /// Метод маппит DTO нового пользователя на User, передает их UserService, результат маппит обратно
        /// </summary>
        /// <param name="createUserDTO">Данные добавляемого пользователя</param>
        /// <returns>Новый пользователь</returns>

        /// <summary>
        /// Вызывает UserService для поиска пользователя по почте, результат маппит на пользователя с списком его ролей
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <returns>Пользователь и список ролей</returns>
        /// <exception cref="InvalidOperationException"></exception>
       
        

        /// <summary>
        /// Вызывает UserService для получения списка пользователей и их ролей, результат
        /// маппит на DTO пользователя
        /// </summary>
        /// <returns>Список пользователей с их ролями</returns>
      

        /// <summary>
        /// Маппит DTO обновленного пользователя на User, передает в UserService
        /// </summary>
        /// <param name="updateUserDTO">Обновленный пользователь</param>
        /// <returns>Обновленный пользователь</returns>
        public async Task<UpdateUserDTO> UpdateMappedUserAsync(UpdateUserDTO updateUserDTO)
        {
            try
            {
                if (updateUserDTO == null) 
                {
                    throw new ValidationErrorException("Invalid data");
                }
                var mapper = new UserMapper();
                var user = mapper.MapUpdateUserDtoToUser(updateUserDTO);
                await _userService.UpdateUserAsync(user);
                if (!string.IsNullOrWhiteSpace(updateUserDTO.Role))
                {
                    await _userService.UpdateUserRoleAsync(updateUserDTO.Email, updateUserDTO.Role);
                }
                return updateUserDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}