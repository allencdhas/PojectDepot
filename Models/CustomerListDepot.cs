using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PojectDepot.Models
{
    public class CustomerListDepot
    {

        //Supplier
        public int SupplierDetails_Id { get; set; }
        public string Supplier_name { get; set; }
        public int Depot_Id { get; set; }
        public string Distancefrom_depot { get; set; }
        public string Area { get; set; }
        public string Supplier_location { get; set; }

        [DisplayFormat(DataFormatString = "dd-mm-yyyy", ApplyFormatInEditMode = true)]
        public DateTime Effective_from { get; set; }


        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();



        //Depot
        public string Depot_code { get; set; }
        public string Depot_name { get; set; }

        [DisplayFormat(DataFormatString = "dd-mm-yyyy", ApplyFormatInEditMode = true)]
        public DateTime? Valid_from { get; set; }

        [DisplayFormat(DataFormatString = "dd-mm-yyyy", ApplyFormatInEditMode = true)]
        public DateTime? Valid_to { get; set; }
        public string Depot_type { get; set; }
        public string Status { get; set; }
        public string Depot_address { get; set; }
        public string Phone_no { get; set; }
        public string flag { get; set; }

        public List<Depot> Depots { get; set; } = new List<Depot>();


        //pagination

        public double numRecords { get; set; }
        public double numPages { get; set; }
    }
}