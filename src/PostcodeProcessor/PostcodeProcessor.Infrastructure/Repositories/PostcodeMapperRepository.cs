using System;
using System.Data.SqlClient;
using Dapper;
using RightmovePostcodeToLocationId.PostcodeProcessor.Core.Domain;
using RightmovePostcodeToLocationId.PostcodeProcessor.Core.Enums;

namespace PostcodeProcessor.Infrastructure.Repositories
{
    public class PostcodeMapperRepository
    {
        private readonly string _connectionString;

        public PostcodeMapperRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PostcodeLocationMapper Get(string postcode)
        {
            const string selectQuery = "SELECT Postcode, ProcessingStatus FROM PostcodeLocationMapper WHERE Postcode = @Postcode;";
            using (var connection = new SqlConnection(_connectionString))
            {
                var existingPostcode = connection.QueryFirstOrDefault<PostcodeLocationMapper>(selectQuery,
                        new
                        {
                            Postcode = postcode
                        });
                return existingPostcode;
            }
        }

        public void Create(string postcode)
        {
            const string insertQuery = "INSERT INTO PostcodeLocationMapper VALUES (@Postcode, @LocationId, @ProcessingStatus);";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(insertQuery, new
                {
                    Postcode = postcode,
                    LocationId = "",
                    ProcessingStatus = Enum.GetName(typeof(ProcessingStatus),
                        ProcessingStatus.SearchForPostcode)
                });
            }
        }

        public void Update(string postcode, ProcessingStatus status)
        {
            const string updateQuery = "UPDATE PostcodeLocationMapper SET ProcessingStatus = @ProcessingStatus WHERE Postcode = @Postcode;";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(updateQuery, new
                {
                    Postcode = postcode,
                    ProcessingStatus = Enum.GetName(typeof(ProcessingStatus), status)
                });
            }
        }

        public void Update(string postcode, string locationId)
        {
            const string updateQueryLocationId = "UPDATE PostcodeLocationMapper " +
                                                 "SET ProcessingStatus = @ProcessingStatus, " +
                                                 "LocationId = @LocationId " +
                                                 "WHERE Postcode = @Postcode;";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(updateQueryLocationId, new
                {
                    Postcode = postcode,
                    LocationId = locationId,
                    ProcessingStatus = Enum.GetName(typeof(ProcessingStatus),
                        ProcessingStatus.LocationIdRetrieved)
                });
            }
        }
    }
}
