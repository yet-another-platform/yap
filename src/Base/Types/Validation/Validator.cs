using Types.Interfaces.Model;

namespace Types.Validation;

public class Validator
{
    public ValidationResult Validate(object obj)
    {
        var result = new ValidationResult();
        if (obj is IUsername username && !IUsername.Validate(username))
        {
            result.Errors.Add(new ValidationError
            {
                PropertyName = nameof(username.Username),
                Message = "Invalid username format"
            });
        }

        if (obj is IPassword password && !IPassword.Validate(password))
        {
            result.Errors.Add(new ValidationError
            {
                PropertyName = nameof(password.Password),
                Message = "Invalid password format"
            });
        }
        
        if (obj is IEmail email && !IEmail.Validate(email))
        {
            result.Errors.Add(new ValidationError
            {
                PropertyName = nameof(email.Email),
                Message = "Invalid email format"
            });
        }

        return result;
    }
}