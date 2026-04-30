using System;
using System.Linq;
using Business.Application.Repository;
using BusinessEntity = Business.Domain.Data.Model.Business;
using Business.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace Business.Infrastructure.Repository;

public class BusinessRepository : IBusinessRepository
{
        private readonly BusinessDbContext _context;
    
        public BusinessRepository(BusinessDbContext context)
        {
            _context = context;
        }
    
        public Task<BusinessEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return _context.Set<BusinessEntity>().FirstOrDefaultAsync(b => b.Id == id, ct);
        }
    
        public Task AddAsync(BusinessEntity business, CancellationToken ct = default)
        {
            return _context.Set<BusinessEntity>().AddAsync(business, ct).AsTask();
        }
    
        public void Update(BusinessEntity business)
        {
            _context.Set<BusinessEntity>().Update(business);
        }
    
        public void Delete(BusinessEntity business)
        {
            _context.Set<BusinessEntity>().Remove(business);
        }
    
        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return _context.SaveChangesAsync(ct);
        }
}
