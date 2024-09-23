using BussinesLogic.EntitiesDto;
using Domain.Filters;

namespace BussinesLogic.Interfaces
{
    public interface IEquipmentService
    {
        Task<IEnumerable<EquipmentDto>> SearchAsync(EquipmentFilter filter);
        Task RemoveAsync(Guid Id);
        Task<EquipmentDto> GetByIdAsync(Guid Id);
        Task<EquipmentDto> UpdateAsync(EquipmentDto EquipmentDto);
        Task<EquipmentDto> AddAsync(EquipmentDto EquipmentDto);
    }
}
