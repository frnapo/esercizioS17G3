﻿using Scarpe.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scarpe.Controllers
{
    public class HomeController : Controller
    {
        List<Prodotti> prodotti = new List<Prodotti>();
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Scarpe"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "SELECT * FROM Articoli WHERE Visibile = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Prodotti prodotto = new Prodotti();
                    prodotto.ID = Convert.ToInt32(reader["idArticolo"]);
                    prodotto.Nome = reader["Nome"].ToString();
                    prodotto.Prezzo = Convert.ToDecimal(reader["Prezzo"]);
                    prodotto.Immagine1 = reader["imgPath"].ToString();
                    prodotti.Add(prodotto);
                }

            }
            catch (Exception ex)
            {
                Response.Write("Error: ");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View(prodotti);
        }





        public ActionResult Dettagli(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Scarpe"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            Prodotti prodotto = new Prodotti();

            try
            {
                conn.Open();
                string query = "SELECT * FROM Articoli WHERE idArticolo = " + id;
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    prodotto.ID = Convert.ToInt32(reader["idArticolo"]);
                    prodotto.Nome = reader["Nome"].ToString();
                    prodotto.Descrizione = reader["Descrizione"].ToString();
                    prodotto.Prezzo = Convert.ToDecimal(reader["Prezzo"]);
                    prodotto.Immagine1 = reader["imgPath"].ToString();
                    prodotto.Immagine2 = reader["imgAlt1"].ToString();
                    prodotto.Immagine3 = reader["imgAlt2"].ToString();
                }

            }
            catch (Exception ex)
            {
                Response.Write("Error: ");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View(prodotto);
        }

        public ActionResult AggiungiArticolo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AggiungiArticolo(Prodotti prodotto, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Scarpe"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            if (file1 != null && file1.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file1.FileName);
                string _path = Path.Combine(Server.MapPath("~/Content/imgs/"), _FileName);
                file1.SaveAs(_path);
                prodotto.Immagine1 = _path;
            }
            if (file2 != null && file2.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file2.FileName);
                string _path = Path.Combine(Server.MapPath("~/Content/imgs/"), _FileName);
                file2.SaveAs(_path);
                prodotto.Immagine2 = _path;
            }
            if (file3 != null && file3.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file3.FileName);
                string _path = Path.Combine(Server.MapPath("~/Content/imgs/"), _FileName);
                file3.SaveAs(_path);
                prodotto.Immagine3 = _path;
            }
            try
            {
                conn.Open();
                string query = "INSERT INTO Articoli (Nome, Descrizione, Prezzo, imgPath, imgAlt1, imgAlt2, Visibile)" +
                    " VALUES (@Nome, @Descrizione, @Prezzo, @imgPath, @imgAlt1, @imgAlt2, @Visibile)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nome", prodotto.Nome);
                cmd.Parameters.AddWithValue("@Descrizione", prodotto.Descrizione);
                cmd.Parameters.AddWithValue("@Prezzo", prodotto.Prezzo);
                cmd.Parameters.AddWithValue("@imgPath", prodotto.Immagine1);
                cmd.Parameters.AddWithValue("@imgAlt1", prodotto.Immagine2);
                cmd.Parameters.AddWithValue("@imgAlt2", prodotto.Immagine3);
                cmd.Parameters.AddWithValue("@Visibile", prodotto.InVetrina);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Response.Write("Error: ");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return RedirectToAction("Index");
        }


        public ActionResult Gestione()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Scarpe"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT * FROM Articoli";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Prodotti prodotto = new Prodotti();
                    prodotto.ID = Convert.ToInt32(reader["idArticolo"]);
                    prodotto.Nome = reader["Nome"].ToString();
                    prodotto.Descrizione = reader["Descrizione"].ToString();
                    prodotto.Prezzo = Convert.ToDecimal(reader["Prezzo"]);
                    prodotto.Immagine1 = reader["imgPath"].ToString();
                    prodotto.Immagine2 = reader["imgAlt1"].ToString();
                    prodotto.Immagine3 = reader["imgAlt2"].ToString();
                    prodotto.InVetrina = Convert.ToBoolean(reader["Visibile"]);
                    prodotti.Add(prodotto);
                }

            }

            catch (Exception ex)
            {
                Response.Write("Error: ");
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return View(prodotti);
        }




        public ActionResult DeleteProd(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Scarpe"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "UPDATE Articoli SET Visibile = 0 WHERE idArticolo = " + id;
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return RedirectToAction("Gestione");
        }

        public ActionResult AddProd(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Scarpe"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "UPDATE Articoli SET Visibile = 1 WHERE idArticolo = " + id;
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return RedirectToAction("Gestione");
        }



        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Utenti utente)
        {
            List<Utenti> utenti = new List<Utenti>()
                {
                    new Utenti { ID = 1, Nome = "admin", Password = "admin", Admin = true },
                    new Utenti { ID = 2, Nome = "user", Password = "user", Admin = false }
                };

            var utenteLoggato = utenti.FirstOrDefault(u => u.Nome == utente.Nome && u.Password == utente.Password);

            if (utenteLoggato != null)
            {
                Session["UtenteLoggato"] = utenteLoggato;
                if (utenteLoggato.Admin)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Error = "Nome utente o password non validi";
                return View("Index");
            }
        }

        public ActionResult Logout()
        {
            Session["UtenteLoggato"] = null;
            return RedirectToAction("Index", "Home");
        }
    }


}
