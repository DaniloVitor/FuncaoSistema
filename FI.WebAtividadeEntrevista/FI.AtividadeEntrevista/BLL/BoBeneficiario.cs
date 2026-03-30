using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {

        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Alterar(beneficiario);
        }

        /// <summary>
        /// Consulta o beneficiário pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public DML.Beneficiario Consultar(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Consultar(id);
        }

        /// <summary>
        /// Excluir o beneficiário pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Excluir(id);
        }

        /// <summary>
        /// Lista os beneficiario
        /// </summary>
        public List<DML.Beneficiario> Listar(long idBeneficiario = 0)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Listar(idBeneficiario);
        }

        /// <summary>
        /// Lista os beneficiário
        /// </summary>
        public List<DML.Beneficiario> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF, long id = 0)
        {
            DAL.DaoBeneficiario beneficiario = new DAL.DaoBeneficiario();
            return beneficiario.VerificarExistencia(CPF, id);
        }

        /// <summary>
        /// VerificarExistenciaPorId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool VerificarExistenciaPorId(long id)
        {
            DAL.DaoBeneficiario beneficiario = new DAL.DaoBeneficiario();
            return beneficiario.VerificarExistenciaPorId(id);
        }

        /// <summary>
        /// ValidarCPF
        /// </summary>
        /// <param name="CPF"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ValidarCPF(string CPF, long id = 0)
        {
            string mensagem = string.Empty;
            CPF = CPF.Replace(".", "").Replace("-", "");

            if (string.IsNullOrEmpty(CPF))
                return mensagem = "CPF é obrigatório";

            if (VerificarExistencia(CPF, id))
                mensagem = "CPF ja cadastrado";

            if (string.IsNullOrEmpty(mensagem))
            {
                if (CPF.Length != 11) return mensagem = "CPF inválido";
            }

            if (string.IsNullOrEmpty(mensagem))
            {
                if (CPF.All(c => c == CPF[0])) return mensagem = "CPF inválido";
            }

            if (string.IsNullOrEmpty(mensagem))
            {
                int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                var tempCpf = CPF.Substring(0, 9);
                int soma = 0;

                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * mult1[i];

                int resto = soma % 11;
                resto = resto < 2 ? 0 : 11 - resto;

                var digito = resto.ToString();
                tempCpf += digito;

                soma = 0;

                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * mult2[i];

                resto = soma % 11;
                resto = resto < 2 ? 0 : 11 - resto;

                digito += resto.ToString();

                if (!CPF.EndsWith(digito))
                    mensagem = "CPF inválido";
            }

            return mensagem;
        }
    }
}
