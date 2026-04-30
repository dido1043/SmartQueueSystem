using System;
using BusinessEntity = Business.Domain.Data.Model.Business;
namespace Business.Application.Repository;

public interface IBusinessRepository
{
    Task<BusinessEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(BusinessEntity business, CancellationToken ct = default);
    void Update(BusinessEntity business);
    void Delete(BusinessEntity business);
    Task<int> SaveChangesAsync(CancellationToken ct = default);

}
