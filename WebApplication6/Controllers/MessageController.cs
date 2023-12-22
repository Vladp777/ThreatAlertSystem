using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThreadAlert.Repositories;
using WebApplication6.Services;
using ThreadAlert.Entities;
using Microsoft.AspNetCore.Authorization;
using ThreadAlert.DTOs;

namespace WebApplication6.Controllers;

[Authorize(Roles = "Admin")]
public class MessageController : Controller
{
    private readonly MessageService _messageService;
    private readonly MessageRepository _messageRepository;
    private readonly DangerousObjectsRepository _dangerousObjectsRepository;


    public MessageController(MessageService messageService, 
        MessageRepository messageRepository, 
        DangerousObjectsRepository dangerousObjectsRepository)
    {
        _messageService = messageService;
        _messageRepository = messageRepository;
        _dangerousObjectsRepository = dangerousObjectsRepository;
    }

    public IActionResult Index()
    {
        var result = _messageRepository.GetAll().Result;
        return View(result);  
    }

    // GET: HomeController1/Details/5
    public ActionResult Details(Guid id)
    {
        var message = _messageRepository.Get(id).Result;
        return View(message);
    }

    // GET: HomeController1/Create
    public async Task<IActionResult> CreateAsync()
    {
        var message = new CreateMessage();
        ViewBag.DangerousObjects = await _dangerousObjectsRepository.GetAll();
        ViewBag.Priorities = Enum.GetValues(typeof(Priority));
        return View(message);
    }

    // POST: HomeController1/Create
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateMessage message)
    {
        try
        {
            //if (ModelState.IsValid)
            {
                await _messageService.CreateMessage(message);
                return RedirectToAction("Index");

            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(String.Empty, ex.Message);

        }

        return View(message);
    }

    public async Task<ActionResult> EditAsync(Guid id)
    {
        var message = _messageRepository.Get(id).Result;
        ViewBag.DangerousObjects = await _dangerousObjectsRepository.GetAll();
        ViewBag.Priorities = Enum.GetValues(typeof(Priority));

        return View(message);
    }
    [HttpPost]
    public async Task<ActionResult> EditAsync(Guid id, Message message)
    {       
        var oldmessage = await _messageRepository.Get(id);
        try
        {
            var result = await _messageRepository.Update(message);
            if (result)
            {
                await _messageRepository.SaveAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(oldmessage);
        }
        catch
        {
            return View(oldmessage);
        }
    }

    public ActionResult Delete(Guid id)
    {
        var message = _messageRepository.Get(id).Result;

        return View(message);
    }

    [HttpPost]
    public async Task<ActionResult> DeleteAsync(Guid id, IFormCollection collection)
    {
        var message = _messageRepository.Get(id).Result;

        try
        {
            await _messageRepository.Delete(id);
            await _messageRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View(message);
        }
    }
}
