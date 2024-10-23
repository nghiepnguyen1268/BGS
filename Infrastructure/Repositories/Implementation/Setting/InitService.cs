using Application.DTOs.Request.Auth;
using Application.DTOs.Response;
using Application.Interfaces.Setting;
using Domain.Constants;
using Domain.Models.Auth;
using Domain.Models.Management;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementation.Setting
{
    public class InitService : IInitService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly BGSDbContext _context;

        public InitService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, BGSDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<GeneralResponse> CreateAsync(CreateStaffRequest model)
        {
            try
            {
                var user = await FindUserByEmailAsync(model.Email!);
                if(user != null)
                {
                    return new GeneralResponse(Success: false, Message: "User already existed");
                }

                var newUser = new User()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true, 
                    PasswordHash = model.Password
                };

                var result = await _userManager.CreateAsync(newUser, model.Password!);
                string error = CheckResponse(result);
                if(!string.IsNullOrEmpty(error))
                {
                    return new GeneralResponse(Success: false, Message: error);
                }

                var role = new IdentityRole() { Name = model.Role };
                var assignRole = await _userManager.AddToRoleAsync(newUser, role.ToString());
                string err = CheckResponse(assignRole);
                if(!string.IsNullOrEmpty(err))
                {
                    return new GeneralResponse(Success: false, Message: err);
                }
                else
                {
                    return new GeneralResponse(Success: true, Message: "Register is successfully");
                }
            }
            catch (Exception ex)
            {
                return new GeneralResponse(Success: false, Message: ex.Message.ToString());
            }
        }
        //Hàm tạo tài khoản addmin 
        public async Task<GeneralResponse> CreateInitAdmin()
        {
            try
            {
                var initAdmin = new CreateStaffRequest()
                {
                    Email = "nghiepnguyen1268@gmail.com",
                    Password = "Nghiep2003@",
                    Role = StaticUserRole.ADMIN
                };

                var createInitAdmin = await CreateAsync(initAdmin);
                if(!createInitAdmin.Success)
                {
                    return new GeneralResponse(Success: false, Message: "Loi dong 35 iinitservice");
                }
                else
                {
                    return new GeneralResponse(Success: true, Message: "Createing admin process is successful");
                }
            }
            catch(Exception ex)
            {
                return new GeneralResponse(Success: false, Message: ex.Message.ToString());
            }
        }

        public async Task<GeneralResponse> SeedRoles()
        {
            try 
            {
                bool isAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.ADMIN);
                bool isManagerRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.MANAGER);
                bool isStaffRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.STAFF);
                bool isUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRole.USER);

                if (isAdminRoleExist || isManagerRoleExist || isStaffRoleExist || isUserRoleExist)
                {
                    return new GeneralResponse(Success: false, Message: "The all roles have been created");
                }

                var adminRole = new IdentityRole() { Name = StaticUserRole.ADMIN };
                var managerRole = new IdentityRole() { Name = StaticUserRole.MANAGER };
                var staffRole = new IdentityRole() { Name = StaticUserRole.STAFF };
                var userRole = new IdentityRole() { Name = StaticUserRole.USER };

                var createAdminRole = await _roleManager.CreateAsync(adminRole);
                var createManagerRole = await _roleManager.CreateAsync(managerRole);
                var createStaffRole = await _roleManager.CreateAsync(staffRole);
                var createUserRole = await _roleManager.CreateAsync(userRole);

                if (!createAdminRole.Succeeded && !createManagerRole.Succeeded && !createStaffRole.Succeeded && !createUserRole.Succeeded)
                {
                    return new GeneralResponse(Success: false, Status: 200, Message: "Create role failed");
                }
                else
                {
                    return new GeneralResponse(Success: false, Status: 200, Message: "Đã tạo role thành công");
                }
            }
            catch(Exception ex)
            {
                return new GeneralResponse(Success: false, Message: ex.Message.ToString());
            }
        }

        public async Task<GeneralResponse> SeedOrderStatus()
        {
            try
            {
                var isConfirmedOrderExist = await _context.OrderStatus.FirstOrDefaultAsync(x => x.StatusName == StaticOrderStatus.ConfirmedOrder); 
                var isPreparingOrderExist = await _context.OrderStatus.FirstOrDefaultAsync(x => x.StatusName == StaticOrderStatus.ConfirmedOrder); 
                var isPendingExist = await _context.OrderStatus.FirstOrDefaultAsync(x => x.StatusName == StaticOrderStatus.ConfirmedOrder); 
                var isDoneExist = await _context.OrderStatus.FirstOrDefaultAsync(x => x.StatusName == StaticOrderStatus.ConfirmedOrder); 
                
                if(isConfirmedOrderExist != null || isPreparingOrderExist != null || isPendingExist != null || isDoneExist != null)
                {
                    return new GeneralResponse(Success: false,  Message: "Các Status này đã được tạo rồi");
                }

                var confirmedOrder = new OrderStatus() { StatusName = StaticOrderStatus.ConfirmedOrder };
                var preparingOrder = new OrderStatus() { StatusName = StaticOrderStatus.PreparingOrder };
                var pendingOrder = new OrderStatus() { StatusName = StaticOrderStatus.Pending };
                var doneOrder = new OrderStatus() { StatusName = StaticOrderStatus.Done };

                _context.OrderStatus.Add(confirmedOrder);
                _context.OrderStatus.Add(preparingOrder);
                _context.OrderStatus.Add(pendingOrder);
                _context.OrderStatus.Add(doneOrder);
                var result = await _context.SaveChangesAsync();

                return new GeneralResponse(Success: true, Message: "Successfully");
            }
            catch (Exception ex)
            {
                return new GeneralResponse(Success: false, Message: ex.Message);
            }
        }

        #region  
        private async Task<User> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        private static string CheckResponse(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var err = result.Errors.Select(i => i.Description);
                return string.Join(Environment.NewLine, err);
            }

            return null!;
        }
        #endregion
    }
}