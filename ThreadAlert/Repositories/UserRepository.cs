using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadAlert.Entities;
using ThreadAlert.Persistance;

namespace ThreadAlert.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public Task<List<AppUser>> GetAll()
    {
        var result = _context.Users.Include(a => a.DangerousObjects).ToList();

        return Task.FromResult(result);
    }

    public Task<List<AppUser>> GetForDangerousObject(DangerousObject dangerousObject)
    {
        var result = _context.Users.Where(c => c.DangerousObjects.Contains(dangerousObject)).ToList();

        return Task.FromResult(result);
    }

    //public Task SetAlertOpt(string id,  AlertOptions alertOptions)
    //{
    //    var user = _context.Users.First(a => a.Id == id);

    //    user.AlertOptions = alertOptions;

    //    _context.SaveChanges();

    //    return Task.FromResult(user);
    //}

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
