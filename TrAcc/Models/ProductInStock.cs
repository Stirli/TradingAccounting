﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrAcc.Models
{
    public class ProductInStock
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime Expires { get; set; }
    }
}