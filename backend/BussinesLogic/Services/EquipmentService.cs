using AutoMapper;
using BussinesLogic.EntitiesDto;
using BussinesLogic.Interfaces;
using DbLevel.Interfaces;
using Domain.Entities;
using Domain.Filters;
using Domain.Specifications;

namespace BussinesLogic.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IRepository<Equipment> _equipmentRepository;
        private readonly IMapper _mapper;
        public EquipmentService(IRepository<Equipment> equipmentRepository, IMapper mapper)
        {
            _equipmentRepository = equipmentRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<EquipmentDto>> SearchAsync(EquipmentFilter filter)
        {
            // Create a specification for searching equipment based on the filter
            var spec = new EquipmentSpecification(filter);

            // Retrieve the list of equipment that matches the specification
            var entities = await _equipmentRepository.ListAsync(spec);

            // Map the entities to DTOs and return them
            return _mapper.Map<List<EquipmentDto>>(entities);
        }

        public async Task RemoveAsync(Guid id)
        {
            // Fetch the equipment entity by ID
            var entity = await _equipmentRepository.GetByIdAsync(id);

            // If the entity is not found, handle the scenario (optional)
            if (entity == null)
            {
                throw new InvalidOperationException("Equipment not found.");
            }

            // Delete the equipment entity
            await _equipmentRepository.DeleteAsync(entity);
        }

        public async Task<EquipmentDto> GetByIdAsync(Guid id)
        {
            // Fetch the equipment entity by ID
            var entity = await _equipmentRepository.GetByIdAsync(id);

            // If the entity is not found, handle the scenario (optional)
            if (entity == null)
            {
                throw new InvalidOperationException("Equipment not found.");
            }

            // Map the entity to DTO and return it
            return _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<EquipmentDto> UpdateAsync(EquipmentDto entityDto)
        {
            // Map the DTO to the Equipment entity
            var updatedEntity = await _equipmentRepository.UpdateAsync(_mapper.Map<Equipment>(entityDto));

            // Map the updated entity back to DTO and return it
            return _mapper.Map<EquipmentDto>(updatedEntity);
        }

        public async Task<EquipmentDto> AddAsync(EquipmentDto entityDto)
        {
            // Map the DTO to the Equipment entity
            var entity = _mapper.Map<Equipment>(entityDto);

            // Add the equipment entity to the repository
            var addedEntity = await _equipmentRepository.AddAsync(entity);

            // Map the added entity back to DTO and return it
            return _mapper.Map<EquipmentDto>(addedEntity);
        }
    }
}
