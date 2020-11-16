using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading.Tasks;
using SweatyBot.Infrastructure;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SweatyBot.Models;

namespace SweatyBot.Services
{
    public class BlacklistService : CustomService
    {
        private const String connectionString = "DefaultEndpointsProtocol=https;AccountName=sweaty;AccountKey=xcpmVcJmV+0y6ZAildxeeGqLtkDHTyDxwfRNx4VA6KN6p7+QDG2v0suoNfMnSSWtLKrRChzzcIWL8B/y0+1mrQ==;EndpointSuffix=core.windows.net";

        //Get current list of shitheads
        public async Task GetBlacklist()
        {
            var condition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "sweaty");
            var query = new TableQuery<BlacklistItem>().Where(condition);

            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("blacklist");

            TableContinuationToken token = null;
            var items = await table.ExecuteQuerySegmentedAsync(query, token);

            if (items.Results.Count > 0)
            {
                var emb = new EmbedBuilder();
                emb.WithTitle("Here is the current shitlist:");
                foreach (var item in items)
                {
                    emb.AddField(item.name + " " + item.rid, item.reason);
                }
                DiscordReply("", emb);
            }
            else
            {
                Log($"No items found", (int)LogType.Text);
            }
        }

        public async Task AddToBlacklist(String name, String rid, String reason)
        {
            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(rid))
            {
                Log("You need to specify a name and rid | !blacklistadd name rid [reason]", (int)LogType.Text);
                return;
            }

            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("blacklist");

            TableOperation retrieve = TableOperation.Retrieve<BlacklistItem>("sweaty", name);
            TableResult result = await table.ExecuteAsync(retrieve);

            if (result.Result != null)
            {
                Log($"Blacklist item {name} already exists.", (int)LogType.Text);
            }
            else
            {
                BlacklistItem item = new BlacklistItem(name)
                {
                    rid = rid,
                    reason = reason
                };

                TableOperation insertOperation = TableOperation.Insert(item);
                await table.ExecuteAsync(insertOperation);
                Log($"Blacklist item {name} added.", (int)LogType.Text);
            }
        }

        public async Task RemoveFromBlacklist(String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                Log("You need to specify a name.", (int)LogType.Text);
                return;
            }

            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("blacklist");

            TableOperation retrieve = TableOperation.Retrieve<BlacklistItem>("sweaty", name);
            TableResult result = await table.ExecuteAsync(retrieve);

            if (result.Result != null)
            {
                BlacklistItem item = (BlacklistItem)result.Result;
                TableOperation delete = TableOperation.Delete(item);
                await table.ExecuteAsync(delete);
                Log($"Blacklist item {name} deleted.", (int)LogType.Text);
            }
            else
            {
                Log($"Blacklist item {name} not found.", (int)LogType.Text);
            }
        }

        public async Task RemoveAll()
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("blacklist");

            var condition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "sweaty");
            var query = new TableQuery<BlacklistItem>().Where(condition);

            TableContinuationToken token = null;
            var items = await table.ExecuteQuerySegmentedAsync(query, token);

            foreach (var item in items)
            {
                TableOperation delete = TableOperation.Delete(item);
                await table.ExecuteAsync(delete);
            }

            Log($"Blacklist purged.", (int)LogType.Text);
        }

        public async Task UpdateBlacklist(String name, String rid, String reason)
        {
            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(rid))
            {
                Log("You need to specify a name and rid", (int)LogType.Text);
                return;
            }

            var account = CloudStorageAccount.Parse(connectionString);
            var client = account.CreateCloudTableClient();
            var table = client.GetTableReference("blacklist");

            TableOperation retrieve = TableOperation.Retrieve<BlacklistItem>("sweaty", name);
            TableResult result = await table.ExecuteAsync(retrieve);

            if (result != null)
            {
                BlacklistItem item = (BlacklistItem)result.Result;
                item.rid = rid;
                item.reason = reason;
                TableOperation update = TableOperation.Replace(item);
                await table.ExecuteAsync(update);
                Log($"Blacklist item {name} updated.", (int)LogType.Text);
            }
            else
            {
                Log($"Blacklist item {name} not found.", (int)LogType.Text);
            }
        }
    }
}
