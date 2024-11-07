using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Sensors.Data;
using Sensors.Extensions;
using Sensors.Helpers;

namespace Sensors.IdentityManager;

public class CustomUserManager : UserManager<IdentityUser>
{
    private readonly ApplicationDbContext _context;

    public CustomUserManager(
        IUserStore<IdentityUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<IdentityUser> passwordHasher,
        IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<IdentityUser>> logger,
        ApplicationDbContext context)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
        _context = context;
    }

    public override async Task<IdentityResult> CreateAsync(IdentityUser user, string password)
    {
        var result = await base.CreateAsync(user, password);
        if (result.Succeeded)
        {
            // Automatically create ClientSecret after user creation
            var clientSecret = new ClientSecret
            {
                UserId = user.Id,
                ClientSecretKey = RandomStringGenerator.Generate() // Generate a new GUID
            };

            await _context.ClientSecrets.AddAsync(clientSecret);
            await _context.SaveChangesAsync();
        }

        return result;
    }
}
