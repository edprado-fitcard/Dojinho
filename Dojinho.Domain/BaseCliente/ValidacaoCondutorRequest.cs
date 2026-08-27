using Dojinho.Domain.FitcarAutorizador;
using Dojinho.Domain.Icode;

namespace Dojinho.Domain.BaseCliente
{
    public class ValidacaoCondutorRequest
    {
        public Condutor Condutor { get; set; }
        public string Senha { get; set; }
        public string BancoCliente { get; set; }
        public string DescricaoEntrada { get; set; }
        public int CodigoCliente { get; set; }
        public Veiculo Veiculo { get; set; }
        public Requisicao Requisicao { get; set; }
    }
}