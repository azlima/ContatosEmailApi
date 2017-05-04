using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ContatosEmailApi.Models;
using System.Net.Mail;

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
            else
            {
                SaveTxt();
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

        private void SaveTxt()
        {
            string[] lines = { "First line", "Second line", "Third line" };
            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllLines(@"C:\WriteLines.txt", lines);


            // Example #2: Write one string to a text file.
            string text = "A class is the most powerful data type in C#. Like a structure, " +
                           "a class defines the data and behavior of the data type. ";
            // WriteAllText creates a file, writes the specified string to the file,
            // and then closes the file.    You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllText(@"C:\WriteText.txt", text);

            // Example #3: Write only some strings in an array to a file.
            // The using statement automatically flushes AND CLOSES the stream and calls 
            // IDisposable.Dispose on the stream object.
            // NOTE: do not use FileStream for text files because it writes bytes, but StreamWriter
            // encodes the output as text.
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\WriteLines2.txt"))
            {
                foreach (string line in lines)
                {
                    // If the line doesn't contain the word 'Second', write the line to the file.
                    if (!line.Contains("Second"))
                    {
                        file.WriteLine(line);
                    }
                }
            }

            // Example #4: Append new text to an existing file.
            // The using statement automatically flushes AND CLOSES the stream and calls 
            // IDisposable.Dispose on the stream object.
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\WriteLines2.txt", true))
            {
                file.WriteLine("Fourth line");
            }
        }

        private void sendEmailViaWebApi()
        {
            string subject = "Email Subject";
            string body = "Email body";
            string FromMail = "shahid@reckonbits.com.pk";
            string emailTo = "reciever@reckonbits.com.pk";
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("mail.terra.com.pk");

            mail.From = new MailAddress(FromMail);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.Body = body;

            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential("shahid@reckonbits.com.pk", "your password");
            SmtpServer.EnableSsl = false;
            SmtpServer.Send(mail);
        }

        [ResponseType(typeof(void))]
        public IHttpActionResult SendEmailToContato(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Contato contato = db.Contatos.Find(id);

            if (id != contato.Id)
            {
                return BadRequest();
            }

            if (contato != null)
            {
                try
                {
                    string subject = contato.Assunto;
                    string body = contato.Mensagem;
                    string FromMail = "no_reply@apphb.com";
                    string emailTo = contato.Para;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.poa.terra.com.br");

                    mail.From = new MailAddress(FromMail);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;

                    SmtpServer.Port = 465;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("gilbertoluis@terra.com.br", "g9m8p5s4");
                    SmtpServer.EnableSsl = false;
                    SmtpServer.Send(mail);
                }
                catch
                {
                    throw;
                }
                
            }
            else
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
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