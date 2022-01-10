using System;
using System.Collections.Generic;
using System.Text;

namespace CostTracing.Core.Models
{
    public class Currency : MetaData
    {
        private string _symbol = "";

        public string Symbol 
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
                Uri = $"/currency/{value}"; 
            }
        }

        public string Country { get; set; }

    }
}
