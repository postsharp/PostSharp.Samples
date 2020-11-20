This example shows how to implement audit in a business application.

Audit records are typically appended to a database and correlated, through a foreign key, to the object entity being accessed and the user performing the operation.
This allow business users or security analysts to get a trail of all operations performed on a given business object.

In this example, we've added audit to the `PurchaseOrder.Approve` method by adding the `[Audit]` aspect. 

Whenever an audited method is executed, the `AuditServices.RecordPublished` event is signaled. An application should subscribe
to this event and write all relevant pieces of information to the audit database table. The event arguments gives information about
the business object being accessed, the name of method being invoked, and a detailed description of the event similar to a logging log record.

In this sample, the `DbAuditRecord` class simulates a database entity. In a real-world example, you would write the audit record into a database table.

## Documentation 

 [Adding Audit to Your Solution](https://doc.postsharp.net/audit).

