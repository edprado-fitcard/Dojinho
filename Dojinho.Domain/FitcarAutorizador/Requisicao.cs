namespace Dojinho.Domain.FitcarAutorizador
{
    public class Requisicao
    {
        public int Id { get; set; }   

        public ModoEntrada ModoEntrada { get; set; }

        public string Mensagem { get; set; }

        public string MensagemResposta { get; set; }

        public string CodigoMensagem { get; set; }

        public string CodigoProcessamento { get; set; }

        public decimal? Valor { get; set; }
      
        public string ModoEntradaCartao { get; set; }

        public string Trilha2 { get; set; }        

        public string IdentificacaoEstabelecimento { get; set; }  

        public string Senha { get; set; }

        public string NSU { get; set; }
        
        public DateTime? DataRequisicao { get; set; }

        public int? CodigoTransacaoREF { get; set; }

        public string DescricaoEntrada { get; set; }

    }
}
