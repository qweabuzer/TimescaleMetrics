using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TimescaleMetrics.Core.Interfaces;
using TimescaleMetrics.Core.Models;
using TimescaleMetrics.DataAccess.Entities;

namespace TimescaleMetrics.DataAccess.Repositories
{
    public class ValueRepository : IValueRepository
    {
        private readonly TimescaleDbContext _dbContext;
        private readonly IMapper _mapper;

        public ValueRepository(TimescaleDbContext dbContext, IMapper mapper)
        {
             _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddValuesAsync(IEnumerable<CSValue> values, string fileName)
        {
            var entities = values.Select(value =>
            {
                var entity = _mapper.Map<ValueEntity>(value);
                entity.FileName = fileName;

                return entity;
            }).ToList();

            await _dbContext.Values.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteValuesAsync(string fileName)
        {
            await _dbContext.Values
                .Where(v => v.FileName == fileName)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> FileExistsAsync(string fileName)
        {
            return await _dbContext.Values
                .AnyAsync(v => v.FileName == fileName);
        }

        public async Task<IEnumerable<CSValue>> GetLastValuesAsync(string fileName, int count = 10)
        {
            var entities = await _dbContext.Values
                .Where(v => v.FileName == fileName)
                .OrderByDescending(v => v.Date)
                .Take(count)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CSValue>>(entities);
        }
    }
}
