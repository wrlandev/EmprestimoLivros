using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLivros.Controllers
{
    public class Emprestimos : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
