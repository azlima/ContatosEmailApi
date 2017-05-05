using ContatosEmailApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace ContatosEmailApi.Controllers
{
    public class ContatosController : ApiController
    {
        private ContatosContext db = new ContatosContext();

        // GET: api/Contatos
        public IQueryable<Contato> GetContatos()
        {
            return db.Contatos;
        }

        // GET: api/Contatos/5
        [ResponseType(typeof(Contato))]
        public IHttpActionResult GetContato(int id)
        {
            Contato contato = db.Contatos.Find(id);
            if (contato == null)
            {
                return NotFound();
            }

            return Ok(contato);
        }

        // PUT: api/Contatos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContato(int id, Contato contato)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contato.Id)
            {
                return BadRequest();
            }

            db.Entry(contato).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContatoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Contatos
        [ResponseType(typeof(Contato))]
        public IHttpActionResult PostContato(Contato contato)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contatos.Add(contato);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = contato.Id }, contato);
        }

        // DELETE: api/Contatos/5
        [ResponseType(typeof(Contato))]
        public IHttpActionResult DeleteContato(int id)
        {
            Contato contato = db.Contatos.Find(id);
            if (contato == null)
            {
                return NotFound();
            }

            db.Contatos.Remove(contato);
            db.SaveChanges();

            return Ok(contato);
        }

        [HttpGet]
        [ActionName("arquivo")]
        public void SaveAndOpenTxt(int contatoId)
        {
            IList<string> linhas = new List<string>();

            Contato contato = db.Contatos.Find(contatoId);
            if (contato != null)
            {
                linhas.Add(string.Format("Para: {0}", contato.Para));

                if (!string.IsNullOrEmpty(contato.Copia))
                {
                    linhas.Add(string.Format("Cópia: {0}", contato.Copia));
                }

                if (!string.IsNullOrEmpty(contato.CopiaOculta))
                {
                    linhas.Add(string.Format("Cópia Oculta: {0}", contato.CopiaOculta));
                }

                linhas.Add(string.Format("Assunto: {0}", contato.Assunto));
                linhas.Add(string.Format("Mensagem: {0}", contato.Mensagem));

                string documentoDiretorio =
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                using (StreamWriter arquivoSaida = new StreamWriter(documentoDiretorio + @"\Email.txt"))
                {
                    foreach (string linha in linhas)
                    {
                        arquivoSaida.WriteLine(linha);
                    }
                    arquivoSaida.Dispose();
                    //Process.Start("notepad.exe", documentoDiretorio + @"\Email.txt");
                    Process.Start("notepad.exe", "Teste");
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContatoExists(int id)
        {
            return db.Contatos.Count(e => e.Id == id) > 0;
        }
    }
}