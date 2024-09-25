using AutoMapper;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using DbLevel.Interfaces;
using Domain.Entities;
using Domain.Filters;
using Domain.Specifications;

namespace BussinesLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserDto>> SearchAsync(UserFilter filter)
        {
            var spec = new UserSpecification(filter);
            var users = await _userRepository.ListAsync(spec);

            return _mapper.Map<List<UserDto>>(users);
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

        public async Task<UserDto> GetByIdAsync(Guid userId)
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

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto)); 
            }

            var user = _mapper.Map<User>(userDto);
            var updatedUser = await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task<UserDto> AddAsync(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto)); 
            }

            var user = _mapper.Map<User>(userDto);
            var addedUser = await _userRepository.AddAsync(user);

            return _mapper.Map<UserDto>(addedUser);
        }
    }
}
