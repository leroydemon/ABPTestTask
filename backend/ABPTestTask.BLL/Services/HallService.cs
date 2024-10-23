using ABPTestTask.BBL.Requests;
using ABPTestTask.BBL.Specifications;
using ABPTestTask.Common.Filters;
using ABPTestTask.Common.Hall;
using ABPTestTask.Common.Interfaces;
using AutoMapper;
using Domain.Filters;
using Domain.Specifications;
using Microsoft.Extensions.Logging;

namespace BussinesLogic.Services
{
    public class HallService : IHallService
    {
        private readonly IRepository<HallEntity> _hallRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<HallService> _logger;
        public HallService(IRepository<HallEntity> hallRepository, IMapper mapper, ILogger<HallService> logger)
        {
            _hallRepository = hallRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<Hall>> SearchAsync(IHallFilter filter)
        {
            try
            {
                var spec = new HallSpecification(filter);
                var entities = await _hallRepository.ListAsync(spec);
                return _mapper.Map<List<Hall>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for halls.");
                throw new Exception("An error occurred while processing your request.");
            }
        }

        public async Task RemoveAsync(Guid id)
        {
            try
            {
                var entity = await _hallRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException("Hall not found.");
                }

                await _hallRepository.DeleteAsync(entity);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Attempted to remove a hall that was not found: {Id}", id);
                throw; // rethrow to handle in the controller
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing the hall.");
                throw new Exception("An error occurred while processing your request.");
            }
        }

        public async Task<Hall> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _hallRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException("Hall not found.");
                }

                return _mapper.Map<Hall>(entity);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Hall not found for ID: {Id}", id);
                throw; // rethrow to handle in the controller
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the hall.");
                throw new Exception("An error occurred while processing your request.");
            }
        }

        public async Task<Hall> UpdateAsync(Hall entity)
        {
            try
            {
                var existingEntity = await _hallRepository.GetByIdAsync(entity.Id);
                if (existingEntity == null)
                {
                    throw new KeyNotFoundException("Hall not found.");
                }

                var updatedEntity = await _hallRepository.UpdateAsync(_mapper.Map<HallEntity>(entity));
                return _mapper.Map<Hall>(updatedEntity);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Attempted to update a hall that was not found: {Id}", entity.Id);
                throw; // rethrow to handle in the controller
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the hall.");
                throw new Exception("An error occurred while processing your request.");
            }
        }

        public async Task<Hall> AddAsync(Hall entityDto)
        {
            try
            {
                var entity = _mapper.Map<HallEntity>(entityDto);
                var addedEntity = await _hallRepository.AddAsync(entity);
                return _mapper.Map<Hall>(addedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the hall.");
                throw new Exception("An error occurred while processing your request.");
            }
        }

        public async Task<IEnumerable<Hall>> SearchAvailableHallsAsync(IHallAvailabilityRequest request)
        {
            try
            {
                var spec = new AvailableHallsSpecification(request);
                var halls = await _hallRepository.ListAsync(spec);
                return _mapper.Map<IEnumerable<Hall>>(halls);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for available halls.");
                throw new Exception("An error occurred while processing your request.");
            }
        }
    }
}
