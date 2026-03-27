namespace Dojinho.Domain.FitcarAutorizador
{
    public enum ERetorno
    {
        EmProcesso = 0,
        CondutorNaoLocalizado = 3,
        CondutorBloqueado = 4,
        SenhaIncorreta = 22,
        CNHVencida = 91,
        SenhaMinimo2Digitos = 119,
        TempoIntervaloCondutorExcedido = 199,
        ErroIxeplicavel = 999
    }
}
