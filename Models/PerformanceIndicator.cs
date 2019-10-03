using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Api.Models
{
    public class PerformanceIndicator
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Value { get; set; }
    }
}