using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dojinho.Domain.BaseCliente
{
    public class Veiculo
    {
        public int cod_veiculo { get; set; }

        public bool? cod_master_veiculo { get; set; }

        public string mo_veiculo { get; set; }

        public string ma_veiculo { get; set; }

        public string placa_veiculo { get; set; }

        public int? cod_unidade_veiculo { get; set; }

        public string prefixo_veiculo { get; set; }

        public string vloc_veiculo { get; set; }

        public int? tipocomb_veiculo { get; set; }

        public int? qtde_abastecimento_dia_veiculo { get; set; }

        public int? controle_tipo_veiculo { get; set; }

        public double? saldo_restante_veiculo { get; set; }

        public double? saldo_contratado_veiculo { get; set; }

        public int? km_veiculo { get; set; }

        public int? cap_tanque_veiculo { get; set; }

        public string nm_cartao_veiculo { get; set; }

        public string chassi_veiculo { get; set; }

        public bool? status_veiculo { get; set; }

        public double? kgGEE_km { get; set; }

        public string ano_veiculo { get; set; }

        public string cor_veiculo { get; set; }

        public double? limite_transacao_veiculo { get; set; }

        public string tipo_veiculo { get; set; }

        public int? controle_intervalo_veiculo { get; set; }

        public bool? bloqueia_km_veiculo { get; set; }

        public int? comb_primario { get; set; }

        public int? limite_comb_secundario { get; set; }

        public bool? bloqueia_tanque_veiculo { get; set; }

        public double? mediakm_minima_veiculo { get; set; }

        public double? mediakm_maxima_veiculo { get; set; }

        public bool? bloqueiamediakm_veiculo { get; set; }

        public double? mediakm_veiculo { get; set; }

        public string condutores_liberados_veiculo { get; set; }

        public DateTime? ipva_veiculo { get; set; }

        public DateTime? seguro_veiculo { get; set; }

        public bool? ipva_ativo_veiculo { get; set; }

        public bool? seguro_ativo_veiculo { get; set; }

        public int? controle_qtde_abastecimentos { get; set; }

        public string tipo_qtde_abastecimentos { get; set; }

        public int? cod_unidade2_veiculo { get; set; }

        public string nm_cartao_auxiliar_veiculo { get; set; }

        public int? cod_unidade_manutencao { get; set; }

        public double? saldo_contratadoaux_veiculo { get; set; }

        public double? saldo_restante_manut_veiculo { get; set; }

        public double? saldo_contratado_manut_veiculo { get; set; }

        public int? controle_tipo_veiculo_manut { get; set; }

        public int? liberaVeiculo { get; set; }

        public int? cod_modelo_veiculo { get; set; }

        public DateTime? UltimaRevisao { get; set; }

        public int? qtdMaxTransacao { get; set; }

        public double? valorMaxTransacao { get; set; }

        public bool? veiculo_provisorio { get; set; }

        public double? valor_veiculo { get; set; }

        public DateTime? dataInativarVeiculo { get; set; }

        public string renavam_veiculo { get; set; }

        public DateTime? data_cadastro_veiculo { get; set; }

        public DateTime? dataReativarVeiculo { get; set; }

        public byte id_status { get; set; }

        public DateTime? data_licenciamento { get; set; }

        public string base_veiculo { get; set; }

        public decimal? cap_oleo_veiculo { get; set; }

        public bool? bloqueia_oleo_veiculo { get; set; }

        public string patrimonio_veiculo { get; set; }

        public int? adesivado { get; set; }

        public int? categoria_veiculo { get; set; }

        public string cnpjFornecedor { get; set; }

        public string cnpjLocadora { get; set; }

        public int? cod_cidade { get; set; }

        public string complemento_tipo { get; set; }

        public DateTime? dataUltimoAjuste { get; set; }

        public DateTime? dataUltimoAjuste_manut { get; set; }

        public string motorizacao { get; set; }

        public string num_serie_motor { get; set; }

        public string observacao_veiculo { get; set; }

        public string razaoFornecedor { get; set; }

        public string razaoLocadora { get; set; }

        public int? regDetran { get; set; }

        public int? status_complementar { get; set; }

        public double? valorAquisicao { get; set; }

        public double? valorLocacao { get; set; }

        public double? valorTotal { get; set; }

        public double? valorTotal_manut { get; set; }

        public double? valorTotalPeriodo { get; set; }

        public double? valorTotalPeriodo_manut { get; set; }

        public double? valorUltimoAjuste { get; set; }

        public double? valorUltimoAjuste_manut { get; set; }

        public short? CodigoBaseVeiculo { get; set; }

        public DateTime? VigenciaInicioLocacao { get; set; }

        public DateTime? VigenciaFimLocacao { get; set; }

        public string CodigoFipe { get; set; }

        public bool? PrefixoAtivo { get; set; }

        public byte? Transmissao { get; set; }

        public string lugares_veiculo { get; set; }
    }
}
