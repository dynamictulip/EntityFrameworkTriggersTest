# Entity Framework triggers spike test thingy

Investigating different ways to audit transactions (and other EF core things)

## Database migrations

Microsoft documentation says that [the recommended way to deploy migrations to a production database is by generating SQL scripts](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli#sql-scripts). This spike application ignores that and does it in code >.<

However, this command generates one big sql script which can be reviewed and run on a db which is at any point of migration.

```bash
dotnet ef migrations script --idempotent -o scripts/idempotentMigrationScript.sql --project EntityFrameworkTriggersTest
```

### Add a new migration

1. Make changes to context/model objects
2. Check that the changes have been picked up
    ```bash 
    dotnet ef migrations has-pending-model-changes --project EntityFrameworkTriggersTest
    ```
3. Make a new migration
    ```bash 
    dotnet ef migrations add AuditTable --project EntityFrameworkTriggersTest
    ```

## http file (aka super-easy request making machine)

`LookHere.http` can be used with Rider, Visual Studio or VS Code (with appropriate plugin) to make API calls without bothering with Swagger. Look at [Jetbrains http file docs](https://www.jetbrains.com/help/idea/exploring-http-syntax.html) for more info on how to use (including [dynamic variables](https://www.jetbrains.com/help/idea/exploring-http-syntax.html#dynamic-variables))

## Things to read/watch:

- nuget package for auditing to a wide range of destinations https://github.com/thepirat000/Audit.NET/
-  https://learn.microsoft.com/en-us/sql/relational-databases/track-changes/about-change-data-capture-sql-server?view=sql-server-ver16
- Nick Chapsas - using interceptors to audit  https://www.youtube.com/watch?v=yKI-_TWxrSk
- SQL Server Row version type (tells you how many times a row has been updated) - https://learn.microsoft.com/en-us/sql/t-sql/data-types/rowversion-transact-sql?view=sql-server-ver16
- Json columns
    - EF - https://devblogs.microsoft.com/dotnet/announcing-ef7-release-candidate-2/#mapping-to-json-columns
    - Azure SQL Database only - https://learn.microsoft.com/en-us/sql/t-sql/data-types/json-data-type?view=azuresqldb-current&viewFallbackFrom=sql-server-ver16

## Thoughts/Questions:

Thought - To use a dedicated audit table we will need a way of representing any object type.

- This could be done in json but we would want a fail safe way of deserialising when the object type changes (and historic data doesn't). This could be done with migrations being applied to the audit table but that requires future maintainers to understand what is going on

Question - do we want to save the entire row of data or just the field that has changed? If more than one field changes then is that more than one table entry?

Question - are we conflating the ideas of "audit" and "history"?

- Audit - change trail to ensure that dodgy stuff doesn't happen. Should be immutable and permanent but shouldn't contain more data than necessary (as it's never going away)
- History - states of a thing recorded at points in time for looking back upon. Can be rewritten (think Git) and should only be kept for as long as it is useful

Question - do we actually want an audit table or should we have versioned rows in the main entity table?

- each entity inherits from a base entity
- each row has extra columns (e.g. timestamp, user, version number)
- Instead of modifying or deleting rows, they stay in the table. There is a "no data" type of row rather than a deleted one
- this could be enforced through interceptors? Might be trickier to convey to pure SQL users though?


Question - should we be using a history table per entity?

Question - should we be using a data warehouse for old data? Or somewhere else outside of our database?
