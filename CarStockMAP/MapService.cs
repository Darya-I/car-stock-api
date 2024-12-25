using CarStockBLL.Interfaces;
using CarStockBLL.Models;
using CarStockMAP.DTO;
using CarStockMAP.Mapping;
using CarStockMAP.Models;


namespace CarStockMAP
{
    public class MapService
    {
        private readonly ICarService _carService;
        private readonly IUserService _userService;

        public MapService(ICarService carService, IUserService userService)
        {
            _carService = carService;
            _userService = userService;
        }


        public async Task<LoginTokenDto> MapUserLogin(LoginRequest loginRequest)
        {
            var mapper = new UserMapper();
            var user = mapper.MapLoginRequestToUser(loginRequest);

            // Аутентификация возвращает пользователя и access token
            var (authenticatedUser, accessToken) = await _userService.Authenticate(user);
            // Передаем пользователя и accessToken в метод маппинга
            var loginResult = mapper.MapUserToLoginTokenDto(authenticatedUser);
            loginResult.Token = accessToken;
            return loginResult;
        }

        public async Task<IEnumerable<CarDTO>> GetMappedCarsAsync()
        {
            var cars = await _carService.GetAllCarsAsync();

            var mapper = new CarMapper();
            //преобразуем каждый объект Car из коллекции cars в объекты CarDto и создаем список
            var carDtos = cars.Select(car => mapper.MapToCarDto(car)).ToList();  

            return carDtos;

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

            // Получаем пользователя и его роли
            var userWithRoles = await _userService.GetUserAsync(email);

            if (userWithRoles == null)
            {
                throw new InvalidOperationException($"User with email '{email}' was not found.");
            }

            // Извлекаем пользователя и роли
            var user = userWithRoles.Value.user;
            var roles = userWithRoles.Value.roles;

            // Маппим пользователя на DTO
            var userDto = mapper.MapUserToGetUsersDto(user);

            // Добавляем роли в DTO
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

        public async Task<OperationResult<string>> CreateMappedCarAsync(CarDTO carDto)
        {
            try
            {
                var mapper = new CarMapper();

                // преобразуем DTO в модель уровня BLL
                var car = mapper.MapToCar(carDto);

                // передаем преобразованную модель в _carService для создания
                var result = await _carService.CreateCarAsync(car);

                return result;
            }
            catch (Exception ex)
            {
                return OperationResult<string>.Failure($"An error occurred while creating the mapped car: {ex.Message}");
            }
        }


    }
}
