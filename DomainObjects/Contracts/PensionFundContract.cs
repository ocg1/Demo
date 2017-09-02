﻿using Auctus.DomainObjects.Accounts;
using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Contracts
{
    public class PensionFundContract
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.UInt32)]
        public Int32 Id { get; set; }
        [DapperType(System.Data.DbType.AnsiStringFixedLength)]
        public String Address { get; set; }
        [DapperType(System.Data.DbType.AnsiStringFixedLength)]
        public String TransactionHash { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime CreationDate { get; set; }
        [DapperType(System.Data.DbType.UInt32)]
        public Int32 GasUsed { get; set; }
        [DapperType(System.Data.DbType.UInt32)]
        public Int32 BlockNumber { get; set; }
        [DapperType(System.Data.DbType.UInt32)]
        public Int32 SmartContractId { get; set; }
        [DapperType(System.Data.DbType.AnsiStringFixedLength)]
        public String PensionFundOptionAddress { get; set; }
        
        public List<PensionFundReferenceContract> PensionFundReferenceContract { get; set; }
        public string SmartContractCode { get; set; }
    }
}