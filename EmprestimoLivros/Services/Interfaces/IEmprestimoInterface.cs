using EmprestimoLivros.Models;

namespace EmprestimoLivros.Services.Interfaces
{
    public interface IEmprestimoInterface
    {
        Task<List<EmprestimoModel>> BuscarTodos();
        Task<EmprestimoModel> BuscarPorId(int id);
        Task<EmprestimoModel> Adicionar(EmprestimoModel emprestimo);
        Task<EmprestimoModel> Editar(EmprestimoModel emprestimo, int id);
        Task<bool> Deletar(int id);
    }
}
