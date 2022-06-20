﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DashboardAPI.Models
{
    public class Orders
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public User OrderedBy { get; set; }
        // if still in cart TrackId will be zero
        public string TrackerID { get; set; } = string.Empty;
    }
}
