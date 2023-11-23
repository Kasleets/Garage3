using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage3.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedParkingRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "VehicleTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Vehicles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "ParkingRecords",
                columns: new[] { "ParkingRecordID", "CheckOutTime", "MemberID", "ParkTime", "VehicleID" },
                values: new object[,]
                {
                    { 1, null, 1, new DateTime(2023, 11, 22, 19, 23, 19, 698, DateTimeKind.Local).AddTicks(1178), 1 },
                    { 2, null, 2, new DateTime(2023, 11, 22, 21, 23, 19, 698, DateTimeKind.Local).AddTicks(1183), 2 },
                    { 3, null, 3, new DateTime(2023, 11, 20, 21, 23, 19, 698, DateTimeKind.Local).AddTicks(1186), 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkingRecords",
                keyColumn: "ParkingRecordID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ParkingRecords",
                keyColumn: "ParkingRecordID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ParkingRecords",
                keyColumn: "ParkingRecordID",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "TypeName",
                table: "VehicleTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RegistrationNumber",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
