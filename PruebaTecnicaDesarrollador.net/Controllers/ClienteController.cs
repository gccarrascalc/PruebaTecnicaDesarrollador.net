using PruebaTecnicaDesarrollador.net.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PruebaTecnicaDesarrollador.net.Models;
using System.Net.Mail;
using System.Net;

namespace PruebaTecnicaDesarrollador.net.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View(new List<ListaViewModel>());
        }

        [HttpPost]

        public ActionResult Index(HttpPostedFileBase postedfile)
        {
            List<ListTablaViewModel> tabla = new List<ListTablaViewModel>();
            string filepath = string.Empty;
            if(postedfile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                filepath = path + Path.GetFileName(postedfile.FileName);
                string extension = Path.GetExtension(postedfile.FileName);
                postedfile.SaveAs(filepath);

                string csvData = System.IO.File.ReadAllText(filepath);
     
                foreach(string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        tabla.Add(new ListTablaViewModel
                        {
                            nombre = row.Split(';')[0],
                            apellido = row.Split(';')[1],
                            identificacion = row.Split(';')[2],
                            celular = row.Split(';')[3],
                            direccion = row.Split(';')[4],
                            ciudad = row.Split(';')[5],
                            correo = row.Split(';')[6],
                        });
                    }
                }
                


            }
            using (UsuarioPruebaTecnicaEntities2 db = new UsuarioPruebaTecnicaEntities2())
            {
                var oTabla = new cliente();
                string co = "";
                foreach (ListTablaViewModel datos in tabla)
                {
                    try
                    {
                        oTabla.nombre = datos.nombre;
                        oTabla.apellido = datos.apellido;
                        oTabla.identificacion = datos.identificacion;
                        oTabla.celular = datos.celular;
                        oTabla.direccion = datos.direccion;
                        oTabla.ciudad = datos.ciudad;
                        oTabla.correo = datos.correo;
                        db.cliente.Add(oTabla);
                        db.SaveChanges();
                        co = datos.correo.Trim();
                        string body = "<body>" + "<h1> Datos almacenados correctamente del usuario Pagos Inteligentes</ h1>" + "</body>";
                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress("camic99901@gmail.com");
                        msg.To.Add(new MailAddress(co));
                        msg.Subject = "Registro Exitoso";
                        msg.IsBodyHtml = true;
                        msg.Body = body;
                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.Port = 587;
                        smtp.Credentials = new NetworkCredential("camic99901@gmail.com", "camilo201099");
                        smtp.EnableSsl = true;
                        smtp.Send(msg);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            List<ListaViewModel> users;
            using (UsuarioPruebaTecnicaEntities2 db = new UsuarioPruebaTecnicaEntities2())
            {
                users = (from d in db.cliente
                         select new ListaViewModel
                         {
                             id = d.id,
                             nombre = d.nombre,
                             apellido = d.apellido,
                             identificacion = d.identificacion,
                             celular = d.celular,
                             direccion = d.direccion,
                             ciudad = d.ciudad,
                             correo = d.correo
                         }).ToList();
            }
            return View(users);
        }

        public ActionResult Editar(int id)
        {
            DatosViewModel model = new DatosViewModel();
            using (UsuarioPruebaTecnicaEntities2 db = new UsuarioPruebaTecnicaEntities2())
            {
                var oTabla = db.cliente.Find(id);
                model.celular = oTabla.celular;
                model.direccion = oTabla.direccion;
                model.ciudad = oTabla.ciudad;

            }
            return View(model);
        }

        [HttpPost]

        public ActionResult Editar(DatosViewModel model)
        {
            using (UsuarioPruebaTecnicaEntities2 db = new UsuarioPruebaTecnicaEntities2())
            {
                var oTabla = db.cliente.Find(model.id);
                oTabla.celular = model.celular;
                oTabla.direccion = model.direccion;
                oTabla.ciudad = model.ciudad;

                db.Entry(oTabla).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return Redirect("~/Cliente/");
        }

        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            DatosViewModel model = new DatosViewModel();
            using (UsuarioPruebaTecnicaEntities2 db = new UsuarioPruebaTecnicaEntities2())
            {
                var oTabla = db.cliente.Find(id);
                db.cliente.Remove(oTabla);
                db.SaveChanges();

            }
            return Redirect("~/Cliente/");
        }

    }
}