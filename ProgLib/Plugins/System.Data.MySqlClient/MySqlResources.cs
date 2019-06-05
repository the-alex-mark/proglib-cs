using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.MySqlClient
{
    /// <summary>
    /// Содержит список сообщений исключений.
    /// </summary>
    public class MySqlResources
    {
        public static String BadVersionFormat
        {
            get { return "Version string not in acceptable format"; }
        }

        public static String NamedPipeNoSeek
        {
            get { return "NamedPipeStream does not support seeking"; }
        }

        public static String StreamAlreadyClosed
        {
            get { return "The stream has already been closed"; }
        }

        public static String BufferCannotBeNull
        {
            get { return " The buffer cannot be null"; }
        }

        public static String BufferNotLargeEnough
        {
            get { return " Buffer is not large enough"; }
        }

        public static String OffsetCannotBeNegative
        {
            get { return " Offset cannot be negative"; }
        }

        public static String CountCannotBeNegative
        {
            get { return " Count cannot be negative"; }
        }

        public static String StreamNoRead
        {
            get { return " The stream does not support reading"; }
        }

        public static String NamedPipeNoSetLength
        {
            get { return "NamedPipeStream doesn't support SetLength"; }
        }

        public static String StreamNoWrite
        {
            get { return "The stream does not support writing"; }
        }

        public static String ErrorCreatingSocket
        {
            get { return "Error creating socket connection"; }
        }

        public static String SocketNoSeek
        {
            get { return "Socket streams do not support seeking"; }
        }

        public static String UnixSocketsNotSupported
        {
            get { return "Unix sockets are not supported on Windows"; }
        }

        public static String OffsetMustBeValid
        {
            get { return "Offset must be a valid position in buffer"; }
        }

        public static String CSNoSetLength
        {
            get { return "SetLength is not a valid operation on CompressedStream"; }
        }

        public static String FromIndexMustBeValid
        {
            get { return "From index must be a valid index inside the from buffer"; }
        }

        public static String FromAndLengthTooBig
        {
            get { return "From index and length use more bytes than from contains"; }
        }

        public static String IndexMustBeValid
        {
            get { return "Index must be a valid position in the buffer"; }
        }

        public static String IndexAndLengthTooBig
        {
            get { return "Index and length use more bytes than to has room for"; }
        }

        public static String PasswordMustHaveLegalChars
        {
            get { return "Password must be valid and contain length characters"; }
        }

        public static String ParameterCannotBeNegative
        {
            get { return "Parameter cannot have a negative value"; }
        }

        public static String ConnectionMustBeOpen
        {
            get { return "Connection must be valid and open"; }
        }

        public static String DataReaderOpen
        {
            get { return "There is already an open DataReader associated with this Connection which must be closed first."; }
        }

        public static String SPNotSupported
        {
            get { return "Stored procedures are not supported on this version of MySQL"; }
        }

        public static String ConnectionNotSet
        {
            get { return "The connection property has not been set or is null."; }
        }

        public static String ConnectionNotOpen
        {
            get { return "The connection is not open."; }
        }

        public static String AdapterIsNull
        {
            get { return "Improper MySqlCommandBuilder state: adapter is null"; }
        }

        public static String AdapterSelectIsNull
        {
            get { return "Improper MySqlCommandBuilder state: adapter's SelectCommand is null"; }
        }

        public static String CBMultiTableNotSupported
        {
            get { return "MySqlCommandBuilder does not support multi-table statements"; }
        }

        public static String CBNoKeyColumn
        {
            get { return "MySqlCommandBuilder cannot operate on tables with no unique or key columns"; }
        }

        public static String ParameterCannotBeNull
        {
            get { return "Parameter cannot be null"; }
        }

        public static String ChaosNotSupported
        {
            get { return "Chaos isolation level is not supported"; }
        }

        public static String ParameterIsInvalid
        {
            get { return "Parameter is invalid."; }
        }

        public static String ConnectionAlreadyOpen
        {
            get { return "The connection is already open."; }
        }

        public static String KeywordNotSupported
        {
            get { return "Keyword not supported."; }
        }

        public static String WriteToStreamFailed
        {
            get { return "Writing to the stream failed."; }
        }

        public static String ReadFromStreamFailed
        {
            get { return "Reading from the stream has failed."; }
        }

        public static String QueryTooLarge
        {
            get { return "Packets larger than max_allowed_packet are not allowed."; }
        }

        public static String UnableToExecuteSP
        {
            get { return "Unable to execute stored procedure '{0}'."; }
        }

        public static String ProcAndFuncSameName
        {
            get { return "same name are not supported."; }
        }

        public static String KeywordNoNull
        {
            get { return "Keyword does not allow null values."; }
        }

        public static String ImproperValueFormat
        {
            get { return "Value has an unsupported format."; }
        }

        public static String InvalidProcName
        {
            get { return "Procedure or function '{0}' cannot be found in database '{1}'."; }
        }

        public static String HardProcQuery
        {
            get { return "Retrieving procedure metadata for {0} from server."; }
        }

        public static String SoftProcQuery
        {
            get { return "Retrieving procedure metadata for {0} from procedure cache."; }
        }

        public static String ConnectionBroken
        {
            get { return "Connection unexpectedly terminated."; }
        }

        public static String IncorrectTransmission
        {
            get { return "An incorrect response was received from the server."; }
        }

        public static String CancelNotSupported
        {
            get { return "Canceling an active query is only supported on MySQL 5.0.0 and above. "; }
        }

        public static String Timeout
        {
            get { return "Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding."; }
        }

        public static String CancelNeeds50
        {
            get { return "Canceling an executing query requires MySQL 5.0 or higher."; }
        }

        public static String NoNestedTransactions
        {
            get { return "Nested transactions are not supported."; }
        }

        public static String CommandTextNotInitialized
        {
            get { return "The CommandText property has not been properly initialized."; }
        }

        public static String UnableToParseFK
        {
            get { return "There was an error parsing the foreign key definition."; }
        }

        public static String PerfMonCategoryHelp
        {
            get { return "This category includes a series of counters for MySQL."; }
        }

        public static String PerfMonCategoryName
        {
            get { return ".NET Data Provider for MySQL"; }
        }

        public static String PerfMonHardProcHelp
        {
            get { return "The number of times a procedures metadata had to be queried from the server."; }
        }

        public static String PerfMonHardProcName
        {
            get { return "Hard Procedure Queries"; }
        }

        public static String PerfMonSoftProcHelp
        {
            get { return "The number of times a procedures metadata was retrieved from the client-side cache."; }
        }

        public static String PerfMonSoftProcName
        {
            get { return "Soft Procedure Queries"; }
        }

        public static String WrongParameterName
        {
            get { return "Parameter '{0}' is not found but a parameter with the name '{1}' is found. Parameter names must include the leading parameter marker."; }
        }

        public static String UnableToConnectToHost
        {
            get { return "Unable to connect to any of the specified MySQL hosts."; }
        }

        public static String UnableToRetrieveParameters
        {
            get { return "Unable to retrieve stored procedure metadata for routine '{0}'.  Either grant  SELECT privilege to mysql.proc for this user or use check parameters=false with  your connection string."; }
        }

        public static String NextResultIsClosed
        {
            get { return "Invalid attempt to call NextResult when the reader is closed."; }
        }

        public static String NoBodiesAndTypeNotSet
        {
            get { return "When calling stored procedures and 'Use Procedure Bodies' is false, all parameters must have their type explicitly set."; }
        }

        public static String TimeoutGettingConnection
        {
            get { return "error connecting: Timeout expired.  The timeout period elapsed prior to obtaining a connection from the pool.  This may have occurred because all pooled connections were in use and max pool size was reached."; }
        }

        public static String ParameterAlreadyDefined
        {
            get { return "Parameter '{0}' has already been defined."; }
        }

        public static String ParameterMustBeDefined
        {
            get { return "Parameter '{0}' must be defined."; }
        }

        public static String ObjectDisposed
        {
            get { return "The object is not open or has been disposed."; }
        }

        public static String MultipleConnectionsInTransactionNotSupported
        {
            get { return "Multiple simultaneous connections or connections with different connection strings inside the same transaction are not currently supported."; }
        }

        public static String DistributedTxnNotSupported
        {
            get { return "MySQL Connector/Net does not currently support distributed transactions."; }
        }

        public static String FatalErrorDuringExecute
        {
            get { return "Fatal error encountered during command execution."; }
        }

        public static String FatalErrorDuringRead
        {
            get { return "Fatal error encountered during data read."; }
        }

        public static String FatalErrorReadingResult
        {
            get { return "Fatal error encountered attempting to read the resultset."; }
        }

        public static String RoutineNotFound
        {
            get { return "Routine '{0}' cannot be found. Either check the spelling or make sure you have sufficient rights to execute the routine."; }
        }

        public static String ParameterNotFoundDuringPrepare
        {
            get { return "Parameter '{0}' was not found during prepare."; }
        }

        public static String ValueNotSupportedForGuid
        {
            get { return "The requested column value could not be treated as or conveted to a Guid."; }
        }

        public static String UnableToDeriveParameters
        {
            get { return "Unable to derive stored routine parameters.  The 'Parameters' information schema table is not available and access to the stored procedure body has been disabled."; }
        }

        public static String DefaultEncodingNotFound
        {
            get { return "The default connection encoding was not found. Please report this as a bug along with your connection string and system details."; }
        }

        public static String GetHostEntryFailed
        {
            get { return "Call to GetHostEntry failed after {0} while querying for hostname '{1}': SocketErrorCode={2}, ErrorCode={3}, NativeErrorCode={4}."; }
        }

        public static String UnableToEnumerateUDF
        {
            get { return "An error occured attempting to enumerate the user-defined functions.  Do you have SELECT privileges on the mysql.func table?"; }
        }

        public static String DataNotInSupportedFormat
        {
            get { return "The given value was not in a supported format."; }
        }

        public static String NoServerSSLSupport
        {
            get { return "The host {0} does not support SSL connections."; }
        }

        public static String CouldNotFindColumnName
        {
            get { return "Could not find specified column in results: {0}"; }
        }

        public static String InvalidColumnOrdinal
        {
            get { return "You have specified an invalid column ordinal."; }
        }

        public static String ReadingPriorColumnUsingSeqAccess
        {
            get { return "Invalid attempt to read a prior column using SequentialAccess"; }
        }

        public static String AttemptToAccessBeforeRead
        {
            get { return "Invalid attempt to access a field before calling Read()"; }
        }

        public static String UnableToStartSecondAsyncOp
        {
            get { return "Unable to start a second async operation while one is running."; }
        }

        public static String MoreThanOneOPRow
        {
            get { return "INTERNAL ERROR:  More than one output parameter row detected."; }
        }

        public static String InvalidValueForBoolean
        {
            get { return "'{0}' is an illegal value for a boolean option."; }
        }

        public static String ServerTooOld
        {
            get { return "Connector/Net no longer supports server versions prior to 5.0"; }
        }

        public static String InvalidConnectionStringValue
        {
            get { return "The requested value '{0}' is invalid for the given keyword '{1}'."; }
        }

        public static String TraceCloseConnection
        {
            get { return "{0}: Connection Closed"; }
        }

        public static String TraceOpenConnection
        {
            get { return "{0}: Connection Opened: connection string = '{1}'"; }
        }

        public static String TraceQueryOpened
        {
            get { return "{0}: Query Opened: {2}"; }
        }

        public static String TraceResult
        {
            get { return "{0}: Resultset Opened: field(s) = {1}, affected rows = {2}, inserted id = {3}"; }
        }

        public static String TraceQueryDone
        {
            get { return "{0}: Query Closed"; }
        }

        public static String TraceSetDatabase
        {
            get { return "{0}: Set Database: {1}"; }
        }

        public static String TraceUAWarningBadIndex
        {
            get { return "{0}: Usage Advisor Warning: Query is using a bad index"; }
        }

        public static String TraceUAWarningNoIndex
        {
            get { return "{0}: Usage Advisor Warning: Query does not use an index"; }
        }

        public static String TraceResultClosed
        {
            get { return "{0}: Resultset Closed. Total rows={1}, skipped rows={2}, size (bytes)={3}"; }
        }

        public static String TraceUAWarningSkippedRows
        {
            get { return "{0}: Usage Advisor Warning: Skipped {2} rows. Consider a more focused query."; }
        }

        public static String TraceUAWarningSkippedColumns
        {
            get { return "{0}: Usage Advisor Warning: The following columns were not accessed: {2}"; }
        }

        public static String TraceUAWarningFieldConversion
        {
            get { return "{0}: Usage Advisor Warning: The field '{2}' was converted to the following types: {3}"; }
        }

        public static String TraceOpenResultError
        {
            get { return "{0}: Error encountered attempting to open result: Number={1}, Message={2}"; }
        }

        public static String TraceFetchError
        {
            get { return "{0}: Error encountered during row fetch. Number = {1}, Message={2}"; }
        }

        public static String TraceWarning
        {
            get { return "{0}: MySql Warning: Level={1}, Code={2}, Message={3}"; }
        }

        public static String TraceErrorMoreThanMaxValueConnections
        {
            get { return "Unable to trace.  There are more than Int32.MaxValue connections in use."; }
        }

        public static String TraceStatementPrepared
        {
            get { return "{0}: Statement prepared: sql='{1}', statement id={2}"; }
        }

        public static String TraceStatementClosed
        {
            get { return "{0}: Statement closed: statement id = {1}"; }
        }

        public static String TraceStatementExecuted
        {
            get { return "{0}: Statement executed: statement id = {1}"; }
        }

        public static String UnableToEnableQueryAnalysis
        {
            get { return "Unable to enable query analysis.  Be sure the MySql.Data.EMTrace assembly is properly located and registered."; }
        }

        public static String TraceQueryNormalized
        {
            get { return "{0}: Query Normalized: {2}"; }
        }

        public static String NoWindowsIdentity
        {
            get { return "Cannot retrieve Windows identity for current user. Connections that use  IntegratedSecurity cannot be  pooled. Use either 'ConnectionReset=true' or  'Pooling=false' in the connection string to fix."; }
        }

        public static String RoutineRequiresReturnParameter
        {
            get { return "Attempt to call stored function '{0}' without specifying a return parameter"; }
        }

        public static String CanNotDeriveParametersForTextCommands
        {
            get { return "Parameters can only be derived for commands using the StoredProcedure command type."; }
        }

        public static String ReplicatedConnectionsAllowOnlyReadonlyStatements
        {
            get { return "Replicated connections allow only readonly statements."; }
        }

        public static String FileBasedCertificateNotSupported
        {
            get { return "File based certificates are only supported when connecting to MySQL Server 5.1 or greater."; }
        }

        public static String SnapshotNotSupported
        {
            get { return "Snapshot isolation level is not supported."; }
        }

        public static String TypeIsNotExceptionInterceptor
        {
            get { return "Type '{0}' is not derived from BaseExceptionInterceptor"; }
        }

        public static String TypeIsNotCommandInterceptor
        {
            get { return "Type '{0}' is not derived from BaseCommandInterceptor"; }
        }

        public static String UnknownAuthenticationMethod
        {
            get { return "Unknown authentication method '{0}' was requested."; }
        }

        public static String AuthenticationFailed
        {
            get { return "Authentication to host '{0}' for user '{1}' using method '{2}' failed with message: {3}"; }
        }

        public static String WinAuthNotSupportOnPlatform
        {
            get { return "Windows authentication connections are not supported on {0}"; }
        }

        public static String AuthenticationMethodNotSupported
        {
            get { return "Authentication method '{0}' not supported by any of the available plugins."; }
        }

        public static String UnableToCreateAuthPlugin
        {
            get { return "Unable to create plugin for authentication method '{0}'. Please see inner exception for details."; }
        }

        public static String MixedParameterNamingNotAllowed
        {
            get { return "Mixing named and unnamed parameters is not allowed."; }
        }

        public static String ParameterIndexNotFound
        {
            get { return "Parameter index was not found in Parameter Collection."; }
        }

        public static String OldPasswordsNotSupported
        {
            get { return "Authentication with old password no longer supported, use 4.1 style passwords."; }
        }

        public static String InvalidMicrosecondValue
        {
            get { return "Microsecond must be a value between 0 and 999999."; }
        }

        public static String InvalidMillisecondValue
        {
            get { return "Millisecond must be a value between 0 and 999. For more precision use Microsecond."; }
        }

        public static String Replication_NoAvailableServer
        {
            get { return "No available server found."; }
        }

        public static String Replication_ConnectionAttemptFailed
        {
            get { return "Attempt to connect to '{0}' server failed."; }
        }

        public static String UnknownConnectionProtocol
        {
            get { return "Unknown connection protocol"; }
        }

        public static String NoUnixSocketsOnWindows
        {
            get { return "Unix sockets are not supported on Windows."; }
        }

        public static String ReplicationServerNotFound
        {
            get { return "Replicated server not found: '{0}'"; }
        }

        public static String ReplicationGroupNotFound
        {
            get { return "Replication group '{0}' not found."; }
        }

        public static String NewValueShouldBeMySqlParameter
        {
            get { return "The new value must be a MySqlParameter object."; }
        }

        public static String ValueNotCorrectType
        {
            get { return "Value '{0}' is not of the correct type."; }
        }

        public static String RSAPublicKeyRetrievalNotEnabled
        {
            get { return "Retrieval of the RSA public key is not enabled for insecure connections."; }
        }

        public static String UnableToReadRSAKey
        {
            get { return "Error encountered reading the RSA public key."; }
        }
    }
}
