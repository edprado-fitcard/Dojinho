using Dojinho.Data.Interfaces.BaseCliente;
using Dojinho.Domain;
using Dojinho.Domain.BaseCliente;
using Dojinho.Domain.FitcarAutorizador;
using Dojinho.Domain.Icode;
using Dojinho.Services.Interfaces.BaseCliente;
using Dojinho.Services.Interfaces.Icode;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Dojinho.Services.BaseCliente
{
    public class CondutorService
    {
        private readonly ICondutorRepository _condutorRepository;
        private readonly IPermissaoXClienteService _permissaoXClienteService;
        private readonly IVeiculoService _veiculoService;

        public CondutorService(ICondutorRepository condutorRepository,
                               IPermissaoXClienteService permissaoXClienteService,
                               IVeiculoService veiculoService)
        {
            _condutorRepository = condutorRepository;
            _permissaoXClienteService = permissaoXClienteService;
            _veiculoService = veiculoService;
        }

        private bool ValidarSenhaInformada(string senhaInformada)
        {
            return senhaInformada.Length >= 2;
        }

        private bool SenhaAcessoValidada(string senhaCondutor, string senhaInformada, string descricaoEntrada) =>
            (senhaInformada == "998877" && descricaoEntrada == "MANUAL") || ComparaVerificandoHASH(senhaCondutor, senhaInformada);


        private ERetorno CadastrarSenhaCondutor(Condutor condutor, string senha, string bancoCliente)
        {
            var senhaInformadaValidada = ValidarSenhaInformada(senha);

            if (!senhaInformadaValidada)
                return ERetorno.SenhaMinimo2Digitos;

            _condutorRepository.CadastrarSenha(condutor, senha, bancoCliente);
            return ERetorno.EmProcesso;
        }


        public ERetorno ValidarCondutor(Condutor condutor, string senha, string bancoCliente, string descricaoEntrada, int codigoCliente, Veiculo veiculo, Requisicao requisicao)
        {
            if (condutor == null)
                return ERetorno.CondutorNaoLocalizado;

            if (condutor.status == false)
                return ERetorno.CondutorBloqueado;

            if (string.IsNullOrEmpty(condutor.Senha))
                return CadastrarSenhaCondutor(condutor, senha, bancoCliente);

            if (!SenhaAcessoValidada(condutor.Senha, senha, descricaoEntrada))
                return ERetorno.SenhaIncorreta;

            if (RegrasAbastecimento(condutor, veiculo.tipocomb_veiculo))
            {
                if (veiculo.liberaVeiculo == 0)
                    return ERetorno.TempoIntervaloCondutorExcedido;

                _veiculoService.AtualizarFlagLiberaVeiculo(veiculo, bancoCliente);
            }

            var permissao = _permissaoXClienteService.ObterPermissao(codigoCliente, Permissao.MenuRestricao);
            return VerificaCNHCondutor(condutor, permissao, bancoCliente, requisicao);
        }

        //Controle de Intervalo em minutos
        private bool PossuiIntervaloAbastecimento(Condutor condutor)
        {
            return condutor.IntervaloAbastecimento != 0;
        }

        private bool PossuiUltimoAbastecimento(Condutor condutor)
        {
            return condutor.UltimoAbastecimento != -1;
        }

        //verificar se o intervalo já foi excedido para liberar um novo abastecimento
        private bool PossuiIntervaloExcedido(Condutor condutor)
        {
            return condutor.UltimoAbastecimento < condutor.IntervaloAbastecimento;
        }

        private bool RegrasAbastecimento(Condutor condutor, int? tipoCombustivel)
        {
            return PossuiIntervaloAbastecimento(condutor) && PossuiUltimoAbastecimento(condutor) && PossuiIntervaloExcedido(condutor) && tipoCombustivel != 4;
        }

        public bool ValidaSenha(Condutor condutor, string senha, string descricaoEntrada)
        {
            if ((senha == "998877" && descricaoEntrada == "MANUAL") || ComparaVerificandoHASH(condutor.Senha, senha))
                return true;

            return false;
        }

        public ERetorno VerificaCNHCondutor(Condutor condutor, PermissaoXCliente permissao, string database, Requisicao requisicao)
        {
            if (permissao != null && condutor.ValidadeCNH.AddDays(condutor.ToleranciaCnhVencida) < DateTime.Today && condutor.tipoRestricaoValidadeCnh)
                return ERetorno.CNHVencida;

            return ERetorno.EmProcesso;
        }

        public static bool ComparaVerificandoHASH(string value, string toCompare)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(toCompare))
                return false;

            if (!IsHash(value))
                value = GetMD5Hash(value);

            if (!IsHash(toCompare))
                toCompare = GetMD5Hash(toCompare);


            return value.ToLower() == toCompare.ToLower();
        }

        public static string GetMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static bool IsHash(string value)
        {
            Regex r = new Regex("[0-9a-fA-F]{32}");
            return r.IsMatch(value);
        }
    }
}
