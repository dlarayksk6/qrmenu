using Microsoft.AspNetCore.Identity;

public class IdentityErrorDescriberTurkish : IdentityErrorDescriber
{
    public override IdentityError DefaultError()
        => new() { Code = nameof(DefaultError), Description = "Bilinmeyen bir hata oluştu." };

    public override IdentityError ConcurrencyFailure()
        => new() { Code = nameof(ConcurrencyFailure), Description = "Eşzamanlılık hatası oluştu. Lütfen tekrar deneyin." };

    public override IdentityError PasswordMismatch()
        => new() { Code = nameof(PasswordMismatch), Description = "Şifre uyuşmuyor." };

    public override IdentityError InvalidToken()
        => new() { Code = nameof(InvalidToken), Description = "Geçersiz token." };

    public override IdentityError LoginAlreadyAssociated()
        => new() { Code = nameof(LoginAlreadyAssociated), Description = "Bu giriş bilgisi zaten başka bir kullanıcıyla ilişkili." };

    public override IdentityError InvalidUserName(string userName)
        => new() { Code = nameof(InvalidUserName), Description = $"'{userName}' geçersiz bir kullanıcı adı." };

    public override IdentityError InvalidEmail(string email)
        => new() { Code = nameof(InvalidEmail), Description = $"'{email}' geçersiz bir e-posta adresi." };

    public override IdentityError DuplicateUserName(string userName)
        => new() { Code = nameof(DuplicateUserName), Description = $"'{userName}' kullanıcı adı zaten alınmış." };

    public override IdentityError DuplicateEmail(string email)
        => new() { Code = nameof(DuplicateEmail), Description = $"'{email}' e-posta adresi zaten kullanımda." };

    public override IdentityError InvalidRoleName(string role)
        => new() { Code = nameof(InvalidRoleName), Description = $"'{role}' geçersiz bir rol adı." };

    public override IdentityError DuplicateRoleName(string role)
        => new() { Code = nameof(DuplicateRoleName), Description = $"'{role}' rol adı zaten kullanımda." };

    public override IdentityError UserAlreadyHasPassword()
        => new() { Code = nameof(UserAlreadyHasPassword), Description = "Kullanıcının zaten bir şifresi var." };

    public override IdentityError UserLockoutNotEnabled()
        => new() { Code = nameof(UserLockoutNotEnabled), Description = "Bu kullanıcı için kilitleme etkin değil." };

    public override IdentityError UserAlreadyInRole(string role)
        => new() { Code = nameof(UserAlreadyInRole), Description = $"Kullanıcı zaten '{role}' rolünde." };

    public override IdentityError UserNotInRole(string role)
        => new() { Code = nameof(UserNotInRole), Description = $"Kullanıcı '{role}' rolünde değil." };

    public override IdentityError PasswordTooShort(int length)
        => new() { Code = nameof(PasswordTooShort), Description = $"Parola en az {length} karakter olmalıdır." };

    public override IdentityError PasswordRequiresNonAlphanumeric()
        => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Parola en az bir özel karakter içermelidir." };

    public override IdentityError PasswordRequiresDigit()
        => new() { Code = nameof(PasswordRequiresDigit), Description = "Parola en az bir rakam içermelidir." };

    public override IdentityError PasswordRequiresLower()
        => new() { Code = nameof(PasswordRequiresLower), Description = "Parola en az bir küçük harf içermelidir." };

    public override IdentityError PasswordRequiresUpper()
        => new() { Code = nameof(PasswordRequiresUpper), Description = "Parola en az bir büyük harf içermelidir." };
}
