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

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UserMapService"/>.
        /// </summary>
        /// <param name="userService">Сервис операций над пользователями</param>
        public UserMapService(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Маппим клеймс пользователя от Гугла на User
        /// </summary>
        /// <param name="googleLoginRequest"> Клеймс пользователя </param>
        /// <value</value>
        /// <returns>DTO пользователя</returns>
        public async Task<GoogleUserDTO> MapGoogleUser(GoogleLoginRequestDTO googleLoginRequest)
        {
            var mapper = new UserMapper();
            
            var user = mapper.MapGoogleLoginRequestToUser(googleLoginRequest);
            
            var (normalUser, accessToken) = await _userService.HandleGoogleUser(user);
            
            var googleUser = mapper.MapGoogleToUser(normalUser);
            
            return googleUser;
        }

        /// <summary>
        /// После аутентификации пользователя через Гугл, маппит его для UserService,
        /// чтобы получить токен
        /// </summary>
        /// <param name="googleLoginRequest"> Клеймс пользователя</param>
        /// <returns>Пользователь и его токен</returns>
        public async Task<LoginTokenDTO> MapGoogleUserLogin(GoogleLoginRequestDTO googleLoginRequest)
        {
            var mapper = new UserMapper();
            
            var user = mapper.MapGoogleLoginRequestToUser(googleLoginRequest);

            var (authenticatedUser, accessToken) = await _userService.GoogleAuthenticate(user);
            
            var googleLoginResult = mapper.MapUserToLoginTokenDto(authenticatedUser);
            
            googleLoginResult.Token = accessToken;
            
            return googleLoginResult;
        }

        /// <summary>
        /// Метод маппит данные от входа пользователя на User, передает их UserService,
        /// результат от UserService маппит на токены и почту
        /// </summary>
        /// <param name="loginRequest">Данные пользователя со входа</param>
        /// <returns>Пользователь и его токен</returns>
        public async Task<LoginTokenDTO> MapUserLogin(LoginRequestDTO loginRequest)
        {
            var mapper = new UserMapper();
            
            var user = mapper.MapLoginRequestToUser(loginRequest);

            // метод возвращает пользователя и access token в метод маппинга
            var (authenticatedUser, accessToken) = await _userService.Authenticate(user);
            
            var loginResult = mapper.MapUserToLoginTokenDto(authenticatedUser);
            
            loginResult.Token = accessToken;
            
            return loginResult;
        }

        /// <summary>
        /// Метод маппит DTO нового пользователя на User, передает их UserService, результат маппит обратно
        /// </summary>
        /// <param name="createUserDTO">Данные добавляемого пользователя</param>
        /// <returns>Новый пользователь</returns>
        public async Task<CreateUserDTO> CreateMappedUserAsync(CreateUserDTO createUserDTO)
        {
            var mapper = new UserMapper();
            
            var user = mapper.MapCreateUserDtoToUser(createUserDTO);

            var result = await _userService.CreateUserAsync(user);
            
            return mapper.MapUserToCreateUserDto(result);
        }

        /// <summary>
        /// Вызывает UserService для поиска пользователя по почте, результат маппит на пользователя с списком его ролей
        /// </summary>
        /// <param name="email">Почта пользователя</param>
        /// <returns>Пользователь и список ролей</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<GetUsersDTO> GetMappedUserAsync(string email)
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

        /// <summary>
        /// Вызывает UserService для получения списка пользователей и их ролей, результат
        /// маппит на DTO пользователя
        /// </summary>
        /// <returns>Список пользователей с их ролями</returns>
        public async Task<IEnumerable<GetUsersDTO>> GetMappedUsersAsync()
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

        /// <summary>
        /// Маппит DTO обновленного пользователя на User, передает в UserService
        /// </summary>
        /// <param name="updateUserDTO">Обновленный пользователь</param>
        /// <returns>Обновленный пользователь</returns>
        public async Task<UpdateUserDTO> UpdateMappedUserAsync(UpdateUserDTO updateUserDTO)
        {
            var mapper = new UserMapper();

            var user = mapper.MapUpdateUserDtoToUser(updateUserDTO);

            await _userService.UpdateUserAsync(user);

            return updateUserDTO;
        }
    }
}
