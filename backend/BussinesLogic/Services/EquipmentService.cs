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
            var spec = new EquipmentSpecification(filter);
            var entities = await _equipmentRepository.ListAsync(spec);

            return _mapper.Map<List<EquipmentDto>>(entities);
        }

        public async Task RemoveAsync(Guid Id)
        {
            var entity = await _equipmentRepository.GetByIdAsync(Id);
            await _equipmentRepository.DeleteAsync(entity);
        }

        public async Task<EquipmentDto> GetByIdAsync(Guid Id)
        {
            var entity = await _equipmentRepository.GetByIdAsync(Id);
            return _mapper.Map<EquipmentDto>(entity);
        }

        public async Task<EquipmentDto> UpdateAsync(EquipmentDto entity)
        {
            var updatedEntity = await _equipmentRepository.UpdateAsync(_mapper.Map<Equipment>(entity));

            return _mapper.Map<EquipmentDto>(updatedEntity);
        }

        public async Task<EquipmentDto> AddAsync(EquipmentDto entityDto)
        {
            var entity = _mapper.Map<Equipment>(entityDto);
            var addedEntity = await _equipmentRepository.AddAsync(entity);

            return _mapper.Map<EquipmentDto>(addedEntity);
        }
    }
}
