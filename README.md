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

## http file (aka super-easy request making machine)

`LookHere.http` can be used with Rider, Visual Studio or VS Code (with appropriate plugin) to make API calls without bothering with Swagger. Look at [Jetbrains http file docs](https://www.jetbrains.com/help/idea/exploring-http-syntax.html) for more info on how to use (including [dynamic variables](https://www.jetbrains.com/help/idea/exploring-http-syntax.html#dynamic-variables))
