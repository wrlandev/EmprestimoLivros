using ClosedXML.Excel;
using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmprestimoLivros.Controllers
{
    public class EmprestimosController : Controller
    {
        private readonly IEmprestimoInterface _iemprestimo;
        private readonly AppDbContext _context;
        public EmprestimosController(IEmprestimoInterface emprestimo, AppDbContext context)
        {
            _iemprestimo = emprestimo;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var buscarTodos = await _iemprestimo.BuscarTodos();
            return View(buscarTodos);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(EmprestimoModel emprestimo)
        {
            if (ModelState.IsValid)
            {
                emprestimo.dataAtualizacao = DateTime.Now;

                await _iemprestimo.Adicionar(emprestimo);

                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";

                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Editar(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimoModel emprestimo = await _iemprestimo.BuscarPorId(id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EmprestimoModel emprestimo, int id)
        {
            if (ModelState.IsValid)
            {
                await _iemprestimo.Editar(emprestimo, id);

                TempData["MensagemSucesso"] = "Cadastro editado com sucesso!";

                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Excluir(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmprestimoModel emprestimo = await _iemprestimo.BuscarPorId(id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        [HttpPost]
        public async Task<IActionResult> Deletar(int id)
        {
            await _iemprestimo.Deletar(id);

            TempData["MensagemSucesso"] = "Cadastro excluído com sucesso!";

            return RedirectToAction("Index");
        }

        public IActionResult Exportar()
        {
            var dados = GetDados();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.AddWorksheet(dados, "Dados Empréstimo");

                using (MemoryStream ms = new MemoryStream())
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

            if (dados.Count > 0)
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
