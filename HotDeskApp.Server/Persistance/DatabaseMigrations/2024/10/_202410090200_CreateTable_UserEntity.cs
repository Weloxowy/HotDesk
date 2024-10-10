using FluentMigrator;

namespace HotDeskApp.Server.Persistance.DatabaseMigrations._2024._10;

[Migration(202410090200)]
public class _202410090200_CreateTable_UserEntity : Migration
{
    private readonly string _tableName = "UserEntity";

    public override void Up()
    {
        if (!Schema.Table(_tableName).Exists())
        {
            Create.Table(_tableName)
                .WithColumn(nameof(Models.UserEntity.Entities.UserEntity.Id)).AsGuid().NotNullable()
                .PrimaryKey()
                .WithColumn(nameof(Models.UserEntity.Entities.UserEntity.Name)).AsString()
                .NotNullable()
                .WithColumn(nameof(Models.UserEntity.Entities.UserEntity.Surname)).AsString()
                .NotNullable()
                .WithColumn(nameof(Models.UserEntity.Entities.UserEntity.Email)).AsString()
                .NotNullable()
                .WithColumn(nameof(Models.UserEntity.Entities.UserEntity.PasswordHash)).AsString()
                .NotNullable()
                .WithColumn(nameof(Models.UserEntity.Entities.UserEntity.LastTimeLogged)).AsDateTime()
                .NotNullable()
                .WithColumn(nameof(Models.UserEntity.Entities.UserEntity.UserRole)).AsInt32()
                .NotNullable();
        }
    }

    public override void Down()
    {
        if (Schema.Table(_tableName).Exists())
        {
            Delete.Table(_tableName);
        }
    }
}