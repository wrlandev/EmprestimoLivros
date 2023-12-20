using DocumentFormat.OpenXml.Office2010.Drawing;
using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLivros.Services
{
    public class EmprestimoService : IEmprestimoInterface
    {
        private readonly AppDbContext _context;
        public EmprestimoService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<EmprestimoModel> Adicionar(EmprestimoModel emprestimo)
        {
            await _context.Empretimos.AddAsync(emprestimo);
            _context.SaveChanges();

            return emprestimo;
        }
        public async Task<EmprestimoModel> BuscarPorId(int id)
        {
            return await _context.Empretimos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<EmprestimoModel>> BuscarTodos()
        {
            return await _context.Empretimos.ToListAsync();
        }

        public async Task<bool> Deletar(int id)
        {
            EmprestimoModel emprestimoPorId = await BuscarPorId(id);

            if (emprestimoPorId == null)
            {
                throw new Exception("Registro não encontrado!");
            }

            _context.Empretimos.Remove(emprestimoPorId);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<EmprestimoModel> Editar(EmprestimoModel emprestimo, int id)
        {
            EmprestimoModel emprestimoPorId = await BuscarPorId(id);

            if (emprestimoPorId == null)
            {
                throw new Exception("Registro não encontrado!");
            }

            emprestimoPorId.Recebedor = emprestimo.Recebedor;
            emprestimoPorId.Fornecedor = emprestimo.Fornecedor;
            emprestimoPorId.LivroEmprestado = emprestimo.LivroEmprestado;

            _context.Empretimos.Update(emprestimoPorId);
            await _context.SaveChangesAsync();

            return emprestimoPorId;

        }
    }
}
