using Microsoft.AspNetCore.Identity;

namespace CarStockBLL.CustomExceptions.IdentityErrorsHandle
{
    /// <summary>
    /// Вспомогательный класс для работы с результатами identity
    /// </summary>
    public class IdentityValidationHandle
    {
        /// <summary>
        /// Получает список ошибок, сгенерированных identity
        /// </summary>
        /// <param name="result">Результат от при валидации пароля <see cref="IdentityResult"/> </param>
        /// <returns>Список кодов ошибок при валидации пароля</returns>
        public List<IdentityError> ExtractPasswordValidationErrors(IdentityResult result)
        {
            var passwordErrorCodes = new HashSet<string>
            {
                "PasswordTooShort",
                "PasswordRequiresNonAlphanumeric",
                "PasswordRequiresDigit",
                "PasswordRequiresUpper"
            };

            return result.Errors
                .Where(e => passwordErrorCodes.Contains(e.Code)) // Остаются только те ошибки, чей код содержится в passwordErrorCodes
                .ToList();
        }
    }
}
