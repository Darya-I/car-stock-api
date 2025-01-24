using CarStockBLL.DTO.User;
using CarStockDAL.Models;
using Riok.Mapperly.Abstractions;

namespace CarStockBLL.Map
{
    [Mapper]
    public partial class UserMapperBLL
    {
        [MapProperty(nameof(user.Email), nameof(GetUserDTOdraft.Email))]
        [MapProperty(nameof(user.UserName), nameof(GetUserDTOdraft.UserName))]
        [MapProperty(nameof(user.Role.Name), nameof(GetUserDTOdraft.RoleName))]
        public partial GetUserDTOdraft UserToGetUserDto(User user);

        [MapProperty(nameof(user.Email), nameof(User.Email))]
        [MapProperty(nameof(user.Email), nameof(User.UserName))]
        [MapProperty(nameof(user.Password), nameof(User.PasswordHash))]
        public partial User UserDtoToUser(UserDTO user);
    }
}