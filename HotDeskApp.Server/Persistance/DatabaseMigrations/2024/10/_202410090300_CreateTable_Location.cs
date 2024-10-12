using FluentMigrator;

namespace HotDeskApp.Server.Persistance.DatabaseMigrations._2024._10;

[Migration(202410090300)]
public class _202410090300_CreateTable_Location : Migration
{
    private readonly string _tableName = "Location";

    public override void Up()
    {
        if (!Schema.Table(_tableName).Exists())
            Create.Table(_tableName)
                .WithColumn(nameof(Models.Location.Entities.Location.Id)).AsGuid().NotNullable()
                .PrimaryKey()
                .WithColumn(nameof(Models.Location.Entities.Location.Name)).AsString()
                .NotNullable()
                .WithColumn(nameof(Models.Location.Entities.Location.Description)).AsString()
                .NotNullable()
                .WithColumn(nameof(Models.Location.Entities.Location.CoverImgPath)).AsString()
                .NotNullable();
    }

    public override void Down()
    {
        if (Schema.Table(_tableName).Exists()) Delete.Table(_tableName);
    }
}