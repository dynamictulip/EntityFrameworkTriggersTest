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
    dotnet ef migrations add <myNewMigrationName> --project EntityFrameworkTriggersTest
    ```
