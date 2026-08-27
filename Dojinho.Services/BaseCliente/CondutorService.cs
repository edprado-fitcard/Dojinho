using System;
using System.Collections.Generic;
using Dojinho.Data.Interfaces.BaseCliente;
using Dojinho.Domain;
using Dojinho.Domain.BaseCliente;
using Dojinho.Domain.FitcarAutorizador;
using Dojinho.Domain.Icode;
using Dojinho.Services.Interfaces.BaseCliente;
using Dojinho.Services.Interfaces.Icode;
using Dojinho.Services.Utils;

namespace Dojinho.Services.BaseCliente
{
    public class CondutorService
    {
        private readonly ICondutorRepository _condutorRepository;
        private readonly IPermissaoXClienteService _permissaoXClienteService;
        private readonly IVeiculoService _veiculoService;
        private readonly List<Func<ValidacaoCondutorRequest, ERetorno?>> _regrasDeValidacao;

        public CondutorService(
            ICondutorRepository condutorRepository,
            IPermissaoXClienteService permissaoXClienteService,
            IVeiculoService veiculoService)
        {
            _condutorRepository = condutorRepository;
            _permissaoXClienteService = permissaoXClienteService;
            _veiculoService = veiculoService;

            _regrasDeValidacao = new List<Func<ValidacaoCondutorRequest, ERetorno?>>
            {
                ValidarExistencia,
                ValidarStatus,
                ValidarSenhaMestra,
                ProcessarCadastroDeSenha,
                ValidarSenha,
                ValidarIntervaloDeAbastecimento
            };
        }

        public ERetorno ValidarCondutor(ValidacaoCondutorRequest request)
        {
            foreach (var regra in _regrasDeValidacao)
            {
                var resultado = regra(request);

                if (resultado.HasValue)
                    return resultado.Value;
            }

            return ValidarCNH(request);
        }

        private ERetorno? ValidarExistencia(ValidacaoCondutorRequest request)
        {
            if (request.Condutor == null)
                return ERetorno.CondutorNaoLocalizado;

            return null;
        }

        private ERetorno? ValidarStatus(ValidacaoCondutorRequest request)
        {
            if (!request.Condutor.Status)
                return ERetorno.CondutorBloqueado;

            return null;
        }

        private ERetorno? ValidarSenhaMestra(ValidacaoCondutorRequest request)
        {
            if (request.Senha == "998877" && request.DescricaoEntrada == "MANUAL")
                return ValidarCNH(request);

            return null;
        }

        private ERetorno? ProcessarCadastroDeSenha(ValidacaoCondutorRequest request)
        {
            if (!string.IsNullOrEmpty(request.Condutor.Senha))
                return null;

            if (string.IsNullOrEmpty(request.Senha))
                return ERetorno.SenhaIncorreta;

            if (request.Senha.Length < 2)
                return ERetorno.SenhaMinimo2Digitos;

            _condutorRepository.CadastrarSenha(request.Condutor, request.Senha, request.BancoCliente);
            return ERetorno.EmProcesso;
        }

        private ERetorno? ValidarSenha(ValidacaoCondutorRequest request)
        {
            if (!GerenciadorDeHash.CompararSenhas(request.Condutor.Senha, request.Senha))
                return ERetorno.SenhaIncorreta;

            return null;
        }

        private ERetorno? ValidarIntervaloDeAbastecimento(ValidacaoCondutorRequest request)
        {
            if (request.Condutor.IntervaloAbastecimento == 0)
                return null;

            if (request.Condutor.UltimoAbastecimento == -1)
                return null;

            if (request.Condutor.UltimoAbastecimento >= request.Condutor.IntervaloAbastecimento)
                return null;

            if (request.Veiculo.tipocomb_veiculo == 4)
                return null;

            if (request.Veiculo.liberaVeiculo == 0)
                return ERetorno.TempoIntervaloCondutorExcedido;

            _veiculoService.AtualizarFlagLiberaVeiculo(request.Veiculo, request.BancoCliente);
            return null;
        }

        private ERetorno ValidarCNH(ValidacaoCondutorRequest request)
        {
            var permissao = _permissaoXClienteService.ObterPermissao(request.CodigoCliente, Permissao.MenuRestricao);

            if (permissao == null)
                return ERetorno.EmProcesso;

            var limiteDeValidade = request.Condutor.ValidadeCNH.AddDays(request.Condutor.ToleranciaCnhVencida);

            if (limiteDeValidade >= DateTime.Today)
                return ERetorno.EmProcesso;

            if (request.Condutor.TipoRestricaoValidadeCnh)
                return ERetorno.CNHVencida;

            return ERetorno.EmProcesso;
        }
    }
}