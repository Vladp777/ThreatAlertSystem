using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ThreadAlert.Entities;
using ThreadAlert.Persistance;

namespace ThreadAlert.Repositories
{
    public class DangerousObjectsRepository
    {
        private readonly AppDbContext _context;

        public DangerousObjectsRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<DangerousObject> Create(DangerousObject entity)
        {
            _context.DangerousObjects.Add(entity);

            return Task.FromResult(entity);
        }

        public Task<DangerousObject> Delete(int Id)
        {
            var deleted = _context.DangerousObjects.First(x => x.Id == Id);

            _context.DangerousObjects.Remove(deleted);

            return Task.FromResult(deleted);
        }

        public Task<DangerousObject?> Get(int id)
        {
            var result = _context.DangerousObjects
                .FirstOrDefault(x => x.Id == id);

            return Task.FromResult(result);
        }

        public Task<List<DangerousObject>> GetAll()
        {
            List<DangerousObject> results = _context.DangerousObjects.ToList();

            return Task.FromResult(results);
        }


        public Task<DangerousObject> Update(DangerousObject entity)
        {
            var updatedEntity = _context.DangerousObjects
                .First(x => x.Id == entity.Id);

            updatedEntity.Name = entity.Name;

            return Task.FromResult(updatedEntity);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
