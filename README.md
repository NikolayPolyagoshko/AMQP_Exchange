# AMQP_Exchange

A simple Database to RabbitMQ connector (despite the name, currently RabbitMQ is only supported)\
Tested with MSSQL only, but should work with other .NET providers\
Based upon EasyNetQ by Mike Hadlow http://easynetq.com/

## Connector operation
 * Connector is a Windows Service
 * Connector uses multiple worker treads - one per Rabbit endpoint and per message direction (In or Out).
 * Connector could be started as a console app with "-debug" key 

## How it works
 * All configurable parameters except db ConnectionString are inside AMQP_Ex database.
 * Inbound workers bound to the Queues, incoming messages put into 'Inbound' table upon receiving.
 * Outbound messages can be directed to a Queue or an RoutingKey + Exchange. Outbound workers periodicaly checks 'Outbound' table and sends them out
 * There is an per-queue option for a Base64-encoded message handling. If enabled, inbound messages encoded after receiving, otbound decoded before sending
 * Logs are stored in the log table
 * Text logs are stored in %TEMP%\AMQP_Exchange. When running with service account, path is %windir%\ServiceProfiles\<Profile_Name>\AppData\Local\Temp\AMQP_Exchange

## Install
 * Unpack zip and run install.cmd. Service and the exchange db will be created.
 * If using database other than MSSQL, you`ll have to to create database and tables manually
 * Edit AMQP_Exchange.exe.config, add connection string to AMQP_Ex database
 * Edit Hosts and Queues tables, add at least one host and at least one queue
 * Start the "AMQP Exchange" service

## Tables list and configuration

 * Hosts 	- Confiruration of RabbitMQ hosts
     * HostId	- Autoincrement Id of a Host
     * Host		- RabbitMQ server hostname or IP address
     * Port		- Server port, can be Null. Default is 5672 (5671 if SSL is enabled)
     * VirtualHost    - Virtual Host, can be Null. Default is "/"
     * Username		- Username
     * Password		- Password
     * SslEnabled	- Enable this flag for a SSL connection to a RabbitMQ server
     * PrefetchCount	- Prefetch Count parameter (see RabbitMQ docs http://www.rabbitmq.com/consumer-prefetch.html). Can be Null, default is 1

 * Queues	- Confiruration of RabbitMQ queues
     * QueueId		- Autoincrement Id of a Queue
     * HostID		- Id of a RabbitMQ host in the Hosts table
     * Direction		- Should be 'In' or 'Out'
     * Name		- Queue name or Routing Key name. Routing Key can be used for outbound messages, only if Exchange is specified\
     * SenderPollInterval - For outbound, controls how frequently worker will check Outbound table for new available message data. Can be null, default is 10000 ms (10 seconds)
     * Base64Data         - If enabled for inbound, messages will be Base64-encoded upon receiving. For outbound, messages will be Base64-decoded before sending
     * CodePage           - Codepage, can be Null. Default is UTF-8 (65001)
     * Exchange           - For outbound, RabbitMQ exchange name. Can be Null. If Exchange is specified, 'Name' is interpreted as Routing Key

 * Inbound	- Inbound messages table
     * MessageId		- Autoincrement Id of an Inbound Message
     * QueueId		- Id of a Queue this message came from
     * DateReceived	- Date when message was received. Filled by worker after writing message into Inbound table
     * DateRead		- Defauld is Null. Not processed by service. Use it in you app to filter processed messages
     * Message		- Message data itself. If 'Base64Data' enabled in Base64-encoded form
     
* Outbound 	- Outbound messages table
     * MessageId		- Autoincrement Id of an Outbound Message
     * QueueId		- Id of a Queue this message directed to
     * DateWritten	- Defauld is Null, not processed by service. Can be used in your app to store timestamp
     * DateSent		- Date when message was sent out. Filled by worker after receiving confirmation from Rabbit host
     * Message		- Message data itself. If 'Base64Data' enabled it should be in Base64-encoded form
     * ErrorFlag		- Flag is set when something is went wrong with sending message

* Log		- Service operatrion log
     * Log_Id		- Autoincrement Id of a Log entry
     * Timestamp		- Log entry date and time
     * Source		- Source name. 'Service' or 'Sender_XX' or 'Receiver_XX', where XX is the QueueId\
     * IsError		- Flag is set if this log entry is an error log entry\
     * Inbound_Id	- Id of the Inbound message, if applicable
     * Outbound_Id	- Id of the Outbound message, if applicable
     * Message		- Log message
     * Details		- Extended log message
