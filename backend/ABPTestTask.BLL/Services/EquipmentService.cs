using ABPTestTask.Common.Equipment;
using ABPTestTask.Common.Equipments;
using ABPTestTask.Common.Interfaces;
using AutoMapper;
using Domain.Filters;
using Domain.Specifications;

namespace BussinesLogic.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IRepository<EquipmentEntity> _equipmentRepository;
        private readonly IMapper _mapper;
        public EquipmentService(IRepository<EquipmentEntity> equipmentRepository, IMapper mapper)
        {
            _equipmentRepository = equipmentRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Equipment>> SearchAsync(EquipmentFilter filter)
        {
            // Create a specification for searching equipment based on the filter
            var spec = new EquipmentSpecification(filter);

            // Retrieve the list of equipment that matches the specification
            var entities = await _equipmentRepository.ListAsync(spec);

            // Map the entities to DTOs and return them
            return _mapper.Map<List<Equipment>>(entities);
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

        public async Task<Equipment> GetByIdAsync(Guid id)
        {
            // Fetch the equipment entity by ID
            var entity = await _equipmentRepository.GetByIdAsync(id);

            // If the entity is not found, handle the scenario (optional)
            if (entity == null)
            {
                throw new InvalidOperationException("Equipment not found.");
            }

            // Map the entity to DTO and return it
            return _mapper.Map<Equipment>(entity);
        }

        public async Task<Equipment> UpdateAsync(Equipment entityDto)
        {
            var entity = _mapper.Map<EquipmentEntity>(entityDto);

            // Map the DTO to the Equipment entity
            var updatedEntity = await _equipmentRepository.UpdateAsync(entity);

            // Map the updated entity back to DTO and return it
            return _mapper.Map<Equipment>(updatedEntity);
        }

        public async Task<Equipment> AddAsync(Equipment entityDto)
        {
            // Map the DTO to the Equipment entity
            var entity = _mapper.Map<EquipmentEntity>(entityDto);

            // Add the equipment entity to the repository
            var addedEntity = await _equipmentRepository.AddAsync(entity);

            // Map the added entity back to DTO and return it
            return _mapper.Map<Equipment>(addedEntity);
        }
    }
}
