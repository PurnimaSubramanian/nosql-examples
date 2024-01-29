# Copyright (c) 2023, 2024 Oracle and/or its affiliates.
# Licensed under the Universal Permissive License v 1.0 as shown at
# https://oss.oracle.com/licenses/upl/
import os
from borneo import (Regions, NoSQLHandle, NoSQLHandleConfig, PutRequest,QueryRequest,
                    TableRequest, DeleteRequest, GetRequest, TableLimits, State)
from borneo.iam import SignatureProvider
from borneo.kv import StoreAccessTokenProvider

# Given a region, and compartment, instantiate a connection to the
# cloud service and return it
def get_connection_cloud():
   #replace the placeholder with your region identifier
   region = '<your_region_identifier>'
   provider = SignatureProvider()
   #If using the DEFAULT profile with the config file in default location
   # ~/.oci/config
   config = NoSQLHandleConfig(region, provider)
   #replace the placeholder with the ocid of your compartment
   config.set_default_compartment("<ocid_of_your_compartment>")
   return(NoSQLHandle(config))

# Given a endpoint, instantiate a connection to onPremise Oracle NoSQL database
def get_connection_onprem():
   # replace the placeholder with the name of your host
   kvstore_endpoint ='http://<hostname>:8080'
   provider = StoreAccessTokenProvider()
   # If using a secure store, uncomment the line below and pass the username,
   # password of the store to StoreAccessTokenProvider
   # provider = StoreAccessTokenProvider(username, password)
   return NoSQLHandle(NoSQLHandleConfig(kvstore_endpoint, provider))

# Create a table and set the table limits
def create_table(handle):
   statement = '''create table if not exists stream_acct (acct_Id INTEGER,
                                                           profile_name STRING,
                                                           account_expiry TIMESTAMP(1),
                                                           acct_data JSON,
                                                           primary key(acct_Id))'''
   request = TableRequest().set_statement(statement).set_table_limits(TableLimits(20, 10, 1))
   # Create the table, waiting for a total of 40000 milliseconds
   # and polling the service every 3000 milliseconds to see if table is active
   table_result = handle.do_table_request(request, 40000, 3000)
   table_result.wait_for_completion(handle, 40000, 3000)
   if (table_result.get_state() == State.ACTIVE):
      print('Created table: stream_acct')
   else:
      raise NameError('Table stream_acct is in an unexpected state ' + str(table_result.get_state()))

# Add a row of data to the table
def insert_record(handle,table_name,acct_data):
   request = PutRequest().set_table_name(table_name).set_value_from_json(acct_data)
   handle.put(request)
   print('Loaded a row into table: stream_acct')

# upsert data
def upsert_data(handle,sqlstmt):
   request = QueryRequest().set_statement(sqlstmt)
   result = handle.query(request)
   print('Upsert data')
   for r in result.get_results():
      print('\t' + str(r))

# update data
def update_data(handle,sqlstmt):
   request = QueryRequest().set_statement(sqlstmt)
   result = handle.query(request)
   print('Data Updated in table: stream_acct')

# delete row with a primary KEY
def del_row(handle,table_name):
   request = DeleteRequest().set_key({'acct_Id': 1}).set_table_name(table_name)
   result = handle.delete(request)
   print('Deleted data from table: stream_acct')

#delete row(s) with a filter condition
def delete_rows(handle,sqlstmt):
   request = QueryRequest().set_statement(sqlstmt)
   result = handle.query(request)
   print('Deleted data from table: stream_acct')

def main():
   handle = None
   acct1='''
   {
   "acct_Id":1,
   "profile_name":"AP",
   "account_expiry":"2023-10-18",
   "acct_data":{
      "firstName":"Adam",
      "lastName":"Phillips",
      "country":"Germany",
      "contentStreamed":[
         {
            "showName":"At the Ranch",
            "showId":26,
            "showtype":"tvseries",
            "genres":[
               "action",
               "crime",
               "spanish"
            ],
            "numSeasons":4,
            "seriesInfo":[
               {
                  "seasonNum":1,
                  "numEpisodes":2,
                  "episodes":[
                     {
                        "episodeID":20,
                        "episodeName":"Season 1 episode 1",
                        "lengthMin":85,
                        "minWatched":85,
                        "date":"2022-04-18"
                     },
                     {
                        "episodeID":30,
                        "lengthMin":60,
                        "episodeName":"Season 1 episode 2",
                        "minWatched":60,
                        "date":"2022 - 04 - 18 "
                     }
                  ]
               },
               {
                  "seasonNum":2,
                  "numEpisodes":2,
                  "episodes":[
                     {
                        "episodeID":40,
                        "episodeName":"Season 2 episode 1",
                        "lengthMin":50,
                        "minWatched":50,
                        "date":"2022-04-25"
                     },
                     {
                        "episodeID":50,
                        "episodeName":"Season 2 episode 2",
                        "lengthMin":45,
                        "minWatched":30,
                        "date":"2022-04-27"
                     }
                  ]
               },
               {
                  "seasonNum":3,
                  "numEpisodes":2,
                  "episodes":[
                     {
                        "episodeID":60,
                        "episodeName":"Season 3 episode 1",
                        "lengthMin":50,
                        "minWatched":50,
                        "date":"2022-04-25"
                     },
                     {
                        "episodeID":70,
                        "episodeName":"Season 3 episode 2",
                        "lengthMin":45,
                        "minWatched":30,
                        "date":"2022 - 04 - 27 "
                     }
                  ]
               }
            ]
         },
         {
            "showName":"Bienvenu",
            "showId":15,
            "showtype":"tvseries",
            "genres":[
               "comedy",
               "french"
            ],
            "numSeasons":2,
            "seriesInfo":[
               {
                  "seasonNum":1,
                  "numEpisodes":2,
                  "episodes":[
                     {
                        "episodeID":20,
                        "episodeName":"Bonjour",
                        "lengthMin":45,
                        "minWatched":45,
                        "date":"2022-03-07"
                     },
                     {
                        "episodeID":30,
                        "episodeName":"Merci",
                        "lengthMin":42,
                        "minWatched":42,
                        "date":"2022-03-08"
                     }
                  ]
               }
            ]
         }
      ]
   }}'''

   acct2='''
   {
   "acct_Id":2,
   "profile_name":"Adwi",
   "account_expiry":"2023-10-31",
   "acct_data":{
      "firstName":"Adelaide",
      "lastName":"Willard",
      "country":"France",
      "contentStreamed":[
         {
            "showName":"Bienvenu",
            "showId":15,
            "showtype":"tvseries",
            "genres":[
               "comedy",
               "french"
            ],
            "numSeasons":2,
            "seriesInfo":[
               {
                  "seasonNum":1,
                  "numEpisodes":2,
                  "episodes":[
                     {
                        "episodeID":22,
                        "episodeName":"Season 1 episode 1",
                        "lengthMin":65,
                        "minWatched":65,
                        "date":"2022-04-18"
                     },
                     {
                        "episodeID":32,
                        "lengthMin":60,
                        "episodeName":"Season 1 episode 2",
                        "minWatched":60,
                        "date":"2022-04-18"
                     }
                  ]
               },
               {
                  "seasonNum":2,
                  "numEpisodes":3,
                  "episodes":[
                     {
                        "episodeID":42,
                        "episodeName":"Season 2 episode 1",
                        "lengthMin":50,
                        "minWatched":50,
                        "date":"2022-04-25"
                     }
                  ]
               }
            ]
         }
      ]
   }}'''

   acct3='''
   {
   "acct_Id":3,
   "profile_name":"Dee",
   "account_expiry":"2023-11-28",
   "acct_data":{
      "firstName":"Dierdre",
      "lastName":"Amador",
      "country":"USA",
      "contentStreamed":[
         {
            "showName":"Bienvenu",
            "showId":15,
            "showtype":"tvseries",
            "genres":[
               "comedy",
               "french"
            ],
            "numSeasons":2,
            "seriesInfo":[
               {
                  "seasonNum":1,
                  "numEpisodes":2,
                  "episodes":[
                     {
                        "episodeID":23,
                        "episodeName":"Season 1 episode 1",
                        "lengthMin":45,
                        "minWatched":40,
                        "date":"2022-08-18"
                     },
                     {
                        "episodeID":33,
                        "lengthMin":60,
                        "episodeName":"Season 1 episode 2",
                        "minWatched":50,
                        "date":"2022-08-19"
                     }
                  ]
               },
               {
                  "seasonNum":2,
                  "numEpisodes":3,
                  "episodes":[
                     {
                        "episodeID":43,
                        "episodeName":"Season 2 episode 1",
                        "lengthMin":50,
                        "minWatched":50,
                        "date":"2022-08-25"
                     },
                     {
                        "episodeID":53,
                        "episodeName":"Season 2 episode 2",
                        "lengthMin":45,
                        "minWatched":30,
                        "date":"2022-08-27"
                     }
                  ]
               }
            ]
         },
         {
            "showName":"Dane",
            "showId":16,
            "showtype":"tvseries",
            "genres":[
               "comedy",
               "drama",
               "danish"
            ],
            "numSeasons":2,
            "seriesInfo":[
               {
                  "seasonNum":1,
                  "numEpisodes":2,
                  "episodes":[
                     {
                        "episodeID":24,
                        "episodeName":"Bonjour",
                        "lengthMin":45,
                        "minWatched":45,
                        "date":"2022-06-07"
                     },
                     {
                        "episodeID":34,
                        "episodeName":"Merci",
                        "lengthMin":42,
                        "minWatched":42,
                        "date":"2022-06-08"
                     }
                  ]
               }
            ]
         }
      ]
   }}'''
   upsert_row = '''UPSERT INTO stream_acct VALUES
   (
      1,
      "AP",
      "2023-10-18",
      {
         "firstName": "Adam",
         "lastName": "Phillips",
         "country": "Germany",
         "contentStreamed": [{
            "showName": "At the Ranch",
            "showId": 26,
            "showtype": "tvseries",
            "genres": ["action", "crime", "spanish"],
            "numSeasons": 4,
            "seriesInfo": [{
               "seasonNum": 1,
               "numEpisodes": 2,
               "episodes": [{
                  "episodeID": 20,
                  "episodeName": "Season 1 episode 1",
                  "lengthMin": 75,
                  "minWatched": 75,
                  "date": "2022-04-18"
               },
               {
                  "episodeID": 30,
                  "lengthMin": 60,
                  "episodeName": "Season 1 episode 2",
                  "minWatched": 40,
                  "date": "2022 - 04 - 18 "
               }]
            },
            {
               "seasonNum": 2,
               "numEpisodes": 2,
               "episodes": [{
                  "episodeID": 40,
                  "episodeName": "Season 2 episode 1",
                  "lengthMin": 40,
                  "minWatched": 30,
                  "date": "2022-04-25"
               },
               {
                  "episodeID": 50,
                  "episodeName": "Season 2 episode 2",
                  "lengthMin": 45,
                  "minWatched": 30,
                  "date": "2022-04-27"
               }]
            },
            {
               "seasonNum": 3,
               "numEpisodes": 2,
               "episodes": [{
                  "episodeID": 60,
                  "episodeName": "Season 3 episode 1",
                  "lengthMin": 20,
                  "minWatched": 20,
                  "date": "2022-04-25"
               },
               {
                  "episodeID": 70,
                  "episodeName": "Season 3 episode 2",
                  "lengthMin": 45,
                  "minWatched": 30,
                  "date": "2022 - 04 - 27 "
               }]
            }]
         },
         {
            "showName": "Bienvenu",
            "showId": 15,
            "showtype": "tvseries",
            "genres": ["comedy", "french"],
            "numSeasons": 2,
            "seriesInfo": [{
               "seasonNum": 1,
               "numEpisodes": 2,
               "episodes": [{
                  "episodeID": 20,
                  "episodeName": "Bonjour",
                  "lengthMin": 45,
                  "minWatched": 45,
                  "date": "2022-03-07"
               },
               {
                  "episodeID": 30,
                  "episodeName": "Merci",
                  "lengthMin": 42,
                  "minWatched": 42,
                  "date": "2022-03-08"
               }]
            }]
         }]
      }
   ) RETURNING * '''
   # if cloud service uncomment this
   handle = get_connection_cloud()
   # if onPremise uncomment this
   # handle = get_connection_onprem()
   create_table(handle)
   insert_record(handle,'stream_acct',acct1)
   insert_record(handle,'stream_acct',acct2)
   insert_record(handle,'stream_acct',acct3)
   upsert_data(handle,upsert_row)
   # update non-JSON data
   upd_stmt ='''UPDATE stream_acct SET account_expiry="2023-12-28T00:00:00.0Z" WHERE acct_Id=3'''
   update_data(handle,upd_stmt)
   # update JSON data and add a node
   upd_json_addnode = '''UPDATE stream_acct acct1 ADD acct1.acct_data.contentStreamed.seriesInfo[1].episodes {
     "date" : "2022-04-26",
     "episodeID" : 43,
     "episodeName" : "Season 2 episode 2",
     "lengthMin" : 45,
     "minWatched" : 45} WHERE acct_Id=2 RETURNING *'''
   update_data(handle,upd_json_addnode)
   # update JSON data and delete a node
   upd_json_delnode = '''UPDATE stream_acct acct1 REMOVE acct1.acct_data.contentStreamed.seriesInfo[1].episodes[1] WHERE acct_Id=2 RETURNING *'''
   update_data(handle,upd_json_delnode)
   # delete row based on primary key
   del_row(handle,'stream_acct')
   # delete data based on a filter condition
   del_stmt ='''DELETE FROM stream_acct acct1 WHERE acct1.acct_data.firstName="Adelaide" AND acct1.acct_data.lastName="Willard"'''
   delete_rows(handle,del_stmt)
   if handle is not None:
      handle.close()
   os._exit(0)

if __name__ == "__main__":
    main()
