using ABPTestTask.Common.Filters;
using ABPTestTask.Common.Interfaces;
using ABPTestTask.Common.User;
using AutoMapper;
using Domain.Filters;
using Domain.Specifications;

namespace BussinesLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<UserEntity> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<User>> SearchAsync(IUserFilter filter)
        {
            var spec = new UserSpecification(filter);
            var users = await _userRepository.ListAsync(spec);

            return _mapper.Map<IEnumerable<User>>(users);
        }

        public async Task RemoveAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID must be provided.", nameof(userId));
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found."); 
            }

            await _userRepository.DeleteAsync(user);
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID must be provided.", nameof(userId));
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found."); 
            }

            return _mapper.Map<User>(user);
        }

        public async Task<User> UpdateAsync(User userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto)); 
            }

            var user = _mapper.Map<UserEntity>(userDto);
            var updatedUser = await _userRepository.UpdateAsync(user);

            return _mapper.Map<User>(updatedUser);
        }

        public async Task<User> AddAsync(User userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto)); 
            }

            var user = _mapper.Map<UserEntity>(userDto);
            var addedUser = await _userRepository.AddAsync(user);

            return _mapper.Map<User>(addedUser);
        }
    }
}
