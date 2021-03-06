﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.Endpoints.Oauth2.Models.Auth
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="SD_SingleSignOn")]
	public partial class AuthDialogDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertUser(User instance);
    partial void UpdateUser(User instance);
    partial void DeleteUser(User instance);
    partial void InsertRole(Role instance);
    partial void UpdateRole(Role instance);
    partial void DeleteRole(Role instance);
    partial void InsertAuthenticatorAvailable(AuthenticatorAvailable instance);
    partial void UpdateAuthenticatorAvailable(AuthenticatorAvailable instance);
    partial void DeleteAuthenticatorAvailable(AuthenticatorAvailable instance);
    partial void InsertScopesRequested(ScopesRequested instance);
    partial void UpdateScopesRequested(ScopesRequested instance);
    partial void DeleteScopesRequested(ScopesRequested instance);
    #endregion
		
		public AuthDialogDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["DbmlConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public AuthDialogDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AuthDialogDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AuthDialogDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AuthDialogDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ValidationResult> ValidationResults
		{
			get
			{
				return this.GetTable<ValidationResult>();
			}
		}
		
		public System.Data.Linq.Table<RegenerationResult> RegenerationResults
		{
			get
			{
				return this.GetTable<RegenerationResult>();
			}
		}
		
		public System.Data.Linq.Table<User> Users
		{
			get
			{
				return this.GetTable<User>();
			}
		}
		
		public System.Data.Linq.Table<Role> Roles
		{
			get
			{
				return this.GetTable<Role>();
			}
		}
		
		public System.Data.Linq.Table<Application> Applications
		{
			get
			{
				return this.GetTable<Application>();
			}
		}
		
		public System.Data.Linq.Table<UserApplication> UserApplications
		{
			get
			{
				return this.GetTable<UserApplication>();
			}
		}
		
		public System.Data.Linq.Table<AuthenticatorAvailable> AuthenticatorAvailables
		{
			get
			{
				return this.GetTable<AuthenticatorAvailable>();
			}
		}
		
		public System.Data.Linq.Table<ScopesRequested> ScopesRequesteds
		{
			get
			{
				return this.GetTable<ScopesRequested>();
			}
		}
		
		public System.Data.Linq.Table<T_Scope> T_Scopes
		{
			get
			{
				return this.GetTable<T_Scope>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class ValidationResult
	{
		
		private bool _authenticated;
		
		private bool _authorized;
		
		private string _user;
		
		public ValidationResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="Autenticado", Storage="_authenticated")]
		public bool authenticated
		{
			get
			{
				return this._authenticated;
			}
			set
			{
				if ((this._authenticated != value))
				{
					this._authenticated = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="Autorizado", Storage="_authorized")]
		public bool authorized
		{
			get
			{
				return this._authorized;
			}
			set
			{
				if ((this._authorized != value))
				{
					this._authorized = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_Token", Storage="_user", CanBeNull=false)]
		public string user
		{
			get
			{
				return this._user;
			}
			set
			{
				if ((this._user != value))
				{
					this._user = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class RegenerationResult
	{
		
		private bool _authorized;
		
		private System.Guid _user_token;
		
		private string _user_name;
		
		private System.Nullable<System.Guid> _user_avatar;
		
		public RegenerationResult()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="Autorizado", Storage="_authorized")]
		public bool authorized
		{
			get
			{
				return this._authorized;
			}
			set
			{
				if ((this._authorized != value))
				{
					this._authorized = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_Token", Storage="_user_token", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid user_token
		{
			get
			{
				return this._user_token;
			}
			set
			{
				if ((this._user_token != value))
				{
					this._user_token = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_NombreCompleto", Storage="_user_name", DbType="VarChar(250) NOT NULL", CanBeNull=false)]
		public string user_name
		{
			get
			{
				return this._user_name;
			}
			set
			{
				if ((this._user_name != value))
				{
					this._user_name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_Avatar", Storage="_user_avatar", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> user_avatar
		{
			get
			{
				return this._user_avatar;
			}
			set
			{
				if ((this._user_avatar != value))
				{
					this._user_avatar = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.VT_Usuarios")]
	public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _token;
		
		private System.DateTime _createdAt;
		
		private string _identifier;
		
		private System.Guid _avatar;
		
		private string _email;
		
		private string _fullname;
		
		private System.DateTime _lastConnection;
		
		private bool _active;
		
		private List<Role> _roles;
		
		private string _type_identifier;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OntokenChanging(System.Guid value);
    partial void OntokenChanged();
    partial void OncreatedAtChanging(System.DateTime value);
    partial void OncreatedAtChanged();
    partial void OnidentifierChanging(string value);
    partial void OnidentifierChanged();
    partial void OnavatarChanging(System.Guid value);
    partial void OnavatarChanged();
    partial void OnemailChanging(string value);
    partial void OnemailChanged();
    partial void OnfullnameChanging(string value);
    partial void OnfullnameChanged();
    partial void OnlastConnectionChanging(System.DateTime value);
    partial void OnlastConnectionChanged();
    partial void OnactiveChanging(bool value);
    partial void OnactiveChanged();
    partial void OnrolesChanging(List<Role> value);
    partial void OnrolesChanged();
    partial void Ontype_identifierChanging(string value);
    partial void Ontype_identifierChanged();
    #endregion
		
		public User()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_Token", Storage="_token", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid token
		{
			get
			{
				return this._token;
			}
			set
			{
				if ((this._token != value))
				{
					this.OntokenChanging(value);
					this.SendPropertyChanging();
					this._token = value;
					this.SendPropertyChanged("token");
					this.OntokenChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_FechaCreacion", Storage="_createdAt", DbType="DateTime NOT NULL")]
		public System.DateTime createdAt
		{
			get
			{
				return this._createdAt;
			}
			set
			{
				if ((this._createdAt != value))
				{
					this.OncreatedAtChanging(value);
					this.SendPropertyChanging();
					this._createdAt = value;
					this.SendPropertyChanged("createdAt");
					this.OncreatedAtChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_Identificador", Storage="_identifier", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string identifier
		{
			get
			{
				return this._identifier;
			}
			set
			{
				if ((this._identifier != value))
				{
					this.OnidentifierChanging(value);
					this.SendPropertyChanging();
					this._identifier = value;
					this.SendPropertyChanged("identifier");
					this.OnidentifierChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_Avatar", Storage="_avatar", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid avatar
		{
			get
			{
				return this._avatar;
			}
			set
			{
				if ((this._avatar != value))
				{
					this.OnavatarChanging(value);
					this.SendPropertyChanging();
					this._avatar = value;
					this.SendPropertyChanged("avatar");
					this.OnavatarChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_Email", Storage="_email", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string email
		{
			get
			{
				return this._email;
			}
			set
			{
				if ((this._email != value))
				{
					this.OnemailChanging(value);
					this.SendPropertyChanging();
					this._email = value;
					this.SendPropertyChanged("email");
					this.OnemailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_NombreCompleto", Storage="_fullname", DbType="VarChar(250) NOT NULL", CanBeNull=false)]
		public string fullname
		{
			get
			{
				return this._fullname;
			}
			set
			{
				if ((this._fullname != value))
				{
					this.OnfullnameChanging(value);
					this.SendPropertyChanging();
					this._fullname = value;
					this.SendPropertyChanged("fullname");
					this.OnfullnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_UltimaConexion", Storage="_lastConnection", DbType="DateTime NOT NULL")]
		public System.DateTime lastConnection
		{
			get
			{
				return this._lastConnection;
			}
			set
			{
				if ((this._lastConnection != value))
				{
					this.OnlastConnectionChanging(value);
					this.SendPropertyChanging();
					this._lastConnection = value;
					this.SendPropertyChanged("lastConnection");
					this.OnlastConnectionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="USUA_Activo", Storage="_active", DbType="Bit NOT NULL")]
		public bool active
		{
			get
			{
				return this._active;
			}
			set
			{
				if ((this._active != value))
				{
					this.OnactiveChanging(value);
					this.SendPropertyChanging();
					this._active = value;
					this.SendPropertyChanged("active");
					this.OnactiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_roles", CanBeNull=false)]
		public List<Role> roles
		{
			get
			{
				return this._roles;
			}
			set
			{
				if ((this._roles != value))
				{
					this.OnrolesChanging(value);
					this.SendPropertyChanging();
					this._roles = value;
					this.SendPropertyChanged("roles");
					this.OnrolesChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="TIEN_Identificador", Storage="_type_identifier", DbType="Char(4)")]
		public string type_identifier
		{
			get
			{
				return this._type_identifier;
			}
			set
			{
				if ((this._type_identifier != value))
				{
					this.Ontype_identifierChanging(value);
					this.SendPropertyChanging();
					this._type_identifier = value;
					this.SendPropertyChanged("type_identifier");
					this.Ontype_identifierChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.V_Perfiles")]
	public partial class Role : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private System.Guid _token;
		
		private string _identifier;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OntokenChanging(System.Guid value);
    partial void OntokenChanged();
    partial void OnidentifierChanging(string value);
    partial void OnidentifierChanged();
    #endregion
		
		public Role()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="PERF_Token", Storage="_token", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
		public System.Guid token
		{
			get
			{
				return this._token;
			}
			set
			{
				if ((this._token != value))
				{
					this.OntokenChanging(value);
					this.SendPropertyChanging();
					this._token = value;
					this.SendPropertyChanged("token");
					this.OntokenChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="PERF_Identificador", Storage="_identifier", DbType="Char(5)")]
		public string identifier
		{
			get
			{
				return this._identifier;
			}
			set
			{
				if ((this._identifier != value))
				{
					this.OnidentifierChanging(value);
					this.SendPropertyChanging();
					this._identifier = value;
					this.SendPropertyChanged("identifier");
					this.OnidentifierChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.VT_AUT_Aplicaciones")]
	public partial class Application
	{
		
		private System.Guid _token;
		
		private System.Guid _avatar;
		
		private string _name;
		
		private int _expiration;
		
		private string _origins;
		
		private string _md5;
		
		public Application()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="APPL_Token", Storage="_token", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid token
		{
			get
			{
				return this._token;
			}
			set
			{
				if ((this._token != value))
				{
					this._token = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="APPL_Avatar", Storage="_avatar", DbType="UniqueIdentifier")]
		public System.Guid avatar
		{
			get
			{
				return this._avatar;
			}
			set
			{
				if ((this._avatar != value))
				{
					this._avatar = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="APPL_Nombre", Storage="_name", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this._name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="APPL_ExpiracionToken", Storage="_expiration", DbType="Int NOT NULL")]
		public int expiration
		{
			get
			{
				return this._expiration;
			}
			set
			{
				if ((this._expiration != value))
				{
					this._expiration = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="APPL_OrigenesHabilitados", Storage="_origins", DbType="VarChar(1000)")]
		public string origins
		{
			get
			{
				return this._origins;
			}
			set
			{
				if ((this._origins != value))
				{
					this._origins = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="APPL_MD5", Storage="_md5", DbType="VarChar(32) NOT NULL", CanBeNull=false)]
		public string md5
		{
			get
			{
				return this._md5;
			}
			set
			{
				if ((this._md5 != value))
				{
					this._md5 = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TB_AUT_Entidad_Aplicacion")]
	public partial class UserApplication
	{
		
		private string _hash;
		
		private System.DateTime _createdAT;
		
		public UserApplication()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="ENAP_Hash", Storage="_hash", DbType="VarChar(32) NOT NULL", CanBeNull=false)]
		public string hash
		{
			get
			{
				return this._hash;
			}
			set
			{
				if ((this._hash != value))
				{
					this._hash = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="ENAP_FechaCreacion", Storage="_createdAT", DbType="DateTime NOT NULL")]
		public System.DateTime createdAT
		{
			get
			{
				return this._createdAT;
			}
			set
			{
				if ((this._createdAT != value))
				{
					this._createdAT = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TB_AUT_TipoAutenticador")]
	public partial class AuthenticatorAvailable : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _name;
		
		private string _description;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnnameChanging(string value);
    partial void OnnameChanged();
    partial void OndescriptionChanging(string value);
    partial void OndescriptionChanged();
    #endregion
		
		public AuthenticatorAvailable()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="TIAU_Identificador", Storage="_name", DbType="Char(5) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				if ((this._name != value))
				{
					this.OnnameChanging(value);
					this.SendPropertyChanging();
					this._name = value;
					this.SendPropertyChanged("name");
					this.OnnameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="TIAU_Nombre", Storage="_description", DbType="VarChar(500) NOT NULL", CanBeNull=false)]
		public string description
		{
			get
			{
				return this._description;
			}
			set
			{
				if ((this._description != value))
				{
					this.OndescriptionChanging(value);
					this.SendPropertyChanging();
					this._description = value;
					this.SendPropertyChanged("description");
					this.OndescriptionChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.TB_AUT_Reclamacion")]
	public partial class ScopesRequested : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _identifier;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidentifierChanging(string value);
    partial void OnidentifierChanged();
    #endregion
		
		public ScopesRequested()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="RECL_Identificador", Storage="_identifier", DbType="VarChar(20) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string identifier
		{
			get
			{
				return this._identifier;
			}
			set
			{
				if ((this._identifier != value))
				{
					this.OnidentifierChanging(value);
					this.SendPropertyChanging();
					this._identifier = value;
					this.SendPropertyChanged("identifier");
					this.OnidentifierChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class T_Scope
	{
		
		private string _scope;
		
		public T_Scope()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_scope", CanBeNull=false)]
		public string scope
		{
			get
			{
				return this._scope;
			}
			set
			{
				if ((this._scope != value))
				{
					this._scope = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
