using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PojectDepot.Models;
using System.Runtime.InteropServices;

namespace PojectDepot.Controllers
{
    public class DepotController : Controller
    {

        private SqlConnection con;
        // GET: Depot

        //landing pages
        public ActionResult Index()
        {
            return View();
        }



        //initial retrieve from db
        public ActionResult Depot(string value)
        {
            DepotViewModel result = new DepotViewModel();
            List<Depot> depots = new List<Depot>();
            if (value == null)
            {
                con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=sample;Integrated Security=True");
                con.Open();
                SqlCommand com = new SqlCommand("DisplayDepot", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Depot depot = new Depot();
                    depot.Depot_Id = (int)reader["Depot_Id"];
                    depot.Depot_code = (string)reader["Depot_code"];
                    depot.Depot_name = (string)reader["Depot_name"];
                    depot.Valid_from = (DateTime)reader["Valid_from"];
                    depot.Valid_to = (DateTime)reader["Valid_to"];
                    depot.Depot_type = (string)reader["Depot_type"];
                    depot.Status = (string)reader["Status"];
                    depot.Depot_address = (string)reader["Depot_address"];
                    depot.Phone_no = (string)reader["Phone_no"];

                    depots.Add(depot);

                }
                result.Depots = depots;
                Session["tempdata"] = depots;
                reader.Close();
            }
            else
            {
                result.Depots = Session["tempdata"] as List<Depot>;
            }



            return View(result);
        }


        //tempAdd DEpot
        public ActionResult addDepot(Depot d)
        {
            DepotViewModel result = new DepotViewModel();
            List<Depot> depots = new List<Depot>();
            List<Depot> newdepots = new List<Depot>();
            newdepots.Add(d);
            Session["newDepots"] = newdepots;

            var items = Session["tempdata"] as List<Depot>;
            items.Add(d);
            Session["tempdata"] = items;

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        //save to db

        int DepotId;
        public ActionResult saveDepot(Depot d)
        {
            
            Depot depot = d;

            con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=sample;Integrated Security=True");

            if (d.Depot_Id == 0)
            {
                

                SqlCommand com = new SqlCommand("CreateDepot", con);
                com.CommandType = CommandType.StoredProcedure;
                
                com.Parameters.AddWithValue("@Depot_code", depot.Depot_code);
                com.Parameters.AddWithValue("@Depot_name", depot.Depot_name);
                com.Parameters.AddWithValue("@Valid_from", depot.Valid_from);
                com.Parameters.AddWithValue("@Valid_to", depot.Valid_to);
                com.Parameters.AddWithValue("@Depot_type", depot.Depot_type);
                com.Parameters.AddWithValue("@Status", depot.Status);
                com.Parameters.AddWithValue("@Depot_address", depot.Depot_address);
                com.Parameters.AddWithValue("@Phone_no", depot.Phone_no);


                SqlParameter output = new SqlParameter("@Id", SqlDbType.Int);
                output.Direction = ParameterDirection.Output;
                com.Parameters.Add(output);

                con.Open();
                com.ExecuteNonQuery();
                con.Close();

                DepotId = (int)com.Parameters["@Id"].Value;
            }

            else
            {
                SqlCommand com1 = new SqlCommand("UpdateDepot", con);
                com1.CommandType = CommandType.StoredProcedure;

                com1.Parameters.AddWithValue("@Id", depot.Depot_Id);
                com1.Parameters.AddWithValue("@Depot_code", depot.Depot_code);
                com1.Parameters.AddWithValue("@Depot_name", depot.Depot_name);
                com1.Parameters.AddWithValue("@Valid_from", depot.Valid_from);
                com1.Parameters.AddWithValue("@Valid_to", depot.Valid_to);
                com1.Parameters.AddWithValue("@Depot_type", depot.Depot_type);
                com1.Parameters.AddWithValue("@Status", depot.Status);
                com1.Parameters.AddWithValue("@Depot_address", depot.Depot_address);
                com1.Parameters.AddWithValue("@Phone_no", depot.Phone_no);

                con.Open();
                com1.ExecuteNonQuery();
                con.Close();
                DepotId = depot.Depot_Id;
            }

            


            var items = Session["SupplierData"] as List<Supplier>;

            foreach (var item1 in items)
            {
                if (item1.flag == "I")
                {
                    con.Open();
                    SqlCommand com2 = new SqlCommand("AddSupplier", con);
                    com2.CommandType = CommandType.StoredProcedure;

                    com2.Parameters.AddWithValue("@Supplier_name", item1.Supplier_name);
                    com2.Parameters.AddWithValue("@Depot_Id", DepotId);
                    com2.Parameters.AddWithValue("@Distancefrom_depot", item1.Distancefrom_depot);
                    com2.Parameters.AddWithValue("@Area", item1.Area);
                    com2.Parameters.AddWithValue("@Supplier_location", item1.Supplier_location);
                    com2.Parameters.AddWithValue("@Effective_from", item1.Effective_from);

                    int i = com2.ExecuteNonQuery();
                    con.Close();
                }

                if (item1.flag == "U")
                {
                    con.Open();
                    SqlCommand comU = new SqlCommand("UpdateSupplier", con);
                    comU.CommandType = CommandType.StoredProcedure;

                    comU.Parameters.AddWithValue("@SupplierDetails_Id", item1.SupplierDetails_Id);
                    comU.Parameters.AddWithValue("@Supplier_name", item1.Supplier_name);
                    comU.Parameters.AddWithValue("@Distancefrom_depot", item1.Distancefrom_depot);
                    comU.Parameters.AddWithValue("@Area", item1.Area);
                    comU.Parameters.AddWithValue("@Supplier_location", item1.Supplier_location);
                    comU.Parameters.AddWithValue("@Effective_from", item1.Effective_from);

                    int i = comU.ExecuteNonQuery();
                    con.Close();
                }

                if (item1.flag == "D")
                {
                    con.Open();
                    SqlCommand comd = new SqlCommand("delSupplier", con);
                    comd.CommandType = CommandType.StoredProcedure;
                    comd.Parameters.AddWithValue("@Id", item1.SupplierDetails_Id);

                    int i2 = comd.ExecuteNonQuery();
                    con.Close();
                }
            }

            Session.Clear();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //temp flag assign D
        public void flagD(int id)
        {
            var items = Session["tempdata"] as List<Depot>;
            //var _id = from item in items
            //          where item.Depot_Id == id
            //          select item ;
            items.Where(a => a.Depot_Id == id).FirstOrDefault().flag = "D";
            Session["tempdata"] = items;
        }

        public ActionResult DeleteDb(int id)
        {
            con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=sample;Integrated Security=True");

            con.Open();
            SqlCommand comd = new SqlCommand("delDepot", con);
            comd.CommandType = CommandType.StoredProcedure;
            comd.Parameters.AddWithValue("@id", id);

            int iD = comd.ExecuteNonQuery();
            con.Close();

            con.Open();
            SqlCommand comd1 = new SqlCommand("delSupplier", con);
            comd1.CommandType = CommandType.StoredProcedure;
            comd1.Parameters.AddWithValue("@id", id);

            int iS = comd1.ExecuteNonQuery();
            con.Close();

            Session["tempdata"] = null;
            Session["SupplierData"] = null;

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}