//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CitiLightningDatabase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transaction
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Strategy { get; set; }
        public System.DateTime Timestamp { get; set; }
        public string Symbol { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public Nullable<int> Size { get; set; }
        public Nullable<bool> Buy { get; set; }
    }
}
