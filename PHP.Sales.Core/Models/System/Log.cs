using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PHP.Sales.Core.Models.System
{
    public class Log : BaseEntity
    {
        //public int Id { get; set; }

        //public string EntityName { get; set; }

        //public string PropertyName { get; set; }

        //public string PrimaryKeyValue { get; set; }

        //public string OldValue { get; set; }

        //public string NewValue { get; set; }

        [Required(ErrorMessage = "A Time Stamp is required")]
        [DisplayName("Time")]
        public DateTime TimeStamp { get; set; }

        [Required(ErrorMessage = "A Product Name is required")]
        [DisplayName("Product ID")]
        public String ProductID { get; set; }

        [Required(ErrorMessage = "An update message is required")]
        [DisplayName("Message")]
        public String Message { get; set; }
    }
}
