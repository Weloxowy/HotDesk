
using FluentMigrator;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Entities;

namespace HotDeskApp.Server.Persistance.DatabaseMigrations._2024._10;

[Migration(202410090500)]
public class _202410090500_CreateTable_RefreshToken : Migration
{
    private readonly string _tableName = "RefreshToken";

    public override void Up()
    {
        if (!Schema.Table(_tableName).Exists())
        {
            Create.Table(_tableName)
                .WithColumn(nameof(RefreshToken.Id)).AsGuid().NotNullable().PrimaryKey()
                .WithColumn(nameof(RefreshToken.Token)).AsString().NotNullable()
                .WithColumn(nameof(RefreshToken.UserId)).AsGuid().NotNullable()
                .WithColumn(nameof(RefreshToken.IsRevoked)).AsBoolean().NotNullable()
                .WithColumn(nameof(RefreshToken.Expiration)).AsDateTime().Nullable();
        }
    }

    public override void Down()
    {
        Delete.Table(_tableName);
    }
}