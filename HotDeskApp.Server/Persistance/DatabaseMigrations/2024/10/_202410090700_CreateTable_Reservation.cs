using FluentMigrator;

namespace HotDeskApp.Server.Persistance.DatabaseMigrations._2024._10;

[Migration(202410090700)]
public class _202410090700_CreateTable_Reservation : Migration
{
    private readonly string _tableName = "Reservation";

    public override void Up()
    {
        if (!Schema.Table(_tableName).Exists())
        {
            Create.Table(_tableName)
                .WithColumn(nameof(Models.Reservation.Entities.Reservation.Id)).AsGuid().NotNullable()
                .PrimaryKey()
                .WithColumn(nameof(Models.Reservation.Entities.Reservation.UserId)).AsGuid().NotNullable()
                .NotNullable()
                .WithColumn(nameof(Models.Reservation.Entities.Reservation.DeskId)).AsGuid().NotNullable()
                .NotNullable()
                .WithColumn(nameof(Models.Reservation.Entities.Reservation.StartDate)).AsDateTime().NotNullable()
                .NotNullable()
                .WithColumn(nameof(Models.Reservation.Entities.Reservation.EndDate)).AsDateTime().NotNullable()
                .NotNullable();
            Create.ForeignKey("FK_Reservation_User").FromTable("Reservation").ForeignColumn("UserId")
                .ToTable("UserEntity")
                .PrimaryColumn("Id");
            Create.ForeignKey("FK_Reservation_Desk").FromTable("Reservation").ForeignColumn("DeskId").ToTable("Desk")
                .PrimaryColumn("Id");
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