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

namespace API.Endpoints.Applications.Models
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="SD_WorkerManager")]
	public partial class ApplicationDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public ApplicationDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["DbmlConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ApplicationDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ApplicationDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ApplicationDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ApplicationDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<NewAplication> NewAplications
		{
			get
			{
				return this.GetTable<NewAplication>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="")]
	public partial class NewAplication
	{
		
		private string _name;
		
		private string _description;
		
		private System.Guid _avatar;
		
		private string _origins;
		
		private int _expirationToken;
		
		public NewAplication()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_name", CanBeNull=false)]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_description", CanBeNull=false)]
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
					this._description = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_avatar")]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_origins", CanBeNull=false)]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_expirationToken")]
		public int expirationToken
		{
			get
			{
				return this._expirationToken;
			}
			set
			{
				if ((this._expirationToken != value))
				{
					this._expirationToken = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
