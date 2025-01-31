using System.Data;
using CarStockBLL.CustomException;
using CarStockBLL.DTO.User;
using CarStockBLL.Interfaces;
using CarStockBLL.Map;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarStockBLL.Services
{
    /// <summary>
    /// Сервис операций над пользователями
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Экземпляр менеджера для работы с пользователями
        /// </summary>
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Экземпляр репозитория для работы с пользователями
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Экземпляр логгера
        /// </summary>
        private readonly ILogger<IUserService> _logger;

        /// <summary>
        /// Экземпляр маппера
        /// </summary>
        private readonly UserMapper _mapper;

        /// <summary>
        /// Инициализирует новый экземпляр сервиса операций над пользователями
        /// </summary>
        /// <param name="userManager">Менеджер управления пользователями</param>
        /// <param name="userRepository">Репозиторий доступа к пользователям</param>
        /// <param name="logger">Логгер</param>
        /// <param name="mapper">Маппер для модели пользователя</param>
        public UserService(
            UserManager<User> userManager,
            IUserRepository userRepository,
            ILogger<IUserService> logger,
            UserMapper mapper) 
        {
            _userManager = userManager;   
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Получает пользователя с списком ролей из базы данных
        /// </summary>
        /// <param name="email">Почта</param>
        /// <returns>DTO представления созданного пользователя</returns>
        public async Task<GetUserDTO> GetUserAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByUsernameAsync(email);
                if (user == null)
                {
                    throw new EntityNotFoundException($"User with email '{email}' was not found.");
                }

                return _mapper.UserToGetUserDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving user. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Создает нового пользователя в базе данных
        /// </summary>
        /// <param name="userDto">Пользователь</param>
        /// <returns>DTO представления созданного пользователя</returns>
        public async Task<GetUserDTO> CreateUserAsync(CreateUserDTO userDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(userDto.Email);

                if (existingUser != null)
                {
                    throw new EntityAlreadyExistsException("A user with this email already exists.");
                }

                var role = await _userRepository.GetUserRoleAsync(userDto.RoleId);

                if (role== null)
                {
                    _logger.LogWarning($"Role with Id {userDto.RoleId} not found");
                    throw new EntityNotFoundException($"Role with Id {userDto.RoleId} not found");
                }

                // С DTO на объект пользователя
                var user = _mapper.UserDtoToUser(userDto);

                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                
                if (!result.Succeeded)
                {
                    _logger.LogError(string.Join($"Failed to create the user. Errors:", result.Errors.Select(e => e.Description)));
                    throw new ApiException("Failed to create the user. Errors:");
                }

                // С объекта нового пользователя на DTO
                var newUser = _mapper.UserToGetUserDto(user);

                return newUser;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating user. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Создает нового пользователя и назначает ему роль User
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>DTO представления созданного пользователя</returns>
        public async Task<GetUserDTO> RegisterUser(User user) 
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser != null)
                {
                    throw new EntityAlreadyExistsException("A user with this email already exists.");
                }

                // Присваиваем роль "User" (объектом, чтобы потом сделать обратный маппинг)
                user.Role = await _userRepository.GetUserRoleAsync(2);

                var result = await _userManager.CreateAsync(user, user.PasswordHash);

                if (!result.Succeeded)
                {
                    _logger.LogError(string.Join($"Failed to register the user. Errors:", result.Errors.Select(e => e.Description)));
                    throw new ApiException("Failed to register the user. Errors:");
                }

                // С объекта нового пользователя на DTO
                var newUser = _mapper.UserToGetUserDto(user);

                return newUser;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while register user. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Удаляет пользователя из базы данных
        /// </summary>
        /// <param name="email">Почта</param>
        public async Task DeleteUserAsync(string email)
        {
            try 
            {
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    throw new EntityNotFoundException("A user with this email does not exists.");
                }

                var result = await _userManager.DeleteAsync(existingUser);
                if (!result.Succeeded)
                {
                    _logger.LogError(string.Join($"Failed to delete the user. Errors:", result.Errors.Select(e => e.Description)));
                    throw new ApiException("Failed to delete the user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting user. Details: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Обновляет данные пользователя
        /// </summary>
        /// <param name="userDto">Пользователь</param>
        /// <returns>DTO представления обновленного пользователя</returns>
        public async Task<GetUserDTO> UpdateUserAsync(UpdateUserDTO userDto)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(userDto.Email);
                
                if (existingUser == null)
                {
                    throw new EntityNotFoundException("A user with this email does not exist.");
                }

                var user = _mapper.UpadateDtoToUser(userDto);

                // Проверка и обновление роли
                if (user.RoleId > 0 && existingUser.RoleId != user.RoleId)
                {
                    var role = await _userRepository.GetUserRoleAsync(user.RoleId);
                    if (role == null)
                    {
                        _logger.LogWarning($"Role with Id {user.RoleId} not found");
                        throw new EntityNotFoundException($"Role with Id {user.RoleId} not found");
                    }
                    existingUser.RoleId = user.RoleId;
                }

                // Проверка и обновление имени пользователя
                if (!string.IsNullOrEmpty(user.UserName) && user.UserName != existingUser.UserName)
                {
                    existingUser.UserName = user.UserName;
                }

                // Проверка и обновление email
                if (!string.IsNullOrEmpty(user.Email) && user.Email != existingUser.Email)
                {
                    existingUser.Email = user.Email;
                }

                // Обновление пароля (если передан новый пароль)
                if (!string.IsNullOrEmpty(userDto.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    
                    var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, userDto.Password);

                    if (!passwordResult.Succeeded)
                    {
                        _logger.LogError($"Failed to update user's password: {string.Join(", ", passwordResult.Errors.Select(e => e.Description))}");
                        throw new ApiException("Failed to update user's password.");
                    }
                }

                // Обновление пользователя
                var result = await _userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                {
                    _logger.LogError($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    throw new ApiException("Failed to update user.");
                }

                return _mapper.UserToGetUserDto(existingUser);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating user. Details: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Получает список пользователей из базы данных
        /// </summary>
        /// <returns>Список DTO представления пользователей</returns>
        public async Task<List<GetUserDTO>> GetAllUsersAsync()
        {
            
            try
            {
                var users = await _userManager.Users
                    .Include(u => u.Role) // Связные роли
                    .ToListAsync();
                
                // К каждому элементу из списка применяется маппер
                var result = users.Select(_mapper.UserToGetUserDto).ToList();

                if (result == null)
                {
                    _logger.LogError("Failed to retrieve users");
                    throw new ApiException("Failed to retrieve users");
                }

                return result;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error occurred while retrieving all users. Details: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Обновляет только некоторые данные пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>DTO представления обновленного пользователя</returns>
        public async Task<GetUserDTO> UpdateUserAccount(User user)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    throw new EntityNotFoundException("A user with this email does not exist.");
                }

                // Проверка и обновление имени пользователя
                if (!string.IsNullOrEmpty(user.UserName) && user.UserName != existingUser.UserName)
                {
                    existingUser.UserName = user.UserName;
                }

                // Обновление пароля (если передан новый пароль)
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);

                    var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, user.PasswordHash);
                    
                    if (!passwordResult.Succeeded)
                    {
                        _logger.LogError($"Failed to update user's password: {string.Join(", ", passwordResult.Errors.Select(e => e.Description))}");
                        throw new ApiException("Failed to update user's password.");
                    }
                }

                // Обновление пользователя
                var result = await _userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                {
                    _logger.LogError($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    throw new ApiException("Failed to update user.");
                }

                return _mapper.UserToGetUserDto(existingUser);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while updating user. Details: {ex.Message}");
                throw;
            }
        }
    }
}