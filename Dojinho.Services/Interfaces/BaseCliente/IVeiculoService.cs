using Dojinho.Domain.BaseCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dojinho.Services.Interfaces.BaseCliente
{
    public interface IVeiculoService
    {
        void AtualizarFlagLiberaVeiculo(Veiculo veiculo, string bancoCliente);
    }
}
