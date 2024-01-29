using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PojectDepot.Models
{
    public class Supplier
    {
        public int SupplierDetails_Id { get; set; }
        public string Supplier_name { get; set; }
        public int Depot_Id { get; set; }
        public string Distancefrom_depot { get; set; }
        public string Area { get; set; }
        public string Supplier_location { get; set; }

        [DisplayFormat(DataFormatString = "dd-mm-yyyy", ApplyFormatInEditMode = true)]
        public DateTime Effective_from { get; set; }

        public string flag { get; set; }
    }
}