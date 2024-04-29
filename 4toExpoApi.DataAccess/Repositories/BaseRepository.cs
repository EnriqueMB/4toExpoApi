using _4toExpoApi.DataAccess.IRepositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly _4toExpoDbContext _context;

        public BaseRepository(_4toExpoDbContext context)
        {
            _context = context;
        }

        protected DbSet<T> EntitySet
        {
            get
            {
                return _context.Set<T>();
            }
        }

        protected async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<T> Add(T entity, ILogger log)
        {
            try
            {
                EntitySet.Add(entity);
                await Save();
                return entity;

            }
            catch (SqlException ex)
            {
                log.LogError(ex, $"Error al agregar la entidad: {ex.Message}");
                throw ex;
            }
        }

        //AddAll
        public async Task<IEnumerable<T>> AddAll(IEnumerable<T> entity, ILogger log)
        {
            try
            {
                EntitySet.AddRange(entity);
                await Save();
                return entity;

            }
            catch (SqlException ex)
            {
                log.LogError(ex, $"Error al agregar la entidad: {ex.Message}");
                throw ex;
            }
        }

        public async Task<int> Delete(int id, ILogger log)
        {
            try
            {
                T entity = EntitySet.Find(id);
                EntitySet.Remove(entity);
                await Save();
                return id;
            }
            catch (SqlException ex)
            {
                log.LogError(ex, $"Error al eliminar la entidad: {ex.Message}");
                throw ex;
            }
        }

        public async Task<long> Delete(long id, ILogger log)
        {
            try
            {
                T entity = EntitySet.Find(id);
                EntitySet.Remove(entity);
                await Save();
                return id;
            }
            catch (SqlException ex)
            {
                log.LogError(ex, $"Error al eliminar la entidad: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> GetAll(ILogger log)
        {
            return await EntitySet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(ILogger log, string[] include)
        {
            var entity = EntitySet.AsQueryable();

            foreach (var item in include)
            {
                entity = entity.Include(item);
            }

            return await entity.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(ILogger log, string[] include, Expression<Func<T, bool>> query)
        {
            var entity = EntitySet.Where(query);

            foreach (var item in include)
            {
                entity = entity.Include(item);
            }

            return await entity.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll(ILogger log, string[] include, Expression<Func<T, bool>> query, int skip, int take)
        {
            var entity = EntitySet.Where(query).Skip(skip).Take(take);

            foreach (var item in include)
            {
                entity = entity.Include(item);
            }

            return await entity.ToListAsync();
        }

        public async Task<T> GetById(int id, ILogger log)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task<T> GetById(long id, ILogger log)
        {
            return await EntitySet.FindAsync(id);
        }

        public async Task<T> Update(T entity, ILogger log)
        {
            try
            {
                EntitySet.Update(entity);
                await Save();
                return entity;
            }
            catch (SqlException ex)
            {
                log.LogError(ex, $"Error al actualizar la entidad: {ex.Message}");
                throw ex;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
