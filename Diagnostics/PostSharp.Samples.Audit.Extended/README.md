This example demonstrates how to extend the default Audit logging back-end so that the audit records contains pieces of information that are
not provided by the default implementation.

In this example, we corrate the audit record not only to the primary business object, but also to all business objects related to this operations,
i.e. to all business objects passed as parameters for this operation.

The `ExtendedAuditRecord` class extends the `AuditRecord` class with a field exposing related business objects.

The extended audit back-end is implemented by the `ExtendedAuditBackend` class which derives from `AuditBackend` and simply overrides the `CreateRecordBuilder`
method. This method returns a instance of the new `ExtendedAuditRecordBuilder` class, derived from `AuditRecordBuilder`.

In the `ExtendedAuditRecordBuilder` class, we override the `CreateRecord` method that it returns a new instance of our `ExtendedAuditRecord` class instead of
the default `AuditRecord`. We then override the `SetParameter` method and add the parameter to the `ExtendedAuditRecord` if it's value is a business object.

In this example, we've added audit to the `PurchaseOrder.AssignTo(Employee)` method by adding the `[Audit]` aspect. 

Whenever an audited method is executed, the `AuditServices.RecordPublished` event is signaled. An application should subscribe
to this event and write all relevant pieces of information to the audit database table. The event arguments now exposes an `ExtendedAuditRecord` instead
of the default `AuditRecord`.

In this sample, the `DbAuditRecord` class simulates a database entity. In a real-world example, you would write the audit record and its relationships into a database table.



## Documentation 

 [Adding Audit to Your Solution](https://doc.postsharp.net/audit).

