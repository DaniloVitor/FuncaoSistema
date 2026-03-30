using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FI.WebAtividadeEntrevista.Controllers
{
    public class BeneficiarioController : Controller
    {
        #region Campos

        BoBeneficiario _bo;

        #endregion


        #region Construtor

        public BeneficiarioController() 
        {
            _bo = new BoBeneficiario();
        }

        #endregion


        #region Views

        public ActionResult Incluir()
        {
            return PartialView("Incluir");
        }

        public PartialViewResult Lista()
        {
            List<BeneficiarioModel> lista = ObterBeneficiarios();
            return PartialView("Lista", lista);
        }

        #endregion


        #region Metodos

        [HttpPost]
        public ActionResult Alterar(BeneficiarioModel model)
        {
            List<BeneficiarioModel> lista = ObterBeneficiarios();
            string mensagem = _bo.ValidarCPF(model.CPF, model.Id);

            if (!string.IsNullOrEmpty(mensagem))
                ModelState.AddModelError("CPF", mensagem);

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                BeneficiarioModel item = lista.Find(b => b.Id == model.Id);

                if (item != null)
                {
                    item.Nome = model.Nome;
                    item.CPF = model.CPF;
                    item.State = EntityStateBeneficiario.Updated;
                }

                Session["Beneficiarios"] = lista;
            }
            
            return PartialView("Lista", lista);
        }

        [HttpPost]
        public ActionResult Excluir(long id)
        {
            List<BeneficiarioModel> lista = ObterBeneficiarios();
            BeneficiarioModel item = lista.Find(b => b.Id == id);

            if (item != null)
            {
                item.State = EntityStateBeneficiario.Deleted;
            }

            Session["Beneficiarios"] = lista;
            return PartialView("Lista", lista);
        }

        [HttpPost]
        public ActionResult Incluir(BeneficiarioModel model)
        {
            List<BeneficiarioModel> lista = ObterBeneficiarios();
            string mensagem = _bo.ValidarCPF(model.CPF);

            if (string.IsNullOrEmpty(mensagem))
            {
                if (lista.Any(b => b.CPF == model.CPF.Replace(".", "").Replace("-", "")))
                {
                    mensagem = "CPF ja cadastrado";
                }
            }

            if (!string.IsNullOrEmpty(mensagem))
            {
                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, mensagem));
            }
            else
            {
                lista.Add(new BeneficiarioModel
                {
                    Id = lista.Count > 0 ? lista.Max(x => x.Id) + 1 : 1,
                    IdCliente = model.IdCliente,
                    Nome = model.Nome,
                    CPF = model.CPF.Replace(".", "").Replace("-", ""),
                    State = EntityStateBeneficiario.New
                });

                Session["Beneficiarios"] = lista;
            }

            return PartialView("Lista", lista);
        }

        private List<BeneficiarioModel> ObterBeneficiarios()
        {
            long idCliente = Session["IdCliente"] == null ? 0 : Convert.ToInt64(Session["IdCliente"]);

            if (Session["Beneficiarios"] == null)
            {
                if (idCliente > 0)
                {
                    var listaBanco = _bo.Listar(idCliente);
                    var listaModel = listaBanco.Select(b => new BeneficiarioModel
                    {
                        Id = b.Id,
                        IdCliente = b.IdCliente,
                        Nome = b.Nome,
                        CPF = b.CPF
                    }).ToList();

                    Session["Beneficiarios"] = listaModel;
                }
                else
                {
                    Session["Beneficiarios"] = new List<BeneficiarioModel>();
                }
            }

            return (List<BeneficiarioModel>)Session["Beneficiarios"];
        }

        #endregion
    }
}