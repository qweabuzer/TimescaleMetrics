using AutoMapper;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TimescaleMetrics.Core.Interfaces;
using TimescaleMetrics.Core.Models;
using TimescaleMetrics.DataAccess.Entities;

namespace TimescaleMetrics.DataAccess.Repositories
{
    public class ResultRepository : IResultRepository
    {

        private readonly TimescaleDbContext _dbContext;
        private readonly IMapper _mapper;

        public ResultRepository(TimescaleDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<IntegrationResults> AddOrUpdateAsync(string fileName, IntegrationResults result)
        {
            var checkRes = await _dbContext.Results
                .FirstOrDefaultAsync(r => r.FileName == fileName);

            var resultEntity = _mapper.Map<ResultEntity>(result);
            resultEntity.FileName = fileName;

            if (checkRes != null)
            {
                resultEntity.Id = checkRes.Id;

                _dbContext.Entry(checkRes).CurrentValues.SetValues(resultEntity); ;
            }
            else
            {
                resultEntity.Id = Guid.NewGuid();
                await _dbContext.Results.AddAsync(resultEntity);
            }

            await _dbContext.SaveChangesAsync();
            return result;
        }

        public async Task DeleteResultsAsync(string fileName)
        {
            await _dbContext.Results
                .Where(r => r.FileName == fileName)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> ExistsFileAsync(string fileName)
        {
            return await _dbContext.Results
                .AnyAsync(x => x.FileName == fileName);
        }

        public async Task<List<IntegrationResults>> GetFilteredResultsAsync(ResultsFilter filter)
        {
            var query = _dbContext.Results.AsQueryable();

            if (!string.IsNullOrEmpty(filter.FileName))
            {
                query = query.Where(r => r.FileName.Contains(filter.FileName));
            }

            if (filter.MinDateFrom.HasValue)
            {
                query = query.Where(r => r.MinDate >= filter.MinDateFrom.Value);
            }

            if (filter.MinDateTo.HasValue)
            {
                query = query.Where(r => r.MinDate <= filter.MinDateTo.Value);
            }

            if (filter.AvgExecutionTimeFrom.HasValue)
            {
                query = query.Where(r => r.AvgExecutionTime >= filter.AvgExecutionTimeFrom.Value);
            }

            if (filter.AvgExecutionTimeTo.HasValue)
            {
                query = query.Where(r => r.AvgExecutionTime <= filter.AvgExecutionTimeTo.Value);
            }

            if (filter.AvgValueFrom.HasValue)
            {
                query = query.Where(r => r.AvgValue >= filter.AvgValueFrom.Value);
            }

            if (filter.AvgValueTo.HasValue)
            {
                query = query.Where(r => r.AvgValue <= filter.AvgValueTo.Value);
            }


            query = query.OrderByDescending(r => r.MinDate);

            var resQuery = query.Select(result => _mapper.Map<IntegrationResults>(result));

            return await resQuery.ToListAsync();
        }
    }
}
