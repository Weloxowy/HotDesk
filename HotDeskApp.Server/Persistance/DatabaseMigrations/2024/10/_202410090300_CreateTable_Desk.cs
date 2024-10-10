using FluentMigrator;

namespace HotDeskApp.Server.Persistance.DatabaseMigrations._2024._10;

[Migration(202410090300)]
public class _202410090300_CreateTable_Desk : Migration
{
    private readonly string _tableName = "Desk";

    public override void Up()
    {
        if (!Schema.Table(_tableName).Exists())
        {
            Create.Table(_tableName)
                .WithColumn(nameof(Models.Desk.Entities.Desk.Id)).AsGuid().NotNullable()
                .PrimaryKey()
                .WithColumn(nameof(Models.Desk.Entities.Desk.Name)).AsString()
                .NotNullable()
                .WithColumn(nameof(Models.Desk.Entities.Desk.Description)).AsString()
                .NotNullable()
                .WithColumn(nameof(Models.Desk.Entities.Desk.IsMaintnance)).AsBoolean()
                .NotNullable()
                .WithColumn(nameof(Models.Desk.Entities.Desk.LocationId)).AsGuid()
                .NotNullable();
            /*
            Create.ForeignKey("FK_Desk_Location").FromTable("Desk").ForeignColumn("LocationId").ToTable("Location")
                .PrimaryColumn("Id");
                */
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