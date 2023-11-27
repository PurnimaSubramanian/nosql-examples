/*-
 * Copyright (c) 2018, 2023 Oracle and/or its affiliates. All rights reserved.
 *
 * Licensed under the Universal Permissive License v 1.0 as shown at
 *  https://oss.oracle.com/licenses/upl/
 */

namespace Oracle.NoSQL.SDK.Samples
{
   using System;
   using System.Threading.Tasks;
   using Oracle.NoSQL.SDK;
   // -----------------------------------------------------------------------
   // Run the example as:
   //
   // dotnet run -f <target framework>
   //
   // where:
   //   - <target framework> is target framework moniker, supported values
   //     are netcoreapp5.1 and net7.0
   // -----------------------------------------------------------------------
   public class ModifyData
   {
      private const string Usage =
            "Usage: dotnet run -f <target framework> [-- <config file>]";
      private const string TableName = "stream_acct";
      private const string acct1= @"{
         ""acct_Id"": 1,
         ""profile_name"": ""AP"",
         ""account_expiry"": ""2023-10-18"",
         ""acct_data"": {
            ""firstName"": ""Adam"",
            ""lastName"": ""Phillips"",
            ""country"": ""Germany"",
            ""contentStreamed"": [{
               ""showName"": ""At the Ranch"",
               ""showId"": 26,
               ""showtype"": ""tvseries"",
               ""genres"": [""action"", ""crime"", ""spanish""],
               ""numSeasons"": 4,
               ""seriesInfo"": [{
                  ""seasonNum"": 1,
                  ""numEpisodes"": 2,
                  ""episodes"": [{
                     ""episodeID"": 20,
                     ""episodeName"": ""Season 1 episode 1"",
                     ""lengthMin"": 85,
                     ""minWatched"": 85,
                     ""date"": ""2022-04-18""
                  },
                  {
                     ""episodeID"": 30,
                     ""lengthMin"": 60,
                     ""episodeName"": ""Season 1 episode 2"",
                     ""minWatched"": 60,
                     ""date"": ""2022 - 04 - 18""
                  }]
               },
               {
                  ""seasonNum"": 2,
                  ""numEpisodes"": 2,
                  ""episodes"": [{
                  ""episodeID"": 40,
                     ""episodeName"": ""Season 2 episode 1"",
                     ""lengthMin"": 50,
                     ""minWatched"": 50,
                     ""date"": ""2022-04-25""
                  },
                  {
                     ""episodeID"": 50,
                     ""episodeName"": ""Season 2 episode 2"",
                     ""lengthMin"": 45,
                     ""minWatched"": 30,
                     ""date"": ""2022-04-27""
                  }]
               },
               {
                  ""seasonNum"": 3,
                  ""numEpisodes"": 2,
                  ""episodes"": [{
                     ""episodeID"": 60,
                     ""episodeName"": ""Season 3 episode 1"",
                     ""lengthMin"": 50,
                     ""minWatched"": 50,
                     ""date"": ""2022-04-25""
                  },
                  {
                     ""episodeID"": 70,
                     ""episodeName"": ""Season 3 episode 2"",
                     ""lengthMin"": 45,
                     ""minWatched"": 30,
                     ""date"": ""2022 - 04 - 27""
                  }]
               }]
            },
            {
               ""showName"": ""Bienvenu"",
               ""showId"": 15,
               ""showtype"": ""tvseries"",
               ""genres"": [""comedy"", ""french""],
               ""numSeasons"": 2,
               ""seriesInfo"": [{
                  ""seasonNum"": 1,
                  ""numEpisodes"": 2,
                  ""episodes"": [{
                     ""episodeID"": 20,
                     ""episodeName"": ""Bonjour"",
                     ""lengthMin"": 45,
                     ""minWatched"": 45,
                     ""date"": ""2022-03-07""
                  },
                  {
                     ""episodeID"": 30,
                     ""episodeName"": ""Merci"",
                     ""lengthMin"": 42,
                     ""minWatched"": 42,
                     ""date"": ""2022-03-08""
                  }]
               }]
            }]
         }
      }";
      private const string acct2= @"{
         ""acct_Id"":2,
        	""profile_name"":""Adwi"",
        	""account_expiry"":""2023-10-31"",
        	""acct_data"":
        	   {""firstName"": ""Adelaide"",
        	   ""lastName"": ""Willard"",
        	   ""country"": ""France"",
        	   ""contentStreamed"": [{
        	      ""showName"" : ""Bienvenu"",
        	      ""showId"" : 15,
        	      ""showtype"" : ""tvseries"",
        	      ""genres"" : [""comedy"", ""french""],
        	      ""numSeasons"" : 2,
        	      ""seriesInfo"": [ {
        	         ""seasonNum"" : 1,
        	         ""numEpisodes"" : 2,
        	         ""episodes"": [ {
        	            ""episodeID"": 22,
        					""episodeName"" : ""Season 1 episode 1"",
        	            ""lengthMin"": 65,
        	            ""minWatched"": 65,
        	            ""date"" : ""2022-04-18""
        	         },
        	         {
        	            ""episodeID"": 32,
        	            ""lengthMin"": 60,
        					""episodeName"" : ""Season 1 episode 2"",
        	            ""minWatched"": 60,
        	            ""date"" : ""2022-04-18""
        	         }]
        	      },
        	      {
        	         ""seasonNum"": 2,
        	         ""numEpisodes"" :3,
        	         ""episodes"": [{
        	            ""episodeID"": 42,
        					""episodeName"" : ""Season 2 episode 1"",
        	            ""lengthMin"": 50,
        	            ""minWatched"": 50,
        	            ""date"" : ""2022-04-25""
        	         }
        	      ]}
        	   ]}
        	]}
      }";
      private const string acct3= @"{
        	""acct_Id"":3,
        	""profile_name"":""Dee"",
        	""account_expiry"":""2023-11-28"",
        	""acct_data"":
        	   {""firstName"": ""Dierdre"",
        	   ""lastName"": ""Amador"",
        	   ""country"": ""USA"",
        	   ""contentStreamed"": [{
        	      ""showName"" : ""Bienvenu"",
        	      ""showId"" : 15,
        	      ""showtype"" : ""tvseries"",
        	      ""genres"" : [""comedy"", ""french""],
        	      ""numSeasons"" : 2,
        	      ""seriesInfo"": [ {
        	         ""seasonNum"" : 1,
        	         ""numEpisodes"" : 2,
        	         ""episodes"": [ {
        	            ""episodeID"": 23,
        					""episodeName"" : ""Season 1 episode 1"",
        	            ""lengthMin"": 45,
        	            ""minWatched"": 40,
        	            ""date"": ""2022-08-18""
        	         },
        	         {
        	            ""episodeID"": 33,
        	            ""lengthMin"": 60,
        					""episodeName"" : ""Season 1 episode 2"",
        	            ""minWatched"": 50,
        	            ""date"" : ""2022-08-19""
        	         }]
        	      },
        	      {
        	         ""seasonNum"": 2,
        	         ""numEpisodes"" : 3,
        	         ""episodes"": [{
        	            ""episodeID"": 43,
        					""episodeName"" : ""Season 2 episode 1"",
        	            ""lengthMin"": 50,
        	            ""minWatched"": 50,
        	            ""date"" : ""2022-08-25""
        	         },
        	         {
        	            ""episodeID"": 53,
        					""episodeName"" : ""Season 2 episode 2"",
        	            ""lengthMin"": 45,
        	            ""minWatched"": 30,
        	            ""date"" : ""2022-08-27""
        	         }
        	      ]}]
        	   },
        	   {
        	      ""showName"": ""Dane"",
        	      ""showId"": 16,
        	      ""showtype"": ""tvseries"",
        	      ""genres"" : [""comedy"", ""drama"",""danish""],
        	      ""numSeasons"" : 2,
        	      ""seriesInfo"": [
        	      {
        	         ""seasonNum"" : 1,
        	         ""numEpisodes"" : 2,
        	         ""episodes"": [
        	         {
        	            ""episodeID"": 24,
        					""episodeName"" : ""Bonjour"",
        	            ""lengthMin"": 45,
        	            ""minWatched"": 45,
        	            ""date"" : ""2022-06-07""
        	         },
        	         {
        	            ""episodeID"": 34,
        					""episodeName"" : ""Merci"",
        	            ""lengthMin"": 42,
        	            ""minWatched"": 42,
        	            ""date"" : ""2022-06-08""
        	         }
        	      ]
        	   }]
        	}]}
      }";
      private const string upsert_row = @"UPSERT INTO stream_acct VALUES
      (
         1,
         ""AP"",
         ""2023-10-18"",
         {
            ""firstName"": ""Adam"",
            ""lastName"": ""Phillips"",
            ""country"": ""Germany"",
            ""contentStreamed"": [{
               ""showName"": ""At the Ranch"",
               ""showId"": 26,
               ""showtype"": ""tvseries"",
               ""genres"": [""action"", ""crime"", ""spanish""],
               ""numSeasons"": 4,
               ""seriesInfo"": [{
                  ""seasonNum"": 1,
                  ""numEpisodes"": 2,
                  ""episodes"": [{
                     ""episodeID"": 20,
                     ""episodeName"": ""Season 1 episode 1"",
                     ""lengthMin"": 75,
                     ""minWatched"": 75,
                     ""date"": ""2022-04-18""
                  },
                  {
                     ""episodeID"": 30,
                     ""lengthMin"": 60,
                     ""episodeName"": ""Season 1 episode 2"",
                     ""minWatched"": 40,
                     ""date"": ""2022 - 04 - 18""
                  }]
               },
               {
                  ""seasonNum"": 2,
                  ""numEpisodes"": 2,
                  ""episodes"": [{
                     ""episodeID"": 40,
                     ""episodeName"": ""Season 2 episode 1"",
                     ""lengthMin"": 40,
                     ""minWatched"": 30,
                     ""date"": ""2022-04-25""
                  },
                  {
                     ""episodeID"": 50,
                     ""episodeName"": ""Season 2 episode 2"",
                     ""lengthMin"": 45,
                     ""minWatched"": 30,
                     ""date"": ""2022-04-27""
                  }]
               },
               {
                  ""seasonNum"": 3,
                  ""numEpisodes"": 2,
                  ""episodes"": [{
                     ""episodeID"": 60,
                     ""episodeName"": ""Season 3 episode 1"",
                     ""lengthMin"": 20,
                     ""minWatched"": 20,
                     ""date"": ""2022-04-25""
                  },
                  {
                     ""episodeID"": 70,
                     ""episodeName"": ""Season 3 episode 2"",
                     ""lengthMin"": 45,
                     ""minWatched"": 30,
                     ""date"": ""2022 - 04 - 27""
                  }]
               }]
            },
            {
               ""showName"": ""Bienvenu"",
               ""showId"": 15,
               ""showtype"": ""tvseries"",
               ""genres"": [""comedy"", ""french""],
               ""numSeasons"": 2,
               ""seriesInfo"": [{
                  ""seasonNum"": 1,
                  ""numEpisodes"": 2,
                  ""episodes"": [{
                     ""episodeID"": 20,
                     ""episodeName"": ""Bonjour"",
                     ""lengthMin"": 45,
                     ""minWatched"": 45,
                     ""date"": ""2022-03-07""
                  },
                  {
                     ""episodeID"": 30,
                     ""episodeName"": ""Merci"",
                     ""lengthMin"": 42,
                     ""minWatched"": 42,
                     ""date"": ""2022-03-08""
                  }]
               }]
            }]
         }
      ) RETURNING *";
      private const string stmt1 ="select * from stream_acct";
      private const string stmt2 ="select account_expiry, acct.acct_data.lastName, acct.acct_data.contentStreamed[].showName from stream_acct acct where acct_id=1";
      private const string updt_stmt = @"UPDATE stream_acct SET account_expiry =""2023-12-28T00:00:00.0Z"" WHERE acct_Id=3";

      private const string upd_json_addnode = @"UPDATE stream_acct acct1 ADD acct1.acct_data.contentStreamed.seriesInfo[1].episodes {
        ""date"" : ""2022-04-26"",
        ""episodeID"" : 43,
        ""episodeName"" : ""Season 2 episode 2"",
        ""lengthMin"" : 45,
        ""minWatched"" : 45} WHERE acct_Id=2 RETURNING *";

      private const string upd_json_delnode = "UPDATE stream_acct acct1 REMOVE acct1.acct_data.contentStreamed.seriesInfo[1].episodes[1] WHERE acct_Id=2 RETURNING *";
      private const string del_stmt = @"DELETE FROM stream_acct acct1 WHERE acct1.acct_data.firstName=""Adelaide"" AND acct1.acct_data.lastName=""Willard"" ";

      //Get a connection handle for Oracle NoSQL Database Cloud Service
      private async static Task<NoSQLClient> getconnection_cloud()
      {
         //replace the place holder for compartment with your region identifier and OCID of your compartment
         var client =  new NoSQLClient(new NoSQLConfig
         {
            Region = <your_region_identifier>,
            Compartment = "<your_compartment_ocid"
         });
         return client;
      }
      //Get a connection handle for onPremise data store
      private async static Task<NoSQLClient> getconnection_onPrem()
      {
         //replace the placeholder with your fullname of the host
         var client = new NoSQLClient(new NoSQLConfig
         {
            ServiceType = ServiceType.KVStore,
            Endpoint = "http://<hostname>:8080"
         });
         return client;
      }
      // Create a table
      private static async Task crtTabAddData(NoSQLClient client)
      {
         var sql =
                $@"CREATE TABLE IF NOT EXISTS {TableName}(acct_Id INTEGER,
                                                          profile_name STRING,
                                                          account_expiry TIMESTAMP(1),
                                                          acct_data JSON,
                                                          primary key(acct_Id))";

         Console.WriteLine("\nCreate table {0}", TableName);
         var tableResult = await client.ExecuteTableDDLAsync(sql,
            new TableDDLOptions
            {
               TableLimits = new TableLimits(20, 20, 1)
            });

         Console.WriteLine("  Creating table {0}", TableName);
         Console.WriteLine("  Table state: {0}", tableResult.TableState);
         // Wait for the operation completion
         await tableResult.WaitForCompletionAsync();
         Console.WriteLine("  Table {0} is created",
             tableResult.TableName);
         Console.WriteLine("  Table state: {0}", tableResult.TableState);
         // Write a record
         Console.WriteLine("\nInsert records");
         var putResult = await client.PutAsync(TableName, FieldValue.FromJsonString(acct1).AsMapValue);
         if (putResult.ConsumedCapacity != null)
         {
             Console.WriteLine("  Write used:");
             Console.WriteLine("  " + putResult.ConsumedCapacity);
         }
         var putResult1 = await client.PutAsync(TableName, FieldValue.FromJsonString(acct2).AsMapValue);
         if (putResult1.ConsumedCapacity != null)
         {
             Console.WriteLine("  Write used:");
             Console.WriteLine("  " + putResult1.ConsumedCapacity);
         }
         var putResult2 = await client.PutAsync(TableName, FieldValue.FromJsonString(acct3).AsMapValue);
         if (putResult2.ConsumedCapacity != null)
         {
             Console.WriteLine("  Write used:");
             Console.WriteLine("  " + putResult2.ConsumedCapacity);
         }
      }
      private static async Task upsertData(NoSQLClient client,String querystmt){
         var queryEnumerable = client.GetQueryAsyncEnumerable(querystmt);
         await DoQuery(queryEnumerable);
      }
      private static async Task updateData(NoSQLClient client,String querystmt){
         var queryEnumerable = client.GetQueryAsyncEnumerable(querystmt);
      }
      private static async Task delRow(NoSQLClient client){
         var primaryKey = new MapValue
           {
              ["acct_Id"] = 1
           };
           // Unconditional delete, should succeed.
           var deleteResult = await client.DeleteAsync(TableName, primaryKey);
          // Expected output: Delete succeeded.
          Console.WriteLine("Delete {0}.",deleteResult.Success ? "succeeded" : "failed");
      }
      private static async Task deleteRows(NoSQLClient client,String querystmt){
         var queryEnumerable = client.GetQueryAsyncEnumerable(querystmt);
      }
      //replace the place holder for compartment with the OCID of your compartment
      public static async Task Main(string[] args)
      {
         try {
            //if using cloud service uncomment the code below
            var client = await getconnection_cloud();
            //if using onPremise uncomment the code below
            //var client = await getconnection_onPrem();
            Console.WriteLine("Created NoSQLClient instance");
            await crtTabAddData(client);
            await upsertData(client,upsert_row);
            Console.WriteLine("Upsert data in table");
            await updateData(client,updt_stmt);
            Console.WriteLine("Data updated in the table");
            await updateData(client,upd_json_addnode);
            Console.WriteLine("New data node added in the table");
            await updateData(client,upd_json_delnode);
            Console.WriteLine("New Data node removed from the table");
            await delRow(client);
            Console.WriteLine("Row deleted based on primary key");
            await deleteRows(client,del_stmt);
            Console.WriteLine("Rows removedfrom the table");

         }
         catch (Exception ex) {
            Console.WriteLine("Exception has occurred:\n{0}: {1}",
            ex.GetType().FullName, ex.Message);
            Console.WriteLine("StackTrace is ");
            Console.WriteLine( ex.StackTrace);
            if (ex.InnerException != null)
            {
               Console.WriteLine("\nCaused by:\n{0}: {1}",
               ex.InnerException.GetType().FullName,
               ex.InnerException.Message);
            }
         }
      }
      private static async Task DoQuery(IAsyncEnumerable<QueryResult<RecordValue>> queryEnumerable){
         Console.WriteLine("  Query results:");
         await foreach (var result in queryEnumerable) {
            foreach (var row in result.Rows)
            {
               Console.WriteLine();
               Console.WriteLine(row.ToJsonString());
            }
         }
      }
   }
}
