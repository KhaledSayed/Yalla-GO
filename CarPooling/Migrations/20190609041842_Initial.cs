using System;
using GeoAPI.Geometries;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarPooling.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Location = table.Column<IPoint>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    SaltPassword = table.Column<byte[]>(nullable: true),
                    HashPassword = table.Column<byte[]>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Activated = table.Column<bool>(nullable: true),
                    Location = table.Column<IPoint>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    LastOnlineAt = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartLocationId = table.Column<int>(nullable: true),
                    FinalLocationId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DriverId = table.Column<int>(nullable: true),
                    ClientId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trips_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trips_Places_FinalLocationId",
                        column: x => x.FinalLocationId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trips_Places_StartLocationId",
                        column: x => x.StartLocationId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatTrips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TripId = table.Column<int>(nullable: true),
                    TridId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    DriverId = table.Column<int>(nullable: true),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatTrips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatTrips_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatTrips_Users_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChatTrips_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientTrips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    TripId = table.Column<int>(nullable: false),
                    FromLocationId = table.Column<int>(nullable: true),
                    ToLocationId = table.Column<int>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    Distance = table.Column<string>(nullable: true),
                    SuggestedPrice = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    StartedAt = table.Column<DateTime>(nullable: false),
                    LeavedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTrips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientTrips_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientTrips_Places_FromLocationId",
                        column: x => x.FromLocationId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientTrips_Places_ToLocationId",
                        column: x => x.ToLocationId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientTrips_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Location = table.Column<IPoint>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false),
                    TripId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripPoints_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ChatTrips_ClientId",
                table: "ChatTrips",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatTrips_DriverId",
                table: "ChatTrips",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatTrips_TripId",
                table: "ChatTrips",
                column: "TripId");

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
                name: "IX_ClientTrips_ClientId",
                table: "ClientTrips",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTrips_FromLocationId",
                table: "ClientTrips",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTrips_ToLocationId",
                table: "ClientTrips",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientTrips_TripId",
                table: "ClientTrips",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Connection_DriverId",
                table: "Connection",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPoints_TripId",
                table: "TripPoints",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_ClientId",
                table: "Trips",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_DriverId",
                table: "Trips",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_FinalLocationId",
                table: "Trips",
                column: "FinalLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_StartLocationId",
                table: "Trips",
                column: "StartLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatTrips");

            migrationBuilder.DropTable(
                name: "ClientInTripPoints");

            migrationBuilder.DropTable(
                name: "Connection");

            migrationBuilder.DropTable(
                name: "TripPoints");

            migrationBuilder.DropTable(
                name: "ClientTrips");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Places");
        }
    }
}
