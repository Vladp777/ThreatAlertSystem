using Microsoft.EntityFrameworkCore;
using ThreadAlert.Entities;
using ThreadAlert.Persistance;

namespace ThreadAlert.Repositories;

public class MessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Message> Create(Message entity)
    {
        _context.Messages.Add(entity);
        return Task.FromResult(entity);
    }

    public Task<Message> Delete(Guid Id)
    {
        var deleted = _context.Messages.First(x => x.Id == Id);

        _context.Messages.Remove(deleted);

        return Task.FromResult(deleted);
    }

    public Task<Message?> Get(Guid id)
    {
        var result = _context.Messages
            .Include(x => x.DangerousObject)
            .FirstOrDefault(x => x.Id == id);

        return Task.FromResult(result);
    }

    public Task<List<Message>> GetAll()
    {
        List<Message> results = _context.Messages
            .Include(a => a.DangerousObject)
            .OrderByDescending(x => x.DateTime)
            .ToList();

        return Task.FromResult(results);
    }

    public Task<Message> UpdateStatus(Guid id, bool isActived)
    {
        var updatedEntity = _context.Messages
            .First(x => x.Id == id);

        updatedEntity.IsActived = isActived;

        return Task.FromResult(updatedEntity);
    }
    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public Task<bool> Update(Message message)
    {
        var oldmessage = _context.Messages.First(x => x.Id == message.Id);

        oldmessage.DangerousObjectId = message.DangerousObjectId;
        oldmessage.Title = message.Title;
        oldmessage.Priority = message.Priority;
        oldmessage.Description = message.Description;
        oldmessage.DateTime = message.DateTime;
        oldmessage.Location = message.Location;
        oldmessage.IsActived = message.IsActived;

        var result = _context.Messages.Update(oldmessage);

        return Task.FromResult(true);

    }
}
