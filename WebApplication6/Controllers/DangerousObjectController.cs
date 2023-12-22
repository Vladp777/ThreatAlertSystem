using Microsoft.AspNetCore.Mvc;
using ThreadAlert.Entities;
using ThreadAlert.Repositories;

namespace WebApplication6.Controllers
{
    public class DangerousObjectController : Controller
    {
        private readonly DangerousObjectsRepository _dangerousObjectsRepository;

        public DangerousObjectController(DangerousObjectsRepository dangerousObjectsRepository)
        {
            _dangerousObjectsRepository = dangerousObjectsRepository;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(DangerousObject model)
        {
            // Логіка для збереження нового небезпечного об'єкту
            // ...
            await _dangerousObjectsRepository.Create(model);
            await _dangerousObjectsRepository.SaveAsync();
            return View(model);
            
        }
    }

}
