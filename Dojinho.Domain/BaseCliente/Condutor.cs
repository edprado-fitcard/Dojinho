namespace Dojinho.Domain.BaseCliente
{
    public class Condutor
    {
        public long Registro { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }        
        public bool status { get; set; }
        public int TotalErro { get; set; }        
        public DateTime ValidadeCNH { get; set; }
        public int IntervaloAbastecimento { get; set; }
        public int UltimoAbastecimento { get; set; }
        public int ToleranciaCnhVencida { get; set; }
        public bool tipoRestricaoValidadeCnh { get; set; }

    }
}
