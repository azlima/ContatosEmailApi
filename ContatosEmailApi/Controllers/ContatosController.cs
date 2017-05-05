using ContatosEmailApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        //[Authorize]
        [ActionName("arquivo")]
        public HttpResponseMessage OpenFile(int contatoId)
        {
            HttpResponseMessage result = null;
            Contato contato = db.Contatos.Find(contatoId);
            if (contato != null)
            {
                string fullSavePath = HttpContext.Current.Server.MapPath("~/App_Data/Email.txt");
                IList<string> linhas = new List<string>();
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

                StringBuilder teste = new StringBuilder();
                foreach (var linha in linhas)
                {
                    teste.AppendLine(linha);
                }

                byte[] buffer = Encoding.ASCII.GetBytes(teste.ToString());
                MemoryStream ms = new MemoryStream(buffer);
                FileStream file = new FileStream(fullSavePath, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                file.Dispose();
                Process.Start("notepad.exe", fullSavePath);
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new FileStream(fullSavePath, FileMode.Open, FileAccess.Read));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = "Email.txt";

                //HttpResponseMessage arquivo = new HttpResponseMessage();
                //using (HttpClient httpClient = new HttpClient())
                //{
                //    WebClient clinete = new WebClient();
                //    clinete.DownloadData(fullSavePath);
                //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(teste.ToString())));
                //    arquivo = result;
                //}

                return result;
            }
            else
            {
                return result;
            }
        }


        //[HttpGet]
        //[ActionName("arquivo")]
        //public HttpResponseMessage SaveAndOpenTxt(int contatoId)
        //{
        //    IList<string> linhas = new List<string>();

        //    Contato contato = db.Contatos.Find(contatoId);
        //    //if (contato != null)
        //    //{
        //        linhas.Add(string.Format("Para: {0}", contato.Para));

        //        if (!string.IsNullOrEmpty(contato.Copia))
        //        {
        //            linhas.Add(string.Format("Cópia: {0}", contato.Copia));
        //        }

        //        if (!string.IsNullOrEmpty(contato.CopiaOculta))
        //        {
        //            linhas.Add(string.Format("Cópia Oculta: {0}", contato.CopiaOculta));
        //        }

        //        linhas.Add(string.Format("Assunto: {0}", contato.Assunto));
        //        linhas.Add(string.Format("Mensagem: {0}", contato.Mensagem));

        //        //string documentoDiretorio =
        //        //    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        //        string documentoDiretorio = HttpContext.Current.Server.MapPath("~/EmailFile/");

        //        //var txtBuilder = new StringBuilder();

        //        //foreach (string linha in linhas)
        //        //{
        //        //    txtBuilder.AppendLine(linha);
        //        //}

        //        //var txtContent = txtBuilder.ToString();
        //        //var txtStream = new MemoryStream(Encoding.UTF8.GetBytes(txtContent));

        //        //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        //        //result.Content = new StreamContent(txtStream);
        //        //result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain"); //text/plain
        //        //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //        //{
        //        //    FileName = documentoDiretorio + @"\Email.txt" // Not sure about that part. You can change to "text.txt" to try
        //        //};



        //        //using (FileStream file = new FileStream(documentoDiretorio + @"\Email.txt", FileMode.Create))
        //        //{
        //        //    byte[] bytes = new byte[file.Length];
        //        //    file.Read(bytes, 0, (int)file.Length);
        //        //    txtStream.Write(bytes, 0, (int)file.Length);
        //        //}

            
        //        //Process.Start("notepad.exe", documentoDiretorio + @"\Email.txt");
        //        //return result;

        //        using (StreamWriter arquivoSaida = new StreamWriter(documentoDiretorio + @"/Email.txt"))
        //        {
        //            foreach (string linha in linhas)
        //            {
        //                arquivoSaida.WriteLine(linha);
        //            }
        //            arquivoSaida.Dispose();
        //            //Process.Start("notepad.exe", documentoDiretorio + @"\Email.txt");
        //        }

        //        HttpResponseMessage result = null;
        //        var localFilePath = documentoDiretorio + @"/Email.txt"; // HttpContext.Current.Server.MapPath(documentoDiretorio + @"/Email.txt");

        //        if (!File.Exists(localFilePath))
        //        {
        //            result = Request.CreateResponse(HttpStatusCode.Gone);
        //        }
        //        else
        //        {
        //            // Serve the file to the client
        //            result = Request.CreateResponse(HttpStatusCode.OK);
        //            result.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
        //            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //            result.Content.Headers.ContentDisposition.FileName = "SampleImg";
        //        }

        //        //using (var client = new WebClient())
        //        //{
        //        //    var documentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        //    client.DownloadFile(localFilePath, documentos + @"\Email.txt");
        //        //}
        //        StringBuilder teste = new StringBuilder();
        //        foreach (var line in linhas)
        //        {
        //            teste.AppendLine(line);
        //        }

        //        byte[] buffer = Encoding.ASCII.GetBytes(teste.ToString());
        //        MemoryStream ms = new MemoryStream(buffer);
        //        var documentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //        //write to file
        //        FileStream file = new FileStream(documentos + @"\Email.txt", FileMode.Create, FileAccess.Write);
        //        ms.WriteTo(file);

        //        return result;

        //        //using (StreamWriter arquivoSaida = new StreamWriter(documentoDiretorio + @"\Email.txt"))
        //        //{
        //        //    foreach (string linha in linhas)
        //        //    {
        //        //        arquivoSaida.WriteLine(linha);
        //        //    }
        //        //    arquivoSaida.Dispose();
        //        //    //Process.Start("notepad.exe", documentoDiretorio + @"\Email.txt");
        //        //    Process.Start("notepad.exe", "Teste");
        //        //}
        //    //}
        //}

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