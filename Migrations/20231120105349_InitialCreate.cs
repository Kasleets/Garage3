using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Garage3.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalNumber = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberID);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    VehicleTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.VehicleTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountID);
                    table.ForeignKey(
                        name: "FK_Accounts_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleTypeID = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NumberOfWheels = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleID);
                    table.ForeignKey(
                        name: "FK_Vehicles_Members_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_VehicleTypeID",
                        column: x => x.VehicleTypeID,
                        principalTable: "VehicleTypes",
                        principalColumn: "VehicleTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParkingRecords",
                columns: table => new
                {
                    ParkingRecordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleID = table.Column<int>(type: "int", nullable: false),
                    ParkTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingRecords", x => x.ParkingRecordID);
                    table.ForeignKey(
                        name: "FK_ParkingRecords_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParkingRecords_Vehicles_VehicleID",
                        column: x => x.VehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberID", "Age", "FirstName", "LastName", "PersonalNumber" },
                values: new object[,]
                {
                    { 1, 73, "Alice", "Johnson", "19501230-1234" },
                    { 2, 43, "Bob", "Smith", "19800216-2345" },
                    { 3, 28, "Carol", "Davis", "19950721-3456" },
                    { 4, 60, "David", "Martinez", "19631005-4567" },
                    { 5, 46, "Eve", "Garcia", "19780819-5678" },
                    { 6, 22, "Frank", "Lee", "20011212-6789" }
                });

            migrationBuilder.InsertData(
                table: "VehicleTypes",
                columns: new[] { "VehicleTypeID", "TypeName" },
                values: new object[,]
                {
                    { 1, "Car" },
                    { 2, "Truck" },
                    { 3, "Motorcycle" },
                    { 4, "Bus" },
                    { 5, "Airplane" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "VehicleID", "Brand", "Color", "Model", "NumberOfWheels", "OwnerID", "RegistrationNumber", "VehicleTypeID" },
                values: new object[,]
                {
                    { 1, "Toyota", "Blue", "Corolla", 4, 1, "ABC123", 1 },
                    { 2, "Honda", "Red", "Civic", 4, 2, "XYZ789", 1 },
                    { 3, "Ford", "Green", "Fiesta", 4, 3, "DEF456", 1 },
                    { 4, "Volvo", "Black", "V70", 4, 4, "GHI789", 1 },
                    { 5, "Saab", "White", "900", 4, 5, "JKL012", 1 },
                    { 6, "Volkswagen", "Silver", "Golf", 4, 6, "MNO345", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_MemberID",
                table: "Accounts",
                column: "MemberID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRecords_MemberID",
                table: "ParkingRecords",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingRecords_VehicleID",
                table: "ParkingRecords",
                column: "VehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OwnerID",
                table: "Vehicles",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeID",
                table: "Vehicles",
                column: "VehicleTypeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "ParkingRecords");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "VehicleTypes");
        }
    }
}
