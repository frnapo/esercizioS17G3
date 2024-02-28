using Scarpe.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Scarpe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Scarpe"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Prodotti> prodotti = new List<Prodotti>();
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
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}