using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarPooling.Migrations
{
    public partial class UpdateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripPoints_Trips_TripId1",
                table: "TripPoints");

            migrationBuilder.DropIndex(
                name: "IX_TripPoints_TripId1",
                table: "TripPoints");

            migrationBuilder.DropColumn(
                name: "Driver_Activated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TripId1",
                table: "TripPoints");

            migrationBuilder.AddColumn<IPoint>(
                name: "Location",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Places",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "ClientInTripPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Location = table.Column<IPoint>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    TripId = table.Column<int>(nullable: false),
                    ClientTripId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInTripPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientInTripPoints_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientInTripPoints_ClientTrips_ClientTripId",
                        column: x => x.ClientTripId,
                        principalTable: "ClientTrips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientInTripPoints_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Connection",
                columns: table => new
                {
                    ConnectionID = table.Column<string>(nullable: false),
                    UserAgent = table.Column<string>(nullable: true),
                    Connected = table.Column<bool>(nullable: false),
                    DriverId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connection", x => x.ConnectionID);
                    table.ForeignKey(
                        name: "FK_Connection_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientInTripPoints_ClientId",
                table: "ClientInTripPoints",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInTripPoints_ClientTripId",
                table: "ClientInTripPoints",
                column: "ClientTripId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInTripPoints_TripId",
                table: "ClientInTripPoints", 
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Connection_DriverId",
                table: "Connection",
                column: "DriverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientInTripPoints");

            migrationBuilder.DropTable(
                name: "Connection");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Users");

            migrationBuilder.AddColumn<bool>(
                name: "Driver_Activated",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TripId1",
                table: "TripPoints",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Places",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TripPoints_TripId1",
                table: "TripPoints",
                column: "TripId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TripPoints_Trips_TripId1",
                table: "TripPoints",
                column: "TripId1",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
