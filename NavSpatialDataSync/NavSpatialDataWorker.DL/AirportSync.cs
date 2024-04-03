using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialDataWorker.DL
{
    public class AirportSync
    {
        private readonly string sourceConnection;
        private readonly string destinationConnection;

        public AirportSync(string sourceConn, string destConn)
        {
            sourceConnection = sourceConn;
            destinationConnection = destConn;
        }

        public void SynchronizeAirports()
        {


            using (SqlConnection srcConn = new SqlConnection(sourceConnection) , destConn = new SqlConnection(destinationConnection))
            {
                srcConn.Open();
                destConn.Open();
                SqlTransaction transaction = destConn.BeginTransaction();
                try
                {
                    //AddNewAirports(srcConn, destConn, transaction);
                    //DeleteObsoleteAirports(srcConn, destConn, transaction); 
                    UpdateExistingAirports(srcConn, destConn, transaction);

                    transaction.Commit();
                    Console.WriteLine("Synchronization of Airports completed successfully.");
                    Console.WriteLine();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}. Rolling back transaction.");
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        Console.WriteLine($"An error occurred while trying to roll back the transaction: {exRollback.Message}");
                    }
                }
            }
        }

        private void AddNewAirports(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("SELECT src.CycleId, src.IcaoCode, src.AirportId, src.AirportName, src.AirportElevation," +
                                            " src.TransitionAltitude, src.TransitionLevel, src.IATADesignator, src.Latitude, src.Longitude, src.LongestRunway, src.TimeZone," +
                                            " src.FIRIdentifier, src.UIRIdentifier FROM Nav.airport src LEFT JOIN navspatialdata.nav.airports dest ON src.AirportId = dest.AirportId" +
                                            " WHERE dest.AirportId IS NULL", srcConn);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                SqlCommand insertCmd = new SqlCommand("INSERT INTO nav.airports (CycleId, IcaoCode, AirportId, AirportName," +
                                                     " AirportElevation, TransitionAltitude, TransitionLevel, IATADesignator, Latitude, Longitude, LongestRunway, TimeZone, FIRIdentifier, UIRIdentifier, SpatialData)" +
                                                     " VALUES (@CycleId, @IcaoCode, @AirportId, @AirportName, @AirportElevation, @TransitionAltitude," +
                                                     " @TransitionLevel, @IATADesignator, @Latitude, @Longitude, @LongestRunway, @TimeZone, @FIRIdentifier, @UIRIdentifier , geometry::Point(@Latitude, @Longitude, 4326))", destConn, transaction);

                int addedCount = 0;

                while (reader.Read())
                {
                    insertCmd.Parameters.Clear();
                    insertCmd.Parameters.AddWithValue("@CycleId", reader["CycleId"]);
                    insertCmd.Parameters.AddWithValue("@IcaoCode", reader.IsDBNull(reader.GetOrdinal("IcaoCode")) ? (object)DBNull.Value : reader["IcaoCode"]);
                    insertCmd.Parameters.AddWithValue("@AirportId", reader["AirportId"]);
                    insertCmd.Parameters.AddWithValue("@AirportName", reader.IsDBNull(reader.GetOrdinal("AirportName")) ? (object)DBNull.Value : reader["AirportName"]);
                    insertCmd.Parameters.AddWithValue("@AirportElevation", reader.IsDBNull(reader.GetOrdinal("AirportElevation")) ? (object)DBNull.Value : reader["AirportElevation"]);
                    insertCmd.Parameters.AddWithValue("@TransitionAltitude", reader.IsDBNull(reader.GetOrdinal("TransitionAltitude")) ? (object)DBNull.Value : reader["TransitionAltitude"]);
                    insertCmd.Parameters.AddWithValue("@TransitionLevel", reader.IsDBNull(reader.GetOrdinal("TransitionLevel")) ? (object)DBNull.Value : reader["TransitionLevel"]);
                    insertCmd.Parameters.AddWithValue("@IATADesignator", reader.IsDBNull(reader.GetOrdinal("IATADesignator")) ? (object)DBNull.Value : reader["IATADesignator"]);
                    insertCmd.Parameters.AddWithValue("@Latitude", reader["Latitude"]);
                    insertCmd.Parameters.AddWithValue("@Longitude", reader["Longitude"]);
                    insertCmd.Parameters.AddWithValue("@LongestRunway", reader.IsDBNull(reader.GetOrdinal("LongestRunway")) ? (object)DBNull.Value : reader["LongestRunway"]);
                    insertCmd.Parameters.AddWithValue("@TimeZone", reader.IsDBNull(reader.GetOrdinal("TimeZone")) ? (object)DBNull.Value : reader["TimeZone"]);
                    insertCmd.Parameters.AddWithValue("@FIRIdentifier", reader.IsDBNull(reader.GetOrdinal("FIRIdentifier")) ? (object)DBNull.Value : reader["FIRIdentifier"]);
                    insertCmd.Parameters.AddWithValue("@UIRIdentifier", reader.IsDBNull(reader.GetOrdinal("UIRIdentifier")) ? (object)DBNull.Value : reader["UIRIdentifier"]);

                    insertCmd.ExecuteNonQuery();
                    addedCount++;
                }
                Console.WriteLine($"{addedCount} new airports were added successfully.");


            }
        }

        private void DeleteObsoleteAirports(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            SqlCommand deleteCmd = new SqlCommand("DELETE dest FROM navspatialdata.nav.airports dest " +
                                          "LEFT JOIN NavDatas.Nav.airport src ON dest.AirportId = src.AirportId " +
                                          "WHERE src.AirportId IS NULL", destConn, transaction);

            int deletedCount = deleteCmd.ExecuteNonQuery();


            Console.WriteLine($"{deletedCount} airports were deleted successfully.");
        }


        private void UpdateExistingAirports(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            // This is a simplified example. You would need to expand this based on the fields you actually need to update.
            SqlCommand updateCmd = new SqlCommand("UPDATE dest SET dest.CycleId = src.CycleId, dest.AirportName = src.AirportName, " +
                                                  "dest.AirportElevation = src.AirportElevation, dest.TransitionAltitude = src.TransitionAltitude, " +
                                                  "dest.TransitionLevel = src.TransitionLevel, dest.IATADesignator = src.IATADesignator, " +
                                                  "dest.Latitude = src.Latitude, dest.Longitude = src.Longitude, dest.LongestRunway = src.LongestRunway, " +
                                                  "dest.TimeZone = src.TimeZone, dest.FIRIdentifier = src.FIRIdentifier, dest.UIRIdentifier = src.UIRIdentifier, " +
                                                  "dest.SpatialData = geometry::Point(src.Latitude, src.Longitude, 4326) " +
                                                  "FROM NavDatas.Nav.airport src JOIN NavSpatialData.nav.airports dest ON src.AirportId = dest.AirportId " +
                                                  "WHERE NOT (src.CycleId = dest.CycleId AND src.AirportName = dest.AirportName AND " +
                                                  "src.AirportElevation = dest.AirportElevation AND src.TransitionAltitude = dest.TransitionAltitude AND " +
                                                  "src.TransitionLevel = dest.TransitionLevel AND src.IATADesignator = dest.IATADesignator AND " +
                                                  "src.Latitude = dest.Latitude AND src.Longitude = dest.Longitude AND " +
                                                  "src.LongestRunway = dest.LongestRunway AND src.TimeZone = dest.TimeZone AND " +
                                                  "src.FIRIdentifier = dest.FIRIdentifier AND src.UIRIdentifier = dest.UIRIdentifier)", destConn, transaction);

            int updatedCount = updateCmd.ExecuteNonQuery();
            Console.WriteLine($"{updatedCount} airports were updated successfully.");
        }
    }
}
