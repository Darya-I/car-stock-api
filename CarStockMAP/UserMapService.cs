using CarStockBLL.Interfaces;
using CarStockBLL.Models;
using CarStockMAP.DTO.Auth;
using CarStockMAP.DTO.User;
using CarStockMAP.Mapping;


namespace CarStockMAP
{
    public class UserMapService
    {
        private readonly IUserService _userService;

        public UserMapService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GoogleUserDTO> MapGoogleUser(GoogleLoginRequestDTO googleLoginRequest)
        {
            var mapper = new UserMapper();
            var user = mapper.MapGoogleLoginRequestToUser(googleLoginRequest);
            
            var (normalUser, accessToken) = await _userService.HandleGoogleUser(user);
            var googleUser = mapper.MapGoogleToUser(normalUser);
            return googleUser;
        }

        public async Task<LoginTokenDto> MapGoogleUserLogin(GoogleLoginRequestDTO googleLoginRequest)
        {
            var mapper = new UserMapper();
            var user = mapper.MapGoogleLoginRequestToUser(googleLoginRequest);

            var (authenticatedUser, accessToken) = await _userService.GoogleAuthenticate(user);
            var googleLoginResult = mapper.MapUserToLoginTokenDto(authenticatedUser);
            googleLoginResult.Token = accessToken;
            return googleLoginResult;
        }

        public async Task<LoginTokenDto> MapUserLogin(LoginRequestDTO loginRequest)
        {
            var mapper = new UserMapper();
            var user = mapper.MapLoginRequestToUser(loginRequest);

            // метод возвращает пользователя и access token
            var (authenticatedUser, accessToken) = await _userService.Authenticate(user);
            // пользователя и accessToken в метод маппинга
            var loginResult = mapper.MapUserToLoginTokenDto(authenticatedUser);
            loginResult.Token = accessToken;
            return loginResult;
        }

       

        public async Task<CreateUserDTO> CreateMappedUserAsync(CreateUserDTO createUserDTO)
        {
            var mapper = new UserMapper();
            
            var user = mapper.MapCreateUserDtoToUser(createUserDTO);

            var result = await _userService.CreateUserAsync(user);

            return mapper.MapUserToCreateUserDto(result);

        }

        public async Task<GetUsersDTO> GetMappedUserAsync(string email)
        {
            var mapper = new UserMapper();

            // берем пользователя и его роли
            var userWithRoles = await _userService.GetUserAsync(email);

            if (userWithRoles == null)
            {
                throw new InvalidOperationException($"User with email '{email}' was not found.");
            }

            // извлекаем пользователя и роли
            var user = userWithRoles.Value.user;
            var roles = userWithRoles.Value.roles;

            var userDto = mapper.MapUserToGetUsersDto(user);

            userDto.Roles = roles;

            return userDto;
        }


        public async Task<IEnumerable<GetUsersDTO>> GetMappedUsersAsync()
        {
            var mapper = new UserMapper();

            var userDtos = new List<GetUsersDTO>();

            // получаем пользователей с ролями
            var usersWithRoles = await _userService.GetAllUsersAsync();

            foreach (var (user, roles) in usersWithRoles)
            {
                // маппим пользователя на DTO
                var userDto = mapper.MapUserToGetUsersDto(user);

                // добавляем роли внутри списка DTO
                userDto.Roles = roles;

                userDtos.Add(userDto);
            }

            return userDtos;
        }

        public async Task<UpdateUserDTO> UpdateMappedUserAsync(UpdateUserDTO updateUserDTO)
        {
            var mapper = new UserMapper();

            var user = mapper.MapUpdateUserDtoToUser(updateUserDTO);

            var result = await _userService.UpdateUserAsync(user);

            return updateUserDTO;
        }

       
    }
}
