using Dojinho.Domain;
using Dojinho.Domain.Icode;
using Dojinho.Services.Interfaces.Icode;

namespace Dojinho.Services.Icode
{
    public class PermissaoXClienteService : IPermissaoXClienteService
    {
        public PermissaoXCliente ObterPermissao(int codigoCliente, Permissao permissao)
        {
            return new PermissaoXCliente
            {
                ID_Cliente = codigoCliente,
                ID_Permissao = 118,
                Valor = "1",
                ID_Auxiliar = "MenuRestricao",
                cod_permissao = 118,
                data_cadastro = DateTime.Now
            };
        }
    }
}
