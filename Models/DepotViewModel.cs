using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PojectDepot.Models
{
    public class DepotViewModel
    {
        public List<Depot> Depots { get; set; } = new List<Depot>();

        public int Depot_Id { get; set; }
        public string Depot_code { get; set; }
        public string Depot_name { get; set; }

        [DisplayFormat(DataFormatString = "dd-mm-yyyy", ApplyFormatInEditMode = true)]
        public DateTime Valid_from { get; set; }

        [DisplayFormat(DataFormatString = "dd-mm-yyyy", ApplyFormatInEditMode = true)]
        public DateTime Valid_to { get; set; }
        public string Depot_type { get; set; }
        public string Status { get; set; }
        public string Depot_address { get; set; }
        public int Phone_no { get; set; }
    }
}