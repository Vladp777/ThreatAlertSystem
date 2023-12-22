using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadAlert.DTOs;
using ThreadAlert.Entities;
using ThreadAlert.Repositories;

namespace WebApplication6.Services;

public class MessageService
{
    private readonly MessageRepository _messageRepository;
    private readonly AlertService _alertService;

    public MessageService(MessageRepository messageRepository, AlertService alertService)
    {
        _messageRepository = messageRepository;
        _alertService = alertService;
    }

    public async Task<Message> CreateMessage(CreateMessage entity)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            IsActived = true,
            DateTime = entity.DateTime,
            DangerousObjectId = entity.DangerousObjectId,
            Description = entity.Description,
            Location = entity.Location,
            Priority = entity.Priority,
            Title = entity.Title
        };

        var result = await _messageRepository.Create(message);

        await _alertService.NotifyUsersAsync(message);

        return result;
    }

    public async Task ChangeMessageStatus(UpdateMessageStatus status)
    {
        await _messageRepository.UpdateStatus(status.id, status.status);

    }

}
