using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Dojinho.Services.Utils
{
    public static class GerenciadorDeHash
    {
        public static bool CompararSenhas(string hashCadastrado, string senhaInformada)
        {
            if (EntradasInvalidas(hashCadastrado, senhaInformada))
                return false;

            var cadastroFormatado = PadronizarParaHash(hashCadastrado);
            var senhaFormatada = PadronizarParaHash(senhaInformada);

            return cadastroFormatado.Equals(senhaFormatada, StringComparison.OrdinalIgnoreCase);
        }

        public static string GerarHashMD5(string value)
        {
            using var md5 = MD5.Create();
            var bytesValue = Encoding.ASCII.GetBytes(value);
            var bytesHash = md5.ComputeHash(bytesValue);

            return ConverterParaHexadecimal(bytesHash);
        }

        private static bool EntradasInvalidas(string entradaBase, string entradaAComparar)
        {
            return string.IsNullOrEmpty(entradaBase) || string.IsNullOrEmpty(entradaAComparar);
        }

        private static bool PossuiFormatoHash(string value)
        {
            var regexValidadorDeHash = new Regex("^[0-9a-fA-F]{32}$");
            return regexValidadorDeHash.IsMatch(value);
        }

        private static string PadronizarParaHash(string value)
        {
            if (PossuiFormatoHash(value))
                return value;

            return GerarHashMD5(value);
        }


        private static string ConverterParaHexadecimal(byte[] bytesHash)
        {
            var resultHexadecimal = new StringBuilder();

            foreach (var b in bytesHash)
            {
                resultHexadecimal.Append(b.ToString("X2"));
            }

            return resultHexadecimal.ToString();
        }
    }
}