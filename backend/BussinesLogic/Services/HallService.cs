using AutoMapper;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using DbLevel.Interfaces;
using Domain.Entities;
using Domain.Filters;
using Domain.Specifications;

namespace BussinesLogic.Services
{
    public class HallService : IHallService
    {
        private readonly IRepository<Hall> _hallRepository;
        private readonly IMapper _mapper;
        public HallService(IRepository<Hall> hallRepository, IMapper mapper)
        {
            _hallRepository = hallRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<HallDto>> SearchAsync(HallFilter filter)
        {
            var spec = new HallSpecification(filter);
            var entities = await _hallRepository.ListAsync(spec);

            return _mapper.Map<List<HallDto>>(entities);
        }

        public async Task RemoveAsync(Guid Id)
        {
            var entity = await _hallRepository.GetByIdAsync(Id);
            await _hallRepository.DeleteAsync(entity);
        }

        public async Task<HallDto> GetByIdAsync(Guid Id)
        {
            var entity = await _hallRepository.GetByIdAsync(Id);
            return _mapper.Map<HallDto>(entity);
        }

        public async Task<HallDto> UpdateAsync(HallDto entity)
        {
            var updatedEntity = await _hallRepository.UpdateAsync(_mapper.Map<Hall>(entity));

            return _mapper.Map<HallDto>(updatedEntity);
        }

        public async Task<HallDto> AddAsync(HallDto entityDto)
        {
            var entity = _mapper.Map<Hall>(entityDto);
            var addedEntity = await _hallRepository.AddAsync(entity);

            return _mapper.Map<HallDto>(addedEntity);
        }

        public async Task<IEnumerable<HallDto>> SearchAvailableHallsAsync(HallAvailabilityRequest request)
        {
            var spec = new AvailableHallsSpecification(request);
            var halls = await _hallRepository.ListAsync(spec);

            return _mapper.Map<List<HallDto>>(halls);
        }
    }
}
