using AutoMapper;
using VideoStreaming.Common.Enums;
using VideoStreaming.Common.Exceptions;
using VideoStreaming.Common.Helpers;
using VideoStreaming.Common.Models.User;
using VideoStreaming.Data;
using VideoStreaming.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using VideoStreaming.Common.Models.Stream;

namespace VideoStreaming.Core;

public class AccountManager
{
    private readonly UserManager<User> userManager;
    private readonly IConfiguration configuration;
    private readonly IMapper mapper;
    private readonly JwtHelper jwtHelper;
    private readonly VideoStreamingDbContext db;
    private readonly int MINUTES_VERIFICATION_CODE_IS_VALID;

    public AccountManager(
        UserManager<User> userManager,
        IConfiguration configuration,
        IMapper mapper,
        VideoStreamingDbContext db
    )
    {
        this.userManager = userManager;
        this.configuration = configuration;
        this.mapper = mapper;
        jwtHelper = new JwtHelper(configuration);
        this.db = db;
    }

    public async Task<UserMeModel> GetMyUserInfo(Guid userId)
    {
        var userQuery = db.Users.Where(u => u.Id == userId && u.Active);
        var user = await mapper.ProjectTo<UserMeModel>(userQuery).FirstOrDefaultAsync();
        ValidationHelper.MustExist(user);

        var meetQuery = db.Meets.Where(mm => mm.UserOwnerId == userId);
        var meets = await mapper.ProjectTo<StreamModel>(meetQuery).ToListAsync();
        user.Streams = meets;

        return user;
    }

    public async Task<AuthResponseModel> Login(UserLoginModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Email);
        ValidationHelper.MustExist(user);

        bool isValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

        if (!isValidPassword)
        {
            throw new ValidationException(ErrorCode.InvalidCredentials);
        }

        var loginResult = await GenerateLoginResponse(user);
        return loginResult;
    }

    public async Task<AuthResponseModel> Register(UserRegisterModel model)
    {
        var existingUser = await userManager.FindByEmailAsync(model.Email);
        ValidationHelper.MustNotExist(existingUser);

        var passwordValidator = new PasswordValidator<User>();
        var passwordValidationResult = await passwordValidator.ValidateAsync(userManager, null, model.Password);

        if (!passwordValidationResult.Succeeded)
        {
            throw new ValidationException(ErrorCode.WrongPassword);
        }

        var newUser = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Email,
            Active = true
        };

        using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            IdentityResult identityResult;
            IFormFile avatarFileToSave = null;

            // User registers with password
            if (string.IsNullOrEmpty(model.Password))
            {
                ValidationHelper.ThrowModelValidationException("Password", "Password is required.");
            }

            identityResult = await userManager.CreateAsync(newUser, model.Password);

            if (!identityResult.Succeeded)
            {
                throw new ValidationException(ErrorCode.IdentityError);
            }

            identityResult = await userManager.AddToRoleAsync(newUser, UserRoleConstants.Streamer);

            if (!identityResult.Succeeded)
            {
                throw new ValidationException(ErrorCode.IdentityError);
            }

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new ValidationException(ErrorCode.IdentityError);
        }

        var roles = await userManager.GetRolesAsync(newUser);
        var token = jwtHelper.GenerateJwtToken(newUser.Id, newUser.Email, roles);

        return new AuthResponseModel
        {
            Token = token,
            Roles = roles.ToList()
        };
    }

    private async Task<AuthResponseModel> GenerateLoginResponse(User user)
    {
        var roles = await userManager.GetRolesAsync(user);
        if (!roles.Any())
        {
            throw new ValidationException(ErrorCode.NoRoleAssigned);
        }

        var token = jwtHelper.GenerateJwtToken(user.Id, user.Email, roles);
        return new AuthResponseModel
        {
            Token = token,
            Roles = roles.ToList()
        };
    }
}
