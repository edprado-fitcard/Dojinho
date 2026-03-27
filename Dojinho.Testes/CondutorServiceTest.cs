using Dojinho.Data.Interfaces.BaseCliente;
using Dojinho.Domain;
using Dojinho.Domain.BaseCliente;
using Dojinho.Domain.FitcarAutorizador;
using Dojinho.Domain.Icode;
using Dojinho.Services.BaseCliente;
using Dojinho.Services.Interfaces.BaseCliente;
using Dojinho.Services.Interfaces.Icode;
using Moq;

namespace Dojinho.Testes
{
    [TestClass]
    public class CondutorServiceTest
    {
        private Mock<ICondutorRepository> _condutorRepositoryMock;
        private Mock<IPermissaoXClienteService> _permissaoServiceMock;
        private Mock<IVeiculoService> _veiculoServiceMock;

        private CondutorService _service;

        private const string _bancoCliente = "s_4074";
        private const int _codigoCliente = 4074;

        [TestInitialize]
        public void Setup()
        {
            _condutorRepositoryMock = new Mock<ICondutorRepository>();
            _permissaoServiceMock = new Mock<IPermissaoXClienteService>();
            _veiculoServiceMock = new Mock<IVeiculoService>();

            _service = new CondutorService(
                _condutorRepositoryMock.Object,
                _permissaoServiceMock.Object,
                _veiculoServiceMock.Object
            );
        }

        /// <summary>
        /// Valida condutor inexistente, deve retornar CondutorNaoLocalizado
        /// </summary>
        [TestMethod]
        public void DeveRetornarCondutorNaoLocalizadoQuandoCondutorForNull()
        {
            var resultado = _service.ValidarCondutor(null, "", _bancoCliente, "", _codigoCliente, new Veiculo(), new Requisicao());

            Assert.AreEqual(ERetorno.CondutorNaoLocalizado, resultado);
        }

        /// <summary>
        /// Valida condutor inativado, deve retornar CondutorBloqueado
        /// </summary>
        [TestMethod]
        public void DeveRetornarCondutorBloqueadoQuandoStatusForFalse()
        {
            var condutor = new Condutor { status = false };

            var resultado = _service.ValidarCondutor(condutor, "", _bancoCliente, "", _codigoCliente, new Veiculo(), new Requisicao());

            Assert.AreEqual(ERetorno.CondutorBloqueado, resultado);
        }

        /// <summary>
        /// Valida a senha de condutor que tenta realizar um abastecimento usando POS, deve retornar SenhaIncorreta.
        /// <para>POS: envia senha em texto plano.</para>
        /// <para>Senha do condutor está em texto plano no banco de dados.</para>
        /// </summary>
        [TestMethod]
        public void POSSenhaCondutorTextoPlanoDeveRetornarSenhaIncorretaQuandoSenhaInvalida()
        {
            var condutor = new Condutor
            {
                status = true,
                Senha = "123"
            };

            var requisicao = new Requisicao
            {
                Senha = "4321",
                DescricaoEntrada = "POS",
                ModoEntrada = ModoEntrada.POS
            };           

            var resultado = _service.ValidarCondutor(condutor, requisicao.Senha, _bancoCliente, requisicao.DescricaoEntrada, _codigoCliente, new Veiculo(), requisicao);

            Assert.AreEqual(ERetorno.SenhaIncorreta, resultado);
        }

        /// <summary>
        /// Valida a senha de condutor que tenta realizar um abastecimento usando POS, deve retornar SenhaIncorreta.
        /// <para>POS: envia senha em texto plano.</para>
        /// <para>Senha do condutor está em MD5 no banco de dados.</para>
        /// </summary>
        [TestMethod]
        public void POSSenhaCondutorMD5DeveRetornarSenhaIncorretaQuandoSenhaInvalida()
        {
            var condutor = new Condutor
            {
                status = true,
                Senha = CondutorService.GetMD5Hash("123")
            };

            var requisicao = new Requisicao
            {
                Senha = "4321",
                DescricaoEntrada = "POS",
                ModoEntrada = ModoEntrada.POS
            };

            var resultado = _service.ValidarCondutor(condutor, requisicao.Senha, _bancoCliente, requisicao.DescricaoEntrada, _codigoCliente, new Veiculo(), requisicao);

            Assert.AreEqual(ERetorno.SenhaIncorreta, resultado);
        }

        /// <summary>
        /// Valida a senha de condutor que tenta realizar um abastecimento usando TEF, deve retornar SenhaIncorreta.
        /// <para>TEF: envia senha em MD5.</para>
        /// <para>Senha do condutor está em texto plano no banco de dados.</para>
        /// </summary>
        [TestMethod]
        public void TEFSenhaCondutorTextoPlanoDeveRetornarSenhaIncorretaQuandoSenhaInvalida()
        {
            var condutor = new Condutor
            {
                status = true,
                Senha = "123"
            };

            var requisicao = new Requisicao
            {
                Senha = CondutorService.GetMD5Hash("4321"),
                DescricaoEntrada = "TEF",
                ModoEntrada = ModoEntrada.TEF
            };

            var resultado = _service.ValidarCondutor(condutor, requisicao.Senha, _bancoCliente, requisicao.DescricaoEntrada, _codigoCliente, new Veiculo(), requisicao);

            Assert.AreEqual(ERetorno.SenhaIncorreta, resultado);
        }

        /// <summary>
        /// Valida a senha de condutor que tenta realizar um abastecimento usando TEF, deve retornar SenhaIncorreta.
        /// <para>TEF: envia senha em MD5.</para>
        /// <para>Senha do condutor está em MD5 no banco de dados.</para>
        /// </summary>
        [TestMethod]
        public void TEFSenhaCondutorMD5DeveRetornarSenhaIncorretaQuandoSenhaInvalida()
        {
            var condutor = new Condutor
            {
                status = true,
                Senha = CondutorService.GetMD5Hash("123")
            };

            var requisicao = new Requisicao
            {
                Senha = CondutorService.GetMD5Hash("4321"),
                DescricaoEntrada = "TEF",
                ModoEntrada = ModoEntrada.TEF
            };

            var resultado = _service.ValidarCondutor(condutor, requisicao.Senha, _bancoCliente, requisicao.DescricaoEntrada, _codigoCliente, new Veiculo(), requisicao);

            Assert.AreEqual(ERetorno.SenhaIncorreta, resultado);
        }

        /// <summary>
        /// Valida uma senha fixa 998877 para entrada manual, quaquer coisa diferente deve retornar SenhaIncorreta.
        /// <para>Manual: envia senha fixa</para>
        /// </summary>
        [TestMethod]
        public void ManualDeveRetornarSenhaIncorretaQuandoSenhaInvalida()
        {
            var condutor = new Condutor
            {
                status = true,
                Senha = "123"
            };

            var requisicao = new Requisicao
            {
                Senha = "4321",
                DescricaoEntrada = "MANUAL",
                ModoEntrada = ModoEntrada.Manual
            };

            var resultado = _service.ValidarCondutor(condutor, requisicao.Senha, _bancoCliente, requisicao.DescricaoEntrada, _codigoCliente, new Veiculo(), requisicao);

            Assert.AreEqual(ERetorno.SenhaIncorreta, resultado);
        }

        [TestMethod]
        public void ManualDeveRetornarCNHVencidaQuandoPermissaoExisteECNHVencidaComRestricao()
        {
            var condutor = new Condutor
            {
                status = true,
                Senha = "123",
                ValidadeCNH = DateTime.Today.AddDays(-10),
                ToleranciaCnhVencida = 0,
                tipoRestricaoValidadeCnh = true
            };

            var veiculo = new Veiculo();

            var permissao = new PermissaoXCliente();

            var requisicao = new Requisicao
            {
                Senha = "998877",
                DescricaoEntrada = "MANUAL",
                ModoEntrada = ModoEntrada.Manual
            };

            _permissaoServiceMock
                .Setup(x => x.ObterPermissao(It.IsAny<int>(), Permissao.MenuRestricao))
                .Returns(permissao);

            var resultado = _service.ValidarCondutor(condutor, requisicao.Senha, _bancoCliente, requisicao.DescricaoEntrada, _codigoCliente, veiculo, requisicao);

            Assert.AreEqual(ERetorno.CNHVencida, resultado);
        }

        [TestMethod]
        public void DeveCadastrarSenhaQuandoCondutorNaoPossuiSenha()
        {
            var condutor = new Condutor
            {
                status = true,
                Senha = null
            };

            var requisicao = new Requisicao
            {
                Senha = "4321",
                DescricaoEntrada = "POS",
                ModoEntrada = ModoEntrada.POS
            };

            var resultado = _service.ValidarCondutor(condutor, requisicao.Senha, _bancoCliente, requisicao.DescricaoEntrada, _codigoCliente, new Veiculo(), requisicao);

            _condutorRepositoryMock.Verify(x => x.CadastrarSenha(condutor, requisicao.Senha, _bancoCliente), Times.Once);

            Assert.AreEqual(ERetorno.EmProcesso, resultado);
        }

        [TestMethod]
        public void DeveRetornarSenhaMinimo2DigitosQuandoSenhaCurta()
        {
            var condutor = new Condutor
            {
                status = true,
                Senha = null
            };

            var requisicao = new Requisicao
            {
                Senha = "1",
                DescricaoEntrada = "POS",
                ModoEntrada = ModoEntrada.POS
            };

            var resultado = _service.ValidarCondutor(condutor, requisicao.Senha, _bancoCliente, requisicao.DescricaoEntrada, _codigoCliente, new Veiculo(), requisicao);

            Assert.AreEqual(ERetorno.SenhaMinimo2Digitos, resultado);
        }

        [TestMethod]
        public void DeveRetornarTempoIntervaloCondutorExcedidoQuandoIntervaloNaoRespeitado()
        {
            var condutor = new Condutor
            {
                status = true,
                Senha = CondutorService.GetMD5Hash("123"),
                IntervaloAbastecimento = 10,
                UltimoAbastecimento = 5
            };

            var veiculo = new Veiculo
            {
                tipocomb_veiculo = 1,
                liberaVeiculo = 0
            };

            var requisicao = new Requisicao
            {
                Senha = "123",
                DescricaoEntrada = "POS",
                ModoEntrada = ModoEntrada.POS
            };

            var resultado = _service.ValidarCondutor(condutor, requisicao.Senha, _bancoCliente, requisicao.DescricaoEntrada, _codigoCliente, veiculo, requisicao);

            Assert.AreEqual(ERetorno.TempoIntervaloCondutorExcedido, resultado);
        }

        /// <summary>
        /// Cenário 1: Permissão EXISTE + CNH vencida + restrição ativa
        /// </summary>
        [TestMethod]
        public void DeveRetornarCNHVencidaQuandoPermissaoExisteECNHVencidaComRestricao()
        {
            var condutor = new Condutor
            {
                status = true,
                Senha = CondutorService.GetMD5Hash("123"),
                ValidadeCNH = DateTime.Today.AddDays(-10),
                ToleranciaCnhVencida = 0,
                tipoRestricaoValidadeCnh = true
            };

            var veiculo = new Veiculo();

            var permissao = new PermissaoXCliente();

            var requisicao = new Requisicao
            {
                Senha = "123",
                DescricaoEntrada = "POS",
                ModoEntrada = ModoEntrada.POS
            };

            _permissaoServiceMock
                .Setup(x => x.ObterPermissao(It.IsAny<int>(), Permissao.MenuRestricao))
                .Returns(permissao);

            var resultado = _service.ValidarCondutor(condutor, requisicao.Senha, _bancoCliente, requisicao.DescricaoEntrada, _codigoCliente, veiculo, requisicao);

            Assert.AreEqual(ERetorno.CNHVencida, resultado);
        }

        [TestMethod]
        public void DeveRetornarCNHVencidaQuandoRestricaoAtiva()
        {
            var condutor = new Condutor
            {
                ValidadeCNH = DateTime.Today.AddDays(-10),
                ToleranciaCnhVencida = 0,
                tipoRestricaoValidadeCnh = true
            };

            var permissao = new PermissaoXCliente();

            var resultado = _service.VerificaCNHCondutor(condutor, permissao, _bancoCliente, new Requisicao());

            Assert.AreEqual(ERetorno.CNHVencida, resultado);
        }
    }
}
