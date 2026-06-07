using AutoMapper;
using ECommerce.Application.Authentication.Dtos;
using ECommerce.Domain.IRepositories;
using ECommerce.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler(UserManager<ApplicationUser> _userManager,
                                        IMapper _mapper, IMerchantRepository _merchantRepository) : IRequestHandler<RegisterCommand, AppUserDto>
    {
        public async Task<AppUserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                throw new Exception("Email already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(x => x.Description)));
            }

            var merchant = new Merchant
            {
                UserId = user.Id,
                Name = request.UserName, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _merchantRepository.CreateAsync(merchant);

            return _mapper.Map<AppUserDto>(user);
        }
    }
}
