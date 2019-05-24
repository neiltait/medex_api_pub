using System.Linq;
using System.Threading.Tasks;
using MedicalExaminer.ToolBox.Common.Services;
using MedicalExaminer.ToolBox.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicalExaminer.ToolBox.MVC.Controllers
{
    public class ImpersonateController : Controller
    {
        private readonly ImpersonateUserService _impersonateUserService;

        public ImpersonateController(
            ImpersonateUserService impersonateUserService)
        {
            _impersonateUserService = impersonateUserService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _impersonateUserService.GetUsers();

            return View(new ImpersonateModel
            {
                Email = "youremail@example.com",
                MeUsedId = null,
                UserOptions = users.Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.FullName,
                })
            });
        }

        public async Task<IActionResult> Update(ImpersonateModel model)
        {
            await _impersonateUserService.Update(model.MeUsedId, model.Email);

            return Redirect(nameof(Index));
        }
    }
}