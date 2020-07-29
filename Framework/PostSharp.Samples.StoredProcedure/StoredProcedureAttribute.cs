using AutoMapper;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace PostSharp.Samples.StoredProcedure
{
  [PSerializable]
  internal class StoredProcedureAttribute : MethodInterceptionAspect
  {
    MethodInfo mapDataReaderMethod;
    MethodInfo mapDataReaderAsyncMethod;

    public override void RuntimeInitialize(MethodBase method)
    {
      this.mapDataReaderMethod = this.GetType().GetMethod(nameof(MapDataReader), BindingFlags.NonPublic | BindingFlags.Static);
      this.mapDataReaderAsyncMethod = this.GetType().GetMethod(nameof(MapDataReaderAsync), BindingFlags.NonPublic | BindingFlags.Static);
    }

    public override bool CompileTimeValidate(MethodBase method)
    {
      if (method.MethodImplementationFlags != MethodImplAttributes.InternalCall)
      {
        // We transform only extern methods.
        return false;
      }

      var methodInfo =  (MethodInfo) method;

   
      // Validate the parameter types.
      var success = true;
      foreach (var parameter in methodInfo.GetParameters())
      {
        if (MapType(parameter.ParameterType) == null)
        {
          Message.Write(parameter, SeverityType.Error, "SP001", "The type of parameter {0} cannot be mapped to a database type.", parameter);
          success = false;
        }
      }


      // Validate the return type.
      var returnType = methodInfo.ReturnType;

      if (returnType == typeof(Task))
      {
        returnType = typeof(void);
      }
      else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
      {
        returnType = returnType.GetGenericArguments()[0];
      }

      if (returnType != typeof(void) &&
        !(methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) &&
        !(methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>)))
      {
        Message.Write(methodInfo, SeverityType.Error, "SP002", "The return type of method {0} must be void, IEnumerable<> or IAsyncEnumerable<>.", methodInfo);
      }

      return success;

    }

    public override void OnInvoke(MethodInterceptionArgs args)
    {
      var method = (MethodInfo) args.Method;
      var instance = (BaseDbApi) args.Instance;
      var command = CreateCommand(method, instance, args.Arguments);

      var returnType = method.ReturnType;

      if (returnType == typeof(void))
      {
        command.ExecuteNonQuery();
      }
      else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
      {
        var reader = command.ExecuteReader();
        args.ReturnValue = this.mapDataReaderMethod.MakeGenericMethod(returnType.GetGenericArguments()[0]).Invoke(null, new object[] { reader, instance.Mapper });
      }

    }

    public override async Task OnInvokeAsync(MethodInterceptionArgs args)
    {
      var method = (MethodInfo) args.Method;
      var instance = (BaseDbApi) args.Instance;
      var command = CreateCommand(method, instance, args.Arguments);

      var returnType = method.ReturnType;

      if (returnType == typeof(Task))
      {
        await command.ExecuteNonQueryAsync();
      }
      else 
      {
        // Must be a Task<T>,
        returnType = returnType.GetGenericArguments()[0];

        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
          var reader = await command.ExecuteReaderAsync();
          args.ReturnValue = this.mapDataReaderMethod.MakeGenericMethod(returnType.GetGenericArguments()[0]).Invoke(null, new object[] { reader, instance.Mapper });
        }
        else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>))
        {
          var reader = await command.ExecuteReaderAsync();
          args.ReturnValue = this.mapDataReaderAsyncMethod.MakeGenericMethod(returnType.GetGenericArguments()[0]).Invoke(null, new object[] { reader, instance.Mapper });
        }

      }

    }

    private static IEnumerable<T> MapDataReader<T>(SqlDataReader reader, IMapper mapper)
    {
      try
      {
        while (reader.Read())
        {
          yield return mapper.Map<IDataRecord, T>(reader);
        }
      }
      finally
      {
        reader.Close();
      }

    }

    private static async IAsyncEnumerable<T> MapDataReaderAsync<T>(SqlDataReader reader, IMapper mapper)
    {
      try
      {
        while (await reader.ReadAsync())
        {
          yield return mapper.Map<IDataRecord, T>(reader);
        }
      }
      finally
      {
        reader.Close();
      }

    }

    private static SqlCommand CreateCommand(MethodInfo method, BaseDbApi instance, Arguments arguments)
    {

      var methodName = method.Name;
      if (methodName.EndsWith("Async"))
      {
        methodName = methodName.Substring(0, methodName.Length - "Async".Length);
      }

      var command = new SqlCommand(methodName)
      {
        Connection = instance.Connection,
        CommandType = CommandType.StoredProcedure,
        Transaction = instance.Transaction

      };

      foreach (var methodParameter in method.GetParameters())
      {
        if (!methodParameter.IsOut)
        {
          command.Parameters.AddWithValue("@" + methodParameter.Name, arguments[methodParameter.Position]);
        }
        else
        {
          var commandParameter = command.CreateParameter();
          commandParameter.ParameterName = "@" + methodParameter.Name;
          commandParameter.SqlDbType = MapType(methodParameter.ParameterType).Value;
          commandParameter.Direction = ParameterDirection.Output;

          command.Parameters.Add(commandParameter);
        }
      }

      return command;
    }

    private static SqlDbType? MapType(Type type)
    {
      // TODO: handle nullable.

      switch (Type.GetTypeCode(type))
      {
        case TypeCode.Boolean:
          return SqlDbType.Bit;

        case TypeCode.Byte:
          return SqlDbType.TinyInt;

        case TypeCode.SByte:
          return null;

        case TypeCode.Int16:
          return SqlDbType.SmallInt;

        case TypeCode.UInt16:
          return null;

        case TypeCode.Int32:
          return SqlDbType.Int;

        case TypeCode.UInt32:
          return null;

        case TypeCode.Single:
          return null;

        case TypeCode.Double:
          return SqlDbType.Float;

        case TypeCode.Char:
          return SqlDbType.NChar;

        case TypeCode.Int64:
          return SqlDbType.BigInt;

        case TypeCode.UInt64:
          return null;

        case TypeCode.Decimal:
          return SqlDbType.Decimal;


        case TypeCode.DateTime:
          return SqlDbType.DateTime;

        case TypeCode.String:
          return SqlDbType.NVarChar;

        default:
          return null;

      }

    }


  }
}
