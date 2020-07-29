using AutoMapper;
using AutoMapper.Data;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace PostSharp.Samples.StoredProcedure
{

  [StoredProcedure(AttributeInheritance = MulticastInheritance.Multicast)]
  internal abstract class BaseDbApi
  {

    protected BaseDbApi( SqlConnection connection, SqlTransaction transaction = null )
    {
      this.Connection = connection;
      this.Transaction = transaction;

      var mapperConfig = new MapperConfiguration(cfg =>
      {
        cfg.AddDataReaderMapping();
        cfg.CreateMap<IDataRecord, Speaker>();
      });

      this.Mapper = mapperConfig.CreateMapper();

    }

    public SqlConnection Connection
    {
      get;
    }

    public SqlTransaction Transaction
    {
      get;
    }

    public IMapper Mapper
    {
      get;
    }


  }

}
