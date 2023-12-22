using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadAlert.Entities;
using ThreadAlert.Repositories;
using WebApplication6.Hubs;

namespace WebApplication6.Services;

public class AlertService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly DangerousObjectsRepository _dangerousObjectRepository;


    public AlertService(DangerousObjectsRepository dangerousObjectRepository, UserManager<AppUser> userManager)
    {
        _dangerousObjectRepository = dangerousObjectRepository;
        _userManager = userManager;
    }

    public async Task NotifyUsersAsync(Message message)
    {
        var obj = await _dangerousObjectRepository.Get(message.DangerousObjectId);

        if (obj == null)
        {
            throw new Exception();
        }
        var users = _userManager.Users
            .Include(a => a.DangerousObjects)
            .Include(a => a.Messages)
            .Where(a => a.DangerousObjects.Contains(obj))
            .ToList();

        foreach (var user in users)
        {
            user.Messages.Add(message);
            await _userManager.UpdateAsync(user);
        }

        //await _notificationHub.Clients.All.SendCoreAsync("Receive", new object[] {message});

        await _dangerousObjectRepository.SaveAsync();
    }
}
