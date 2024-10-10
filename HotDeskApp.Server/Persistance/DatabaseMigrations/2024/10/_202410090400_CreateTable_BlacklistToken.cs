using FluentMigrator;
using HotDeskApp.Server.Models.Tokens.BlacklistToken.Entities;

namespace HotDeskApp.Server.Persistance.DatabaseMigrations._2024._10;

[Migration(202410090400)]
public class _202410090400_CreateTable_BlacklistToken : Migration
{
    private readonly string _tableName = "BlacklistToken";

    public override void Up()
    {
        if (!Schema.Table(_tableName).Exists())
        {
            Create.Table(_tableName)
                .WithColumn(nameof(BlacklistToken.Token)).AsString().NotNullable()
                .PrimaryKey()
                .WithColumn(nameof(BlacklistToken.EndOfBlacklisting)).AsDateTime()
                .NotNullable()
                .WithColumn(nameof(BlacklistToken.UserId)).AsGuid().NotNullable();
        }
    }

    public override void Down()
    {
        Delete.Table(_tableName);
    }
}