using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavSpatialDataWorker.DL
{
    public class FirUirSync
    {
        private readonly string sourceConnection;
        private readonly string destinationConnection;

        public FirUirSync(string sourceConn, string destConn)
        {
            sourceConnection = sourceConn;
            destinationConnection = destConn;
        }

        public void SynchronizeFirUir()
        {


            using (SqlConnection srcConn = new SqlConnection(sourceConnection), destConn = new SqlConnection(destinationConnection))
            {
                srcConn.Open();
                destConn.Open();
                SqlTransaction transaction = destConn.BeginTransaction();
                try
                {
                    AddNewFirUirPoint(srcConn, destConn, transaction);
                    DeleteObsoleteFirUirPoint(srcConn, destConn, transaction);
                    UpdateExistingFirUirPoint(srcConn, destConn, transaction);

                    transaction.Commit();
                    Console.WriteLine("Synchronization of FirUir completed successfully.");
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

        private void AddNewFirUirPoint(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            string query = @"
                            INSERT INTO NavSpatialData.Nav.FirUir
                            (
                                CycleId, AreaCode, FirUirIdentifier, FirUirAddress, FirUirName,
                                FirUirIndicator, SequenceNumber, AdjacentFirIdentifier, AdjacentUirIdentifier,
                                ReportingUnitsSpeed, ReportingUnitsAltitude, BoundaryVia, EntryReport,
                                FirUirLatitude, FirUirLongitude, ArcOriginLatitude, ArcOriginLongitude,
                                ArcDistance, ArcBearing, FirUpperLimit, UirLowerLimit, UirUpperLimit,
                                CruiseTableInd, CycleDate
                            )
                            SELECT
                                CycleId, AreaCode, FirUirIdentifier, FirUirAddress, FirUirName,
                                FirUirIndicator, SequenceNumber, AdjacentFirIdentifier, AdjacentUirIdentifier,
                                ReportingUnitsSpeed, ReportingUnitsAltitude, BoundaryVia, EntryReport,
                                FirUirLatitude, FirUirLongitude, ArcOriginLatitude, ArcOriginLongitude,
                                ArcDistance, ArcBearing, FirUpperLimit, UirLowerLimit, UirUpperLimit,
                                CruiseTableInd, CycleDate
                            FROM NavDatas.Nav.FirUir src
                            WHERE NOT EXISTS (
                                SELECT 1
                                FROM NavSpatialData.Nav.FirUir dest
                                WHERE 
                                    dest.FirUirIdentifier = src.FirUirIdentifier AND
                                    dest.FirUirName = src.FirUirName AND
                                    dest.SequenceNumber = src.SequenceNumber
                            )";

            using (SqlCommand command = new SqlCommand(query, destConn, transaction))
            {
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} new FIR/UIR points were added.");
            }
        }


        private void UpdateExistingFirUirPoint(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            string query = @"
                            UPDATE dest
                            SET
                                dest.CycleId = src.CycleId,
                                dest.AreaCode = src.AreaCode,
                                dest.FirUirAddress = src.FirUirAddress,
                                dest.ReportingUnitsSpeed = src.ReportingUnitsSpeed,
                                dest.ReportingUnitsAltitude = src.ReportingUnitsAltitude,
                                dest.BoundaryVia = src.BoundaryVia,
                                dest.EntryReport = src.EntryReport,
                                dest.FirUirLatitude = src.FirUirLatitude,
                                dest.FirUirLongitude = src.FirUirLongitude,
                                dest.ArcOriginLatitude = src.ArcOriginLatitude,
                                dest.ArcOriginLongitude = src.ArcOriginLongitude,
                                dest.ArcDistance = src.ArcDistance,
                                dest.ArcBearing = src.ArcBearing,
                                dest.FirUpperLimit = src.FirUpperLimit,
                                dest.UirLowerLimit = src.UirLowerLimit,
                                dest.UirUpperLimit = src.UirUpperLimit,
                                dest.CruiseTableInd = src.CruiseTableInd,
                                dest.CycleDate = src.CycleDate,
                                dest.AdjacentFirIdentifier = src.AdjacentFirIdentifier,
                                dest.AdjacentUirIdentifier = src.AdjacentUirIdentifier
                            FROM NavDatas.Nav.FirUir AS src
                            JOIN NavSpatialData.Nav.FirUir AS dest ON 
                                src.FirUirIdentifier = dest.FirUirIdentifier AND
                                src.FirUirName = dest.FirUirName AND
                                src.SequenceNumber = dest.SequenceNumber
                            WHERE
                                (dest.CycleId <> src.CycleId OR
                                dest.AreaCode <> src.AreaCode OR
                                dest.FirUirAddress <> src.FirUirAddress OR
                                dest.ReportingUnitsSpeed <> src.ReportingUnitsSpeed OR
                                dest.ReportingUnitsAltitude <> src.ReportingUnitsAltitude OR
                                dest.BoundaryVia <> src.BoundaryVia OR
                                dest.EntryReport <> src.EntryReport OR
                                dest.FirUirLatitude <> src.FirUirLatitude OR
                                dest.FirUirLongitude <> src.FirUirLongitude OR
                                dest.ArcOriginLatitude <> src.ArcOriginLatitude OR
                                dest.ArcOriginLongitude <> src.ArcOriginLongitude OR
                                dest.ArcDistance <> src.ArcDistance OR
                                dest.ArcBearing <> src.ArcBearing OR
                                dest.FirUpperLimit <> src.FirUpperLimit OR
                                dest.UirLowerLimit <> src.UirLowerLimit OR
                                dest.UirUpperLimit <> src.UirUpperLimit OR
                                dest.CruiseTableInd <> src.CruiseTableInd OR
                                dest.CycleDate <> src.CycleDate OR
                                dest.AdjacentFirIdentifier <> src.AdjacentFirIdentifier OR
                                dest.AdjacentUirIdentifier <> src.AdjacentUirIdentifier)";

            using (SqlCommand command = new SqlCommand(query, destConn, transaction))
            {
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} FIR/UIR points were updated.");
            }
        }


        private void DeleteObsoleteFirUirPoint(SqlConnection srcConn, SqlConnection destConn, SqlTransaction transaction)
        {
            string query = @"
                            DELETE dest 
                            FROM NavSpatialData.Nav.FirUir AS dest
                            WHERE NOT EXISTS (
                                SELECT 1 
                                FROM NavDatas.Nav.FirUir AS src 
                                WHERE 
                                    src.FirUirIdentifier = dest.FirUirIdentifier AND 
                                    src.FirUirName = dest.FirUirName AND 
                                    src.SequenceNumber = dest.SequenceNumber
                            )";

            using (SqlCommand command = new SqlCommand(query, destConn, transaction))
            {
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} FIR/UIR points were deleted from the destination.");
            }
        }

    }
}
