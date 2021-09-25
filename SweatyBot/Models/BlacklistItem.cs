using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace SweatyBot.Models
{
    public class BlacklistItem : TableEntity
    {
        public BlacklistItem(string name)
        {
            this.name = name;
            this.PartitionKey = "sweaty";
            this.RowKey = name;
        }

        public BlacklistItem() { }

        public String name { get; set; }
        public String rid { get; set; }
        public String reason { get; set; }
    }

}
