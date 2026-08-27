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

        public ERetorno ValidarCondutor(Condutor condutor, string senha, string bancoCliente, string descricaoEntrada, int codigoCliente, Veiculo veiculo, Requisicao requisicao)
        {
            var permissao = _permissaoXClienteService.ObterPermissao(codigoCliente, Permissao.MenuRestricao);

            if (condutor == null)
                return ERetorno.CondutorNaoLocalizado;

            if (condutor.status == false)
                return ERetorno.CondutorBloqueado;

            if (senha == "998877" && descricaoEntrada == "MANUAL")
                return VerificaCNHCondutor(condutor, permissao, bancoCliente, requisicao);

            if (!string.IsNullOrEmpty(condutor.Senha))
            {
                if (ValidaSenha(condutor, senha, descricaoEntrada))
                {
                    if (RegrasAbastecimento(condutor))
                    {
                        // Verificar se o tipo de combustível é diferente de 4 (Flex), caso seja diferente, verificar a flag liberaVeiculo para liberar ou não um novo abastecimento
                        if (veiculo.tipocomb_veiculo != 4)
                        {
                            if (veiculo.liberaVeiculo == 0)
                                return ERetorno.TempoIntervaloCondutorExcedido;

                            _veiculoService.AtualizarFlagLiberaVeiculo(veiculo, bancoCliente);
                        }
                    }

                    return VerificaCNHCondutor(condutor, permissao, bancoCliente, requisicao);
                }

                return ERetorno.SenhaIncorreta;
            }

            if (!string.IsNullOrEmpty(senha))
            {
                // Validar se tem pelo menos 2 caracteres a nova senha(igual no POS)
                if (senha.Length >= 2)
                {
                    _condutorRepository.CadastrarSenha(condutor, senha, bancoCliente);
                    return ERetorno.EmProcesso;
                }

                return ERetorno.SenhaMinimo2Digitos;
            }

            return ERetorno.SenhaIncorreta;
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

        private bool RegrasAbastecimento(Condutor condutor)
        {
            return PossuiIntervaloAbastecimento(condutor) && PossuiUltimoAbastecimento(condutor) && PossuiIntervaloExcedido(condutor);
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
