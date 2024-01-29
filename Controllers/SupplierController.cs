using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PojectDepot.Models;
using Newtonsoft.Json.Linq;
using System.Collections;
using PagedList;
using Microsoft.Ajax.Utilities;
using System.Web.Helpers;

namespace PojectDepot.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier

        CustomerListDepot result = new CustomerListDepot();
        private SqlConnection con;

        //initial and edit db retrieval hit
        public ActionResult SupplierList(string value = null, int? id = null, int? page = null)
        {
            


            List<Supplier> data = new List<Supplier>();
            List<Depot> depots = new List<Depot>();
            int? iD = id;

            if ((value == null && id != null))
            {
                con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=sample;Integrated Security=True");
                con.Open();
                SqlCommand com = new SqlCommand("DisplaySupplier", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Supplier supplier = new Supplier();
                    supplier.SupplierDetails_Id = (int)reader["SupplierDetails_Id"];
                    supplier.Supplier_name = (string)reader["Supplier_name"];
                    supplier.Depot_Id = (int)reader["Depot_Id"];
                    supplier.Distancefrom_depot = (string)reader["Distancefrom_depot"];
                    supplier.Area = (string)reader["Area"];
                    supplier.Supplier_location = (string)reader["Supplier_location"];
                    supplier.Effective_from = (DateTime)reader["Effective_from"];

                    data.Add(supplier);
                    result.numRecords += 1;

                }

                result.numPages = Math.Ceiling(result.numRecords / 5);
                ViewBag.numPages = result.Suppliers.Count;

                result.Suppliers = data;
                Session["SupplierData"] = data;
                Session["SupplierDB"] = data;
                con.Close();

                con.Open();
                SqlCommand com1 = new SqlCommand("DisplayIDDepot", con);
                com1.CommandType = CommandType.StoredProcedure;
                com1.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader1 = com1.ExecuteReader();
                while (reader1.Read())
                {
                    Depot depot = new Depot();
                    result.Depot_Id = (int)reader1["Depot_Id"];  
                    result.Depot_code = (string)reader1["Depot_code"];
                    result.Depot_name = (string)reader1["Depot_name"];
                    result.Valid_from = (DateTime)reader1["Valid_from"];
                    result.Valid_to = (DateTime)reader1["Valid_to"];
                    result.Depot_type = (string)reader1["Depot_type"];
                    result.Status = (string)reader1["Status"];
                    result.Depot_address = (string)reader1["Depot_address"];
                    result.Phone_no = (string)reader1["Phone_no"];

                    depots.Add(depot);
                    Session["EditDepotVal"] = result;
                    
                }

                

               /// result.flag = "U";
                con.Close();
            }
            else if((value == null && id == null) && page == null)
            {
                Session["SupplierData"] = null;
            }
            else
            {
                var items = Session["SupplierData"] as List<Supplier>;
                if(items != null)
                {
                    result.Suppliers = Session["SupplierData"] as List<Supplier>;
                }
            }
            Depot tempdata = Session["tempDepot"] as Depot;
            if (tempdata != null)
            {
                result.Depot_Id = tempdata.Depot_Id;
                result.Depot_code = tempdata.Depot_code;
                result.Depot_name = tempdata.Depot_name;
                result.Valid_from = tempdata.Valid_from;
                result.Valid_to = tempdata.Valid_to;
                result.Depot_type = tempdata.Depot_type;
                result.Status = tempdata.Status;
                result.Depot_address = tempdata.Depot_address;
                result.Phone_no = tempdata.Phone_no;
            }
            Session["tempDepot"] = null;

            if (page != null)
            {
                var pagedata = Pagination(page);
                Session["SupplierPage"] = pagedata;
                //result.Suppliers = pagedata;
            }
            //var count = Session["SupplierData"] as List<Supplier>;
            //ViewBag.numPages = Math.Ceiling(Convert.ToDouble(count.Count) / 5);

            return View(result);
        }


        //temp add supplier
        public ActionResult addSupplier(Supplier s, Depot d)
            {
            CustomerListDepot result = new CustomerListDepot();
            List<Supplier> depots = new List<Supplier>();
            s.flag = "I";
            ////depots.Add(d);
            
            if (Session["SupplierData"] != null)
            {
                
                var items = Session["SupplierData"] as List<Supplier>;
                items.Add(s);
                Session["SupplierData"] = items;
            }
            else
            {
                var items = new List<Supplier>();
                items.Add(s);
                Session["SupplierData"] = items;
            }

            Session["tempDepot"] = d;
            
            return Json(true, JsonRequestBehavior.AllowGet);
        }


/*        public void CreateSupplier1(Supplier s)
        {

            con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=sample;Integrated Security=True");

            SqlCommand com = new SqlCommand("AddSupplier", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@SupplierDetails_Id", s.SupplierDetails_Id);
            com.Parameters.AddWithValue("@Supplier_name", s.Supplier_name);
            com.Parameters.AddWithValue("@Depot_Id", s.Depot_Id);
            com.Parameters.AddWithValue("@Distancefrom_depot", s.Distancefrom_depot);
            com.Parameters.AddWithValue("@Area", s.Area);
            com.Parameters.AddWithValue("@Supplier_location", s.Supplier_location);
            com.Parameters.AddWithValue("@Effective_from", s.Effective_from);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();

            ViewBag.msg = " Customer Added!";
        }*/


        public void flagD(int id)
        {
            var items = Session["SupplierData"] as List<Supplier>;

            items.Where(a => a.SupplierDetails_Id == id).FirstOrDefault().flag = "D";
            Session["SupplierData"] = items;
        }

        [HttpPost]
        public ActionResult flagU(int id, Supplier s, Depot d)
        {
            var items = Session["SupplierData"] as List<Supplier>;

            foreach (var item in items.Where(a => a.SupplierDetails_Id == id))
            {
                item.Supplier_name = s.Supplier_name;
                item.Distancefrom_depot = s.Distancefrom_depot;
                item.Area = s.Area;
                item.Supplier_location = s.Supplier_location;
                item.Effective_from = s.Effective_from;
                item.flag = "U";
            }
            //items.Where(a => a.SupplierDetails_Id == id).FirstOrDefault().flag = "I";
            Session["SupplierData"] = items;
            Session["tempDepot"] = d;


            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /*public ActionResult flagD(int id, Supplier s)
        {
            var items = Session["SupplierData"] as List<Supplier>;

            foreach (var item in items.Where(a => a.SupplierDetails_Id == id))
            {
                item.Supplier_name = s.Supplier_name;
                item.Depot_Id = s.Depot_Id;
                item.Distancefrom_depot = s.Distancefrom_depot;
                item.Area = s.Area;
                item.Supplier_location = s.Supplier_location;
                item.Effective_from = s.Effective_from;
                item.flag = "D";
            }
            //items.Where(a => a.SupplierDetails_Id == id).FirstOrDefault().flag = "I";
            Session["SupplierData"] = items;


            return Json(true, JsonRequestBehavior.AllowGet);
        }*/


        public ActionResult Pagination(int? page=null)
        {
            var lower = Convert.ToInt32(((page - 1)*5));
            var upper = Convert.ToInt32(((page)*5)-2);
            var data = Session["SupplierData"] as List<Supplier>;
            var datapage1 = new List<Supplier>();

            for(int i=lower; i<(5+lower); i++)
            {
                    datapage1.Add(data[i]);              
            }
            result.Suppliers = datapage1;
            //datapage1 = data.Slice(lower, 5);

            return View("Suppliertable", result);
        }


    }  
}