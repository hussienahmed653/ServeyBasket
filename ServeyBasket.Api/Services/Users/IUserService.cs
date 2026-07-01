namespace ServeyBasket.Services.Users;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetUserProfileAsync(string userId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    Task<IEnumerable<UserResponse>> GetAllAsync();
    Task<Result<UserResponse>> GetAsync(string id);
    Task<Result<UserResponse>> AddAsync(CreateUserRequest request);
    Task<Result> UpdateAsync(string id, UpdateUserRequest request);
    Task<Result> ToggleStatusAsync(string id);
    Task<Result> Unlock(string id);
}
