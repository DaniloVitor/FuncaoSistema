using System.Collections.Generic;
using System.Linq;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Incluir(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF, long id = 0)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF, id);
        }

        /// <summary>
        /// ValidarCPF
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public string ValidarCPF(string CPF, long id = 0)
        {
            string mensagem = string.Empty;

            if (VerificarExistencia(CPF, id))
                mensagem = "CPF ja cadastrado";

            if (string.IsNullOrEmpty(CPF))
            {
                return mensagem = "CPF é obrigatório";
            }

            CPF = CPF.Replace(".", "").Replace("-", "");

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
