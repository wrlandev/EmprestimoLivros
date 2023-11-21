using ClosedXML.Excel;
using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data;

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
				emprestimo.dataAtualizacao = DateTime.Now;

				_context.Empretimos.Add(emprestimo);
				_context.SaveChanges();

				TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";

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
				var emprestimoDB = _context.Empretimos.Find(emprestimo.Id);

				emprestimoDB.Fornecedor = emprestimo.Fornecedor;
				emprestimoDB.Recebedor = emprestimo.Recebedor;
				emprestimoDB.LivroEmprestado = emprestimo.LivroEmprestado;

				_context.Empretimos.Update(emprestimoDB);
				_context.SaveChanges();

				TempData["MensagemSucesso"] = "Edição realizado com sucesso!";

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

			TempData["MensagemSucesso"] = "Exclusão realizado com sucesso!";

			return RedirectToAction("Index");
		}

		public IActionResult Exportar()
		{
			var dados = GetDados();

			using(XLWorkbook workbook = new XLWorkbook())
			{
				workbook.AddWorksheet(dados,"Dados Empréstimo");

				using(MemoryStream ms = new MemoryStream())
				{
					workbook.SaveAs(ms);
					return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spredsheetml.sheet", "Emprestimo.xls");
				}
            }
		}

		private DataTable GetDados()
		{
			DataTable dataTable = new DataTable();

			dataTable.TableName = "Dados empréstimos";

			dataTable.Columns.Add("Recebedor", typeof(string));
			dataTable.Columns.Add("Fornecedor", typeof(string));
            dataTable.Columns.Add("Livro", typeof(string));
            dataTable.Columns.Add("Data empréstimo", typeof(DateTime));

			var dados = _context.Empretimos.ToList();

			if(dados.Count > 0)
			{
				dados.ForEach(item =>
				{
					dataTable.Rows.Add(item.Recebedor, item.Fornecedor, item.LivroEmprestado, item.dataAtualizacao);
				});
			}

            return dataTable;
		}
	}
}
