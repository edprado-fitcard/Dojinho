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

            if (condutor.status)
            {
                if (senha == "998877" && descricaoEntrada == "MANUAL")
                {

                    return VerificaCNHCondutor(condutor, permissao, bancoCliente, requisicao);
                }
                else
                {
                    if (!string.IsNullOrEmpty(condutor.Senha))
                    {
                        if (ValidaSenha(condutor, senha, descricaoEntrada))
                        {
                            //Controle de Intervalo em minutos
                            if (condutor.IntervaloAbastecimento != 0)
                            {
                                // Verificar se o condutor tem um abastecimento anterior, caso tenha verificar se o intervalo já foi excedido para liberar um novo abastecimento
                                if (condutor.UltimoAbastecimento != -1)
                                {
                                    if (condutor.UltimoAbastecimento < condutor.IntervaloAbastecimento)
                                    {
                                        // Verificar se o tipo de combustível é diferente de 4 (Flex), caso seja diferente, verificar a flag liberaVeiculo para liberar ou não um novo abastecimento
                                        if (veiculo.tipocomb_veiculo != 4)
                                        {
                                            if (veiculo.liberaVeiculo == 0)
                                            {
                                                return ERetorno.TempoIntervaloCondutorExcedido;
                                            }
                                            else
                                            {
                                                _veiculoService.AtualizarFlagLiberaVeiculo(veiculo, bancoCliente);
                                            }
                                        }
                                    }
                                }
                            }

                            return VerificaCNHCondutor(condutor, permissao, bancoCliente, requisicao);
                        }
                        else return ERetorno.SenhaIncorreta;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(senha))
                        {
                            // Validar se tem pelo menos 2 caracteres a nova senha(igual no POS)
                            if (senha.Length >= 2)
                            {
                                _condutorRepository.CadastrarSenha(condutor, senha, bancoCliente);
                                return ERetorno.EmProcesso;
                            }
                            else return ERetorno.SenhaMinimo2Digitos;
                        }
                        else return ERetorno.SenhaIncorreta; 
                    }
                }
            }

            else return ERetorno.CondutorBloqueado;
        }

        public bool ValidaSenha(Condutor condutor, string senha, string descricaoEntrada)
        {
            if (senha == "998877" && descricaoEntrada == "MANUAL")
                return true;
            if (ComparaVerificandoHASH(condutor.Senha, senha))
                return true;
            else
                return false;
        }

        public ERetorno VerificaCNHCondutor(Condutor condutor, PermissaoXCliente permissao, string database, Requisicao requisicao)
        {
            if (permissao != null && condutor.ValidadeCNH.AddDays(condutor.ToleranciaCnhVencida) < DateTime.Today)
            {
                if (condutor.tipoRestricaoValidadeCnh)
                {
                    return ERetorno.CNHVencida;
                }
                else
                {                  
                    return ERetorno.EmProcesso;
                }
            }

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


            if (value.ToLower() == toCompare.ToLower()) return true;
            return false;
        }

        public static string GetMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static bool IsHash(string value)
        {
            Regex r = new Regex("[0-9a-fA-F]{32}");
            if (r.IsMatch(value))
                return true;
            else
                return false;
        }
    }
}
