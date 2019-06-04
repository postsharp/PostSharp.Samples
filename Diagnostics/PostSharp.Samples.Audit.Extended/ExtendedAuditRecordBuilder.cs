using PostSharp.Patterns.Diagnostics.Audit;
using PostSharp.Patterns.Diagnostics.Backends.Audit;
using PostSharp.Patterns.Diagnostics.Contexts;
using PostSharp.Patterns.Formatters;
using PostSharp.Reflection;

namespace PostSharp.Samples.Audit.Extended
{
  public class ExtendedAuditRecordBuilder : AuditRecordBuilder
  {
    public ExtendedAuditRecordBuilder(AuditBackend backend) : base(backend)
    {
    }

    protected override AuditRecord CreateRecord(LoggingContext context, ref LogRecordInfo recordInfo,
      ref LogMemberInfo memberInfo)
    {
      // Return an instance of our own extended class.
      return new ExtendedAuditRecord(context.Source.SourceType, memberInfo.MemberName, recordInfo.RecordKind);
    }

    public override void SetParameter<T>(
      int index, string parameterName, ParameterKind parameterKind, string typeName, T value, IFormatter<T> formatter)
    {
      base.SetParameter(index, parameterName, parameterKind, typeName, value, formatter);

      // When the parameter is a business object, add it to the list of correlated business objects.      
      var businessObject = value as BusinessObject;

      if (businessObject != null)
        ((ExtendedAuditRecord) CurrentRecord).RelatedBusinessObjects.Add(businessObject);
    }
  }
}