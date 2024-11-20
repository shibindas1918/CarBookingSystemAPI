using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Principal;

namespace CarBookingSystemAPI.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
         public string Address { get; set; }    
    }
}
