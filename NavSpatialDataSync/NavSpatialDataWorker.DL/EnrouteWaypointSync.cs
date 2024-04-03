using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialDataWorker.DL
{
    public class EnrouteWaypointSync
    {
        private readonly string sourceConnection;
        private readonly string destinationConnection;

        public EnrouteWaypointSync(string sourceConn, string destConn)
        {
            sourceConnection = sourceConn;
            destinationConnection = destConn;
        }
        public void SynchronizeEnrouteWaypoints()
        {

            using (SqlConnection srcConn = new SqlConnection(sourceConnection),
                destConn = new SqlConnection(destinationConnection))
            {
                srcConn.Open();
                destConn.Open();

                SqlTransaction transaction = destConn.BeginTransaction();


                try
                {
                    AddNewEnrouteWaypoints(srcConn, destConn, transaction);
                    DeleteObsoleteEnrouteWaypoints(srcConn, destConn, transaction);
                    UpdateExistingEnrouteWaypoints(srcConn, destConn, transaction);
                    transaction.Commit();

                    Console.WriteLine("EnrouteWaypoint synchronization completed successfully.");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred during EnrouteWaypoint synchronization: {ex.Message}. Rolling back transaction.");
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

        private void DeleteObsoleteEnrouteWaypoints(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            SqlCommand deleteCmd = new SqlCommand("DELETE dest FROM NavSpatialData.Nav.EnrouteWaypoint dest " +
                                                  "LEFT JOIN Nav.EnrouteWaypoint src ON dest.WaypointId = src.WaypointId AND dest.IcaoCode = src.IcaoCode " +
                                                  "WHERE src.WaypointId IS NULL AND src.IcaoCode IS NULL", destConn, transaction);
            int deletedCount = deleteCmd.ExecuteNonQuery();
            Console.WriteLine($"{deletedCount} obsolete EnrouteWaypoints were deleted successfully.");
        }

        private void AddNewEnrouteWaypoints(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("SELECT src.CycleId, src.AreaCode, src.RegionCode, src.IcaoCode, src.WaypointId, " +
                                             "src.WaypointName, src.Latitude, src.Longitude, src.FIRIdentifier, src.UIRIdentifier FROM Nav.EnrouteWaypoint src " +
                                             "LEFT JOIN NavSpatialData.Nav.EnrouteWaypoint dest ON src.WaypointId = dest.WaypointId AND src.IcaoCode = dest.IcaoCode " +
                                             "WHERE dest.WaypointId IS NULL AND dest.IcaoCode IS NULL", srcConn);

            SqlCommand insertCmd = new SqlCommand("INSERT INTO NavSpatialData.Nav.EnrouteWaypoint (CycleId, AreaCode, RegionCode, IcaoCode, WaypointId, WaypointName, Latitude, Longitude, FIRIdentifier, UIRIdentifier, SpatialData)" +
                                                  "VALUES (@CycleId, @AreaCode, @RegionCode, @IcaoCode, @WaypointId, @WaypointName, @Latitude, @Longitude, @FIRIdentifier, @UIRIdentifier, " +
                                                  "CASE WHEN @Latitude IS NOT NULL AND @Longitude IS NOT NULL THEN geometry::Point(@Latitude, @Longitude, 4326) ELSE NULL END)", destConn, transaction);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                int wayPointCount = 0;

                while (reader.Read())
                {
                    insertCmd.Parameters.Clear();
                    insertCmd.Parameters.AddWithValue("@CycleId", reader["CycleId"]);
                    insertCmd.Parameters.AddWithValue("@AreaCode", reader["AreaCode"]);
                    insertCmd.Parameters.AddWithValue("@RegionCode", reader.IsDBNull(reader.GetOrdinal("RegionCode")) ? DBNull.Value : reader["RegionCode"]);
                    insertCmd.Parameters.AddWithValue("@IcaoCode", reader["IcaoCode"]);
                    insertCmd.Parameters.AddWithValue("@WaypointId", reader["WaypointId"]);
                    insertCmd.Parameters.AddWithValue("@WaypointName", reader.IsDBNull(reader.GetOrdinal("WaypointName")) ? DBNull.Value : reader["WaypointName"]);
                    insertCmd.Parameters.AddWithValue("@Latitude", reader["Latitude"]);
                    insertCmd.Parameters.AddWithValue("@Longitude", reader["Longitude"]);
                    insertCmd.Parameters.AddWithValue("@FIRIdentifier", reader.IsDBNull(reader.GetOrdinal("FIRIdentifier")) ? DBNull.Value : reader["FIRIdentifier"]);
                    insertCmd.Parameters.AddWithValue("@UIRIdentifier", reader.IsDBNull(reader.GetOrdinal("UIRIdentifier")) ? DBNull.Value : reader["UIRIdentifier"]);

                    insertCmd.ExecuteNonQuery();
                    wayPointCount++;
                }

                Console.WriteLine($"{wayPointCount} new EnrouteWaypoints were added.");
            }
        }

        private void UpdateExistingEnrouteWaypoints(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            SqlCommand updateCmd = new SqlCommand("UPDATE dest SET dest.CycleId = src.CycleId, dest.AreaCode = src.AreaCode, " +
                                                  "dest.RegionCode = src.RegionCode, dest.IcaoCode = src.IcaoCode, dest.WaypointName = src.WaypointName, " +
                                                  "dest.Latitude = src.Latitude, dest.Longitude = src.Longitude, dest.FIRIdentifier = src.FIRIdentifier, " +
                                                  "dest.UIRIdentifier = src.UIRIdentifier, dest.SpatialData = geometry::Point(src.Latitude, src.Longitude, 4326) " +
                                                  "FROM NavDatas.Nav.EnrouteWaypoint src JOIN NavSpatialData.Nav.EnrouteWaypoint dest " +
                                                  "ON src.WaypointId = dest.WaypointId AND src.IcaoCode = dest.IcaoCode " +
                                                  "WHERE NOT (src.CycleId = dest.CycleId AND src.AreaCode = dest.AreaCode AND " +
                                                  "src.RegionCode = dest.RegionCode AND src.IcaoCode = dest.IcaoCode AND " +
                                                  "src.WaypointName = dest.WaypointName AND src.Latitude = dest.Latitude AND " +
                                                  "src.Longitude = dest.Longitude AND src.FIRIdentifier = dest.FIRIdentifier AND " +
                                                  "src.UIRIdentifier = dest.UIRIdentifier AND " +
                                                  "dest.SpatialData.STEquals(geometry::Point(src.Latitude, src.Longitude, 4326)) = 1)", destConn, transaction);
            int updatedCount = updateCmd.ExecuteNonQuery();
            Console.WriteLine($"{updatedCount} enroute waypoints were updated successfully.");
        }

    }
}
