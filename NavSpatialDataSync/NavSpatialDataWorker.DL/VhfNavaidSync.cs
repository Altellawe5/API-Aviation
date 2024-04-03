using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialDataWorker.DL
{
    public class VhfNavaidSync
    {
        private readonly string sourceConnection;
        private readonly string destinationConnection;

        public VhfNavaidSync(string sourceConn, string destConn)
        {
            sourceConnection = sourceConn;
            destinationConnection = destConn;
        }
        public void SynchronizeVhfNavaids()
        {

            using (SqlConnection srcConn = new SqlConnection(sourceConnection),
                   destConn = new SqlConnection(destinationConnection))
            {
                srcConn.Open();
                destConn.Open();
                SqlTransaction transaction = destConn.BeginTransaction();

                try
                {
                    AddNewVhfNavaids(srcConn, destConn, transaction);
                    DeleteObsoleteVhfNavaids(srcConn, destConn, transaction);
                    UpdateExistingVhfNavaids(srcConn, destConn, transaction);
                   

                    transaction.Commit();
                    Console.WriteLine("VhfNavaid synchronization completed successfully.");

                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred during VhfNavaid synchronization: {ex.Message}. Rolling back transaction.");
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
        private void DeleteObsoleteVhfNavaids(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            SqlCommand deleteCmd = new SqlCommand("DELETE dest FROM NavSpatialData.Nav.vhfnavaid dest " +
                                                  "LEFT JOIN Nav.VhfNavaid src ON dest.VorIdentifier = src.VorIdentifier AND dest.VorName = src.VorName " +
                                                  "WHERE src.VorIdentifier IS NULL AND src.VorName IS NULL", destConn, transaction);
            int deletedCount = deleteCmd.ExecuteNonQuery();
            Console.WriteLine($"{deletedCount} obsolete VhfNavaids were deleted successfully.");
        }
        private void AddNewVhfNavaids(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            SqlCommand cmd = new SqlCommand("SELECT src.CycleId, src.AreaCode, src.VorIdentifier, src.VorFrequency, src.VorLatitude, src.VorLongitude, src.VorName, src.FIRIdentifier, src.UIRIdentifier FROM Nav.VhfNavaid src " +
                                            "LEFT JOIN NavSpatialData.Nav.vhfnavaid dest ON src.VorIdentifier = dest.VorIdentifier AND src.VorName = dest.VorName " +
                                            "WHERE dest.VorIdentifier IS NULL AND dest.VorName IS NULL", srcConn);

            SqlCommand insertCmd = new SqlCommand("INSERT INTO Nav.VhfNavaid (CycleId, AreaCode, VorIdentifier, VorFrequency, VorLatitude, VorLongitude, VorName, FIRIdentifier, UIRIdentifier, SpatialData)" +
                                                  "VALUES (@CycleId, @AreaCode, @VorIdentifier, @VorFrequency, @VorLatitude, @VorLongitude, @VorName, @FIRIdentifier, @UIRIdentifier, " +
                                                  "CASE WHEN @VorLatitude IS NOT NULL AND @VorLongitude IS NOT NULL THEN geometry::Point(@VorLatitude, @VorLongitude, 4326) ELSE NULL END)", destConn, transaction);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                int vhfCount = 0;

                while (reader.Read())
                {
                    insertCmd.Parameters.Clear();
                    insertCmd.Parameters.AddWithValue("@CycleId", reader["CycleId"]);
                    insertCmd.Parameters.AddWithValue("@AreaCode", reader["AreaCode"] ?? DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@VorIdentifier", reader["VorIdentifier"]);
                    insertCmd.Parameters.AddWithValue("@VorFrequency", reader["VorFrequency"] ?? DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@VorLatitude", reader["VorLatitude"] ?? DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@VorLongitude", reader["VorLongitude"] ?? DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@VorName", reader["VorName"] ?? DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@FIRIdentifier", reader["FIRIdentifier"] ?? DBNull.Value);
                    insertCmd.Parameters.AddWithValue("@UIRIdentifier", reader["UIRIdentifier"] ?? DBNull.Value);

                    insertCmd.ExecuteNonQuery();
                    vhfCount++;
                }

                Console.WriteLine($"{vhfCount} new VhfNavaids were added.");
            }
        }

        private void UpdateExistingVhfNavaids(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            SqlCommand updateCmd = new SqlCommand("UPDATE dest SET dest.CycleId = src.CycleId, dest.AreaCode = src.AreaCode, " +
                                                  "dest.VorFrequency = src.VorFrequency, dest.VorLatitude = src.VorLatitude, " +
                                                  "dest.VorLongitude = src.VorLongitude, dest.VorName = src.VorName, " +
                                                  "dest.FIRIdentifier = src.FIRIdentifier, dest.UIRIdentifier = src.UIRIdentifier, " +
                                                  "dest.SpatialData = geometry::Point(src.VorLatitude, src.VorLongitude, 4326) " +
                                                  "FROM NavDatas.Nav.VhfNavaid src JOIN NavSpatialData.Nav.vhfnavaid dest " +
                                                  "ON src.VorIdentifier = dest.VorIdentifier " +
                                                  "WHERE NOT (src.CycleId = dest.CycleId AND src.AreaCode = dest.AreaCode AND " +
                                                  "src.VorFrequency = dest.VorFrequency AND src.VorLatitude = dest.VorLatitude AND " +
                                                  "src.VorLongitude = dest.VorLongitude AND src.VorName = dest.VorName AND " +
                                                  "src.FIRIdentifier = dest.FIRIdentifier AND src.UIRIdentifier = dest.UIRIdentifier AND " +
                                                  "dest.SpatialData.STEquals(geometry::Point(src.VorLatitude, src.VorLongitude, 4326)) = 1)", destConn, transaction);
            int updatedCount = updateCmd.ExecuteNonQuery();
            Console.WriteLine($"{updatedCount} VHF Navaids were updated successfully.");
        }

      


    }
}
