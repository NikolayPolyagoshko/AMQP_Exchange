﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:2.0.50727.5485
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AMQP_Db
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	public partial class exDb : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertHosts(RabbitHost instance);
    partial void UpdateHosts(RabbitHost instance);
    partial void DeleteHosts(RabbitHost instance);
    partial void InsertInbound(Inbound instance);
    partial void UpdateInbound(Inbound instance);
    partial void DeleteInbound(Inbound instance);
    partial void InsertLog(LogRecord instance);
    partial void UpdateLog(LogRecord instance);
    partial void DeleteLog(LogRecord instance);
    partial void InsertOutbound(Outbound instance);
    partial void UpdateOutbound(Outbound instance);
    partial void DeleteOutbound(Outbound instance);
    partial void InsertQueues(RabbitQueue instance);
    partial void UpdateQueues(RabbitQueue instance);
    partial void DeleteQueues(RabbitQueue instance);
    #endregion
		
		public exDb(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public exDb(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public exDb(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public exDb(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<RabbitHost> Hosts
		{
			get
			{
				return this.GetTable<RabbitHost>();
			}
		}
		
		public System.Data.Linq.Table<Inbound> Inbound
		{
			get
			{
				return this.GetTable<Inbound>();
			}
		}
		
		public System.Data.Linq.Table<LogRecord> LogTable
		{
			get
			{
				return this.GetTable<LogRecord>();
			}
		}
		
		public System.Data.Linq.Table<Outbound> Outbound
		{
			get
			{
				return this.GetTable<Outbound>();
			}
		}
		
		public System.Data.Linq.Table<RabbitQueue> Queues
		{
			get
			{
				return this.GetTable<RabbitQueue>();
			}
		}
	}
	
	[Table(Name="dbo.Hosts")]
	public partial class RabbitHost : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Host_Id;
		
		private string _Host;
		
		private System.Nullable<int> _Port;
		
		private string _VirtualHost;
		
		private string _Username;
		
		private string _Password;
		
		private bool _SslEnabled;
		
		private System.Nullable<int> _PrefetchCount;
		
		private EntitySet<RabbitQueue> _Queues;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnHost_IdChanging(int value);
    partial void OnHost_IdChanged();
    partial void OnHostChanging(string value);
    partial void OnHostChanged();
    partial void OnPortChanging(System.Nullable<int> value);
    partial void OnPortChanged();
    partial void OnVirtualHostChanging(string value);
    partial void OnVirtualHostChanged();
    partial void OnUsernameChanging(string value);
    partial void OnUsernameChanged();
    partial void OnPasswordChanging(string value);
    partial void OnPasswordChanged();
    partial void OnSslEnabledChanging(bool value);
    partial void OnSslEnabledChanged();
    partial void OnPrefetchCountChanging(System.Nullable<int> value);
    partial void OnPrefetchCountChanged();
    #endregion
		
		public RabbitHost()
		{
			this._Queues = new EntitySet<RabbitQueue>(new Action<RabbitQueue>(this.attach_Queues), new Action<RabbitQueue>(this.detach_Queues));
			OnCreated();
		}
		
		[Column(Storage="_Host_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Host_Id
		{
			get
			{
				return this._Host_Id;
			}
			set
			{
				if ((this._Host_Id != value))
				{
					this.OnHost_IdChanging(value);
					this.SendPropertyChanging();
					this._Host_Id = value;
					this.SendPropertyChanged("Host_Id");
					this.OnHost_IdChanged();
				}
			}
		}
		
		[Column(Storage="_Host", DbType="NVarChar(200) NOT NULL", CanBeNull=false)]
		public string Host
		{
			get
			{
				return this._Host;
			}
			set
			{
				if ((this._Host != value))
				{
					this.OnHostChanging(value);
					this.SendPropertyChanging();
					this._Host = value;
					this.SendPropertyChanged("Host");
					this.OnHostChanged();
				}
			}
		}
		
		[Column(Storage="_Port", DbType="Int")]
		public System.Nullable<int> Port
		{
			get
			{
				return this._Port;
			}
			set
			{
				if ((this._Port != value))
				{
					this.OnPortChanging(value);
					this.SendPropertyChanging();
					this._Port = value;
					this.SendPropertyChanged("Port");
					this.OnPortChanged();
				}
			}
		}
		
		[Column(Storage="_VirtualHost", DbType="NVarChar(200)")]
		public string VirtualHost
		{
			get
			{
				return this._VirtualHost;
			}
			set
			{
				if ((this._VirtualHost != value))
				{
					this.OnVirtualHostChanging(value);
					this.SendPropertyChanging();
					this._VirtualHost = value;
					this.SendPropertyChanged("VirtualHost");
					this.OnVirtualHostChanged();
				}
			}
		}
		
		[Column(Storage="_Username", DbType="NVarChar(200)")]
		public string Username
		{
			get
			{
				return this._Username;
			}
			set
			{
				if ((this._Username != value))
				{
					this.OnUsernameChanging(value);
					this.SendPropertyChanging();
					this._Username = value;
					this.SendPropertyChanged("Username");
					this.OnUsernameChanged();
				}
			}
		}
		
		[Column(Storage="_Password", DbType="NVarChar(200)")]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[Column(Storage="_SslEnabled", DbType="Bit NOT NULL")]
		public bool SslEnabled
		{
			get
			{
				return this._SslEnabled;
			}
			set
			{
				if ((this._SslEnabled != value))
				{
					this.OnSslEnabledChanging(value);
					this.SendPropertyChanging();
					this._SslEnabled = value;
					this.SendPropertyChanged("SslEnabled");
					this.OnSslEnabledChanged();
				}
			}
		}
		
		[Column(Storage="_PrefetchCount", DbType="Int")]
		public System.Nullable<int> PrefetchCount
		{
			get
			{
				return this._PrefetchCount;
			}
			set
			{
				if ((this._PrefetchCount != value))
				{
					this.OnPrefetchCountChanging(value);
					this.SendPropertyChanging();
					this._PrefetchCount = value;
					this.SendPropertyChanged("PrefetchCount");
					this.OnPrefetchCountChanged();
				}
			}
		}
		
		[Association(Name="FK_Queues_Hosts", Storage="_Queues", ThisKey="Host_Id", OtherKey="Host_Id", DeleteRule="NO ACTION")]
		public EntitySet<RabbitQueue> Queues
		{
			get
			{
				return this._Queues;
			}
			set
			{
				this._Queues.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Queues(RabbitQueue entity)
		{
			this.SendPropertyChanging();
			entity.Host = this;
		}
		
		private void detach_Queues(RabbitQueue entity)
		{
			this.SendPropertyChanging();
			entity.Host = null;
		}
	}
	
	[Table(Name="dbo.Inbound")]
	public partial class Inbound : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Message_Id;
		
		private int _QueueId;
		
		private System.DateTime _DateReceived;
		
		private System.Nullable<System.DateTime> _DateRead;
		
		private string _Message;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnMessage_IdChanging(int value);
    partial void OnMessage_IdChanged();
    partial void OnQueueIdChanging(int value);
    partial void OnQueueIdChanged();
    partial void OnDateReceivedChanging(System.DateTime value);
    partial void OnDateReceivedChanged();
    partial void OnDateReadChanging(System.Nullable<System.DateTime> value);
    partial void OnDateReadChanged();
    partial void OnMessageChanging(string value);
    partial void OnMessageChanged();
    #endregion
		
		public Inbound()
		{
			OnCreated();
		}
		
		[Column(Storage="_Message_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Message_Id
		{
			get
			{
				return this._Message_Id;
			}
			set
			{
				if ((this._Message_Id != value))
				{
					this.OnMessage_IdChanging(value);
					this.SendPropertyChanging();
					this._Message_Id = value;
					this.SendPropertyChanged("Message_Id");
					this.OnMessage_IdChanged();
				}
			}
		}
		
		[Column(Storage="_QueueId", DbType="Int NOT NULL")]
		public int QueueId
		{
			get
			{
				return this._QueueId;
			}
			set
			{
				if ((this._QueueId != value))
				{
					this.OnQueueIdChanging(value);
					this.SendPropertyChanging();
					this._QueueId = value;
					this.SendPropertyChanged("QueueId");
					this.OnQueueIdChanged();
				}
			}
		}
		
		[Column(Storage="_DateReceived", DbType="DateTime NOT NULL")]
		public System.DateTime DateReceived
		{
			get
			{
				return this._DateReceived;
			}
			set
			{
				if ((this._DateReceived != value))
				{
					this.OnDateReceivedChanging(value);
					this.SendPropertyChanging();
					this._DateReceived = value;
					this.SendPropertyChanged("DateReceived");
					this.OnDateReceivedChanged();
				}
			}
		}
		
		[Column(Storage="_DateRead", DbType="DateTime")]
		public System.Nullable<System.DateTime> DateRead
		{
			get
			{
				return this._DateRead;
			}
			set
			{
				if ((this._DateRead != value))
				{
					this.OnDateReadChanging(value);
					this.SendPropertyChanging();
					this._DateRead = value;
					this.SendPropertyChanged("DateRead");
					this.OnDateReadChanged();
				}
			}
		}
		
		[Column(Storage="_Message", DbType="NVarChar(MAX)", UpdateCheck=UpdateCheck.Never)]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this.OnMessageChanging(value);
					this.SendPropertyChanging();
					this._Message = value;
					this.SendPropertyChanged("Message");
					this.OnMessageChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Log")]
	public partial class LogRecord : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Log_Id;
		
		private System.DateTime _Timestamp;
		
		private string _Source;
		
		private bool _IsError;
		
		private System.Nullable<int> _HostId;
		
		private System.Nullable<int> _QueueId;
		
		private System.Nullable<int> _Inbound_Id;
		
		private System.Nullable<int> _Outbound_Id;
		
		private string _Message;
		
		private string _Details;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnLog_IdChanging(int value);
    partial void OnLog_IdChanged();
    partial void OnTimestampChanging(System.DateTime value);
    partial void OnTimestampChanged();
    partial void OnSourceChanging(string value);
    partial void OnSourceChanged();
    partial void OnIsErrorChanging(bool value);
    partial void OnIsErrorChanged();
    partial void OnHostIdChanging(System.Nullable<int> value);
    partial void OnHostIdChanged();
    partial void OnQueueIdChanging(System.Nullable<int> value);
    partial void OnQueueIdChanged();
    partial void OnInbound_IdChanging(System.Nullable<int> value);
    partial void OnInbound_IdChanged();
    partial void OnOutbound_IdChanging(System.Nullable<int> value);
    partial void OnOutbound_IdChanged();
    partial void OnMessageChanging(string value);
    partial void OnMessageChanged();
    partial void OnDetailsChanging(string value);
    partial void OnDetailsChanged();
    #endregion
		
		public LogRecord()
		{
			Timestamp = DateTime.Now;
			OnCreated();
		}
		
		[Column(Storage="_Log_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Log_Id
		{
			get
			{
				return this._Log_Id;
			}
			set
			{
				if ((this._Log_Id != value))
				{
					this.OnLog_IdChanging(value);
					this.SendPropertyChanging();
					this._Log_Id = value;
					this.SendPropertyChanged("Log_Id");
					this.OnLog_IdChanged();
				}
			}
		}
		
		[Column(Storage="_Timestamp", DbType="DateTime NOT NULL")]
		public System.DateTime Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				if ((this._Timestamp != value))
				{
					this.OnTimestampChanging(value);
					this.SendPropertyChanging();
					this._Timestamp = value;
					this.SendPropertyChanged("Timestamp");
					this.OnTimestampChanged();
				}
			}
		}
		
		[Column(Storage="_Source", DbType="NVarChar(200)")]
		public string Source
		{
			get
			{
				return this._Source;
			}
			set
			{
				if ((this._Source != value))
				{
					this.OnSourceChanging(value);
					this.SendPropertyChanging();
					this._Source = value;
					this.SendPropertyChanged("Source");
					this.OnSourceChanged();
				}
			}
		}
		
		[Column(Storage="_IsError", DbType="Bit NOT NULL")]
		public bool IsError
		{
			get
			{
				return this._IsError;
			}
			set
			{
				if ((this._IsError != value))
				{
					this.OnIsErrorChanging(value);
					this.SendPropertyChanging();
					this._IsError = value;
					this.SendPropertyChanged("IsError");
					this.OnIsErrorChanged();
				}
			}
		}
		
		[Column(Storage="_HostId", DbType="Int")]
		public System.Nullable<int> HostId
		{
			get
			{
				return this._HostId;
			}
			set
			{
				if ((this._HostId != value))
				{
					this.OnHostIdChanging(value);
					this.SendPropertyChanging();
					this._HostId = value;
					this.SendPropertyChanged("HostId");
					this.OnHostIdChanged();
				}
			}
		}
		
		[Column(Storage="_QueueId", DbType="Int")]
		public System.Nullable<int> QueueId
		{
			get
			{
				return this._QueueId;
			}
			set
			{
				if ((this._QueueId != value))
				{
					this.OnQueueIdChanging(value);
					this.SendPropertyChanging();
					this._QueueId = value;
					this.SendPropertyChanged("QueueId");
					this.OnQueueIdChanged();
				}
			}
		}
		
		[Column(Storage="_Inbound_Id", DbType="Int")]
		public System.Nullable<int> Inbound_Id
		{
			get
			{
				return this._Inbound_Id;
			}
			set
			{
				if ((this._Inbound_Id != value))
				{
					this.OnQueueIdChanging(value);
					this.SendPropertyChanging();
					this._Inbound_Id = value;
					this.SendPropertyChanged("Inbound_Id");
					this.OnQueueIdChanged();
				}
			}
		}
		
		[Column(Storage="_Outbound_Id", DbType="Int")]
		public System.Nullable<int> Outbound_Id
		{
			get
			{
				return this._Outbound_Id;
			}
			set
			{
				if ((this._Outbound_Id != value))
				{
					this.OnQueueIdChanging(value);
					this.SendPropertyChanging();
					this._Outbound_Id = value;
					this.SendPropertyChanged("OutboundId");
					this.OnQueueIdChanged();
				}
			}
		}
		
		[Column(Storage="_Message", DbType="NVarChar(MAX)", UpdateCheck=UpdateCheck.Never)]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this.OnMessageChanging(value);
					this.SendPropertyChanging();
					this._Message = value;
					this.SendPropertyChanged("Message");
					this.OnMessageChanged();
				}
			}
		}
		
		[Column(Storage="_Details", DbType="NVarChar(MAX)", UpdateCheck=UpdateCheck.Never)]
		public string Details
		{
			get
			{
				return this._Details;
			}
			set
			{
				if ((this._Details != value))
				{
					this.OnDetailsChanging(value);
					this.SendPropertyChanging();
					this._Details = value;
					this.SendPropertyChanged("Details");
					this.OnDetailsChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Outbound")]
	public partial class Outbound : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Message_Id;
		
		private int _QueueId;
		
		private System.Nullable<System.DateTime> _DateWritten;
		
		private System.Nullable<System.DateTime> _DateSent;
		
		private string _Message;
		
		private bool _ErrorFlag;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnMessage_IdChanging(int value);
    partial void OnMessage_IdChanged();
    partial void OnQueueIdChanging(int value);
    partial void OnQueueIdChanged();
    partial void OnDateWrittenChanging(System.Nullable<System.DateTime> value);
    partial void OnDateWrittenChanged();
    partial void OnDateSentChanging(System.Nullable<System.DateTime> value);
    partial void OnDateSentChanged();
    partial void OnMessageChanging(string value);
    partial void OnMessageChanged();
    partial void OnErrorFlagChanging(bool value);
    partial void OnErrorFlagChanged();
    #endregion
		
		public Outbound()
		{
			OnCreated();
		}
		
		[Column(Storage="_Message_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Message_Id
		{
			get
			{
				return this._Message_Id;
			}
			set
			{
				if ((this._Message_Id != value))
				{
					this.OnMessage_IdChanging(value);
					this.SendPropertyChanging();
					this._Message_Id = value;
					this.SendPropertyChanged("Message_Id");
					this.OnMessage_IdChanged();
				}
			}
		}
		
		[Column(Storage="_QueueId", DbType="Int NOT NULL")]
		public int QueueId
		{
			get
			{
				return this._QueueId;
			}
			set
			{
				if ((this._QueueId != value))
				{
					this.OnQueueIdChanging(value);
					this.SendPropertyChanging();
					this._QueueId = value;
					this.SendPropertyChanged("QueueId");
					this.OnQueueIdChanged();
				}
			}
		}
		
		[Column(Storage="_DateWritten", DbType="DateTime")]
		public System.Nullable<System.DateTime> DateWritten
		{
			get
			{
				return this._DateWritten;
			}
			set
			{
				if ((this._DateWritten != value))
				{
					this.OnDateWrittenChanging(value);
					this.SendPropertyChanging();
					this._DateWritten = value;
					this.SendPropertyChanged("DateWritten");
					this.OnDateWrittenChanged();
				}
			}
		}
		
		[Column(Storage="_DateSent", DbType="DateTime")]
		public System.Nullable<System.DateTime> DateSent
		{
			get
			{
				return this._DateSent;
			}
			set
			{
				if ((this._DateSent != value))
				{
					this.OnDateSentChanging(value);
					this.SendPropertyChanging();
					this._DateSent = value;
					this.SendPropertyChanged("DateSent");
					this.OnDateSentChanged();
				}
			}
		}
		
		[Column(Storage="_Message", DbType="NVarChar(MAX)", UpdateCheck=UpdateCheck.Never)]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this.OnMessageChanging(value);
					this.SendPropertyChanging();
					this._Message = value;
					this.SendPropertyChanged("Message");
					this.OnMessageChanged();
				}
			}
		}
		
		[Column(Storage="_ErrorFlag", DbType="Bit NOT NULL")]
		public bool ErrorFlag
		{
			get
			{
				return this._ErrorFlag;
			}
			set
			{
				if ((this._ErrorFlag != value))
				{
					this.OnErrorFlagChanging(value);
					this.SendPropertyChanging();
					this._ErrorFlag = value;
					this.SendPropertyChanged("ErrorFlag");
					this.OnErrorFlagChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.Queues")]
	public partial class RabbitQueue : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _Queue_Id;
		
		private int _Host_Id;
		
		private string _Direction;
		
		private string _Name;
		
		private string _Exchange;
		
		private System.Nullable<int> _SenderPollInterval;
		
		private bool _Base64Data;
		
		private System.Nullable<int> _Codepage;
		
		private EntityRef<RabbitHost> _Hosts;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnQueue_IdChanging(int value);
    partial void OnQueue_IdChanged();
    partial void OnHost_IdChanging(int value);
    partial void OnHost_IdChanged();
    partial void OnDirectionChanging(string value);
    partial void OnDirectionChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnExchangeChanging(string value);
    partial void OnExchangeChanged();
    partial void OnSenderPollIntervalChanging(System.Nullable<int> value);
    partial void OnSenderPollIntervalChanged();
    partial void OnBase64DataChanging(bool value);
    partial void OnBase64DataChanged();
    partial void OnCodepageChanging(System.Nullable<int> value);
    partial void OnCodepageChanged();
    #endregion
		
		public RabbitQueue()
		{
			this._Hosts = default(EntityRef<RabbitHost>);
			OnCreated();
		}
		
		[Column(Storage="_Queue_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int Queue_Id
		{
			get
			{
				return this._Queue_Id;
			}
			set
			{
				if ((this._Queue_Id != value))
				{
					this.OnQueue_IdChanging(value);
					this.SendPropertyChanging();
					this._Queue_Id = value;
					this.SendPropertyChanged("Queue_Id");
					this.OnQueue_IdChanged();
				}
			}
		}
		
		[Column(Storage="_Host_Id", DbType="Int NOT NULL")]
		public int Host_Id
		{
			get
			{
				return this._Host_Id;
			}
			set
			{
				if ((this._Host_Id != value))
				{
					if (this._Hosts.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnHost_IdChanging(value);
					this.SendPropertyChanging();
					this._Host_Id = value;
					this.SendPropertyChanged("Host_Id");
					this.OnHost_IdChanged();
				}
			}
		}
		
		[Column(Storage="_Direction", DbType="NChar(10) NOT NULL", CanBeNull=false)]
		public string Direction
		{
			get
			{
				return this._Direction;
			}
			set
			{
				if ((this._Direction != value))
				{
					this.OnDirectionChanging(value);
					this.SendPropertyChanging();
					this._Direction = value;
					this.SendPropertyChanged("Direction");
					this.OnDirectionChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="NVarChar(1000)", CanBeNull=true)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_Exchange", DbType="NVarChar(1000)", CanBeNull=true)]
		public string Exchange
		{
			get
			{
				return this._Exchange;
			}
			set
			{
				if ((this._Exchange != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Exchange = value;
					this.SendPropertyChanged("Exchange");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_SenderPollInterval", DbType="Int")]
		public System.Nullable<int> SenderPollInterval
		{
			get
			{
				return this._SenderPollInterval;
			}
			set
			{
				if ((this._SenderPollInterval != value))
				{
					this.OnSenderPollIntervalChanging(value);
					this.SendPropertyChanging();
					this._SenderPollInterval = value;
					this.SendPropertyChanged("SenderPollInterval");
					this.OnSenderPollIntervalChanged();
				}
			}
		}
		
		[Column(Storage="_Base64Data", DbType="Bit NOT NULL")]
		public bool Base64Data
		{
			get
			{
				return this._Base64Data;
			}
			set
			{
				if ((this._Base64Data != value))
				{
					this.OnBase64DataChanging(value);
					this.SendPropertyChanging();
					this._Base64Data = value;
					this.SendPropertyChanged("Base64Data");
					this.OnBase64DataChanged();
				}
			}
		}
		
		[Column(Storage="_Codepage", DbType="Int")]
		public System.Nullable<int> Codepage
		{
			get
			{
				return this._Codepage;
			}
			set
			{
				if ((this._Codepage != value))
				{
					this.OnCodepageChanging(value);
					this.SendPropertyChanging();
					this._Codepage = value;
					this.SendPropertyChanged("Codepage");
					this.OnCodepageChanged();
				}
			}
		}
		
		[Association(Name="FK_Queues_Hosts", Storage="_Hosts", ThisKey="Host_Id", OtherKey="Host_Id", IsForeignKey=true)]
		public RabbitHost Host
		{
			get
			{
				return this._Hosts.Entity;
			}
			set
			{
				RabbitHost previousValue = this._Hosts.Entity;
				if (((previousValue != value) 
							|| (this._Hosts.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Hosts.Entity = null;
						previousValue.Queues.Remove(this);
					}
					this._Hosts.Entity = value;
					if ((value != null))
					{
						value.Queues.Add(this);
						this._Host_Id = value.Host_Id;
					}
					else
					{
						this._Host_Id = default(int);
					}
					this.SendPropertyChanged("Hosts");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
