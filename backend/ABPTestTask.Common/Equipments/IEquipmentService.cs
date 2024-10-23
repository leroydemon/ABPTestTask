
using Domain.Filters;
using Domain.SortableFields;

namespace ABPTestTask.Common.Equipments
{
    public interface IEquipmentService
    {
        Task<IEnumerable<Equipment>> SearchAsync(IFilterBase<EquipmentSortableFields> filter);
        Task RemoveAsync(Guid Id);
        Task<Equipment> GetByIdAsync(Guid Id);
        Task<Equipment> UpdateAsync(Equipment EquipmentDto);
        Task<Equipment> AddAsync(Equipment EquipmentDto);
    }
}
