using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EmprestimoLivros.Controllers
{
	public class EmprestimosController : Controller
	{
		readonly private ApplicationDbContext _context;
		public EmprestimosController(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			IEnumerable<EmprestimosModel> emprestimos = _context.Empretimos;
			return View(emprestimos);
		}

		public IActionResult Cadastrar()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Cadastrar(EmprestimosModel emprestimo)
		{
			if (ModelState.IsValid)
			{
				_context.Empretimos.Add(emprestimo);
				_context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult Editar(int? id)
		{
			if(id == null || id == 0)
			{
				return NotFound();
			}
			EmprestimosModel emprestimo = _context.Empretimos.FirstOrDefault(x => x.Id == id);

			if(emprestimo == null)
			{
				return NotFound();
			}

			return View(emprestimo);
		}

		[HttpPost]
		public IActionResult Editar(EmprestimosModel emprestimo)
		{
			if(ModelState.IsValid)
			{
				_context.Empretimos.Update(emprestimo);
				_context.SaveChanges();

				return RedirectToAction("Index");
			}

			return View(emprestimo);
		}

		public IActionResult Excluir(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			EmprestimosModel emprestimo = _context.Empretimos.FirstOrDefault(x => x.Id == id);

			if (emprestimo == null)
			{
				return NotFound();
			}

			return View(emprestimo);
		}

		[HttpPost]
		public IActionResult Excluir(EmprestimosModel emprestimo)
		{
			if (emprestimo == null)
			{
				return NotFound();
			}

			_context.Empretimos.Remove(emprestimo);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
