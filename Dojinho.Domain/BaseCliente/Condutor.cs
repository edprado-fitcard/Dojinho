using System;

namespace Dojinho.Domain.BaseCliente
{
    public class Condutor
    {
        public long Registro { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public bool Status { get; set; }
        public int TotalErro { get; set; }
        public DateTime ValidadeCNH { get; set; }
        public int IntervaloAbastecimento { get; set; }
        public int UltimoAbastecimento { get; set; }
        public int ToleranciaCnhVencida { get; set; }
        public bool TipoRestricaoValidadeCnh { get; set; }
    }
}