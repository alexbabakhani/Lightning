//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CitiLightning_Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Portfolio
    {
        public int Id { get; set; }
        public Nullable<int> PortfolioID { get; set; }
        public string Symbol { get; set; }
        public Nullable<long> NumberOfShares { get; set; }
        public Nullable<decimal> PriceBought { get; set; }
        public Nullable<int> IsActive { get; set; }
    }
}
