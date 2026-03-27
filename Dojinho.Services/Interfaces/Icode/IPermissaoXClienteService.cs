using Dojinho.Domain;
using Dojinho.Domain.Icode;

namespace Dojinho.Services.Interfaces.Icode
{
    public interface IPermissaoXClienteService
    {
        PermissaoXCliente ObterPermissao(int codigoCliente, Permissao permissao);
    }
}
