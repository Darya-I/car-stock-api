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
        public async Task<CreateUserDTO> CreateMappedUserAsync(CreateUserDTO createUserDTO)
        {
            try
            {
                var mapper = new UserMapper();
                var user = mapper.MapCreateUserDtoToUser(createUserDTO);
                var result = await _userService.CreateUserAsync(user);
                return mapper.MapUserToCreateUserDto(result);
            }
            catch (Exception) 
            {
                throw;
            }
        }

        /// <summary>
        /// Вызывает UserService для поиска пользователя по почте, результат маппит на пользователя с списком его ролей
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <returns>Пользователь и список ролей</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<GetUsersDTO> GetMappedUserAsync(string email)
        {
            try
            {
                var mapper = new UserMapper();
                // Получить пользователя и его роли
                var userWithRoles = await _userService.GetUserAsync(email);
                if (userWithRoles == null)
                {
                    throw new InvalidOperationException($"User with email '{email}' was not found.");
                }
                // Извлечь пользователя и роли
                var user = userWithRoles.Value.user;
                var roles = userWithRoles.Value.roles;
                var userDto = mapper.MapUserToGetUsersDto(user);
                userDto.Roles = roles;
                return userDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Вызывает UserService для получения списка пользователей и их ролей, результат
        /// маппит на DTO пользователя
        /// </summary>
        /// <returns>Список пользователей с их ролями</returns>
        public async Task<IEnumerable<GetUsersDTO>> GetMappedUsersAsync()
        {
            try
            {
                var mapper = new UserMapper();
                var userDtos = new List<GetUsersDTO>();
                // Получить пользователей с ролями
                var usersWithRoles = await _userService.GetAllUsersAsync();
                foreach (var (user, roles) in usersWithRoles)
                {
                    // Смаппить пользователя на DTO
                    var userDto = mapper.MapUserToGetUsersDto(user);
                    // Добавить роли внутри списка DTO
                    userDto.Roles = roles;
                    userDtos.Add(userDto);
                }
                return userDtos;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Маппит DTO обновленного пользователя на User, передает в UserService
        /// </summary>
        /// <param name="updateUserDTO">Обновленный пользователь</param>
        /// <returns>Обновленный пользователь</returns>
        public async Task<UpdateUserDTO> UpdateMappedUserAsync(UpdateUserDTO updateUserDTO)
        {
            try
            {
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