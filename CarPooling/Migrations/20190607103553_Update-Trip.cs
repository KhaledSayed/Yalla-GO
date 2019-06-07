using Microsoft.EntityFrameworkCore.Migrations;

namespace CarPooling.Migrations
{
    public partial class UpdateTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_Users_DriverId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Car_CarId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_CarId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Car_DriverId",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Trips");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TripId1",
                table: "TripPoints",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CarId",
                table: "Users",
                column: "CarId",
                unique: true,
                filter: "[CarId] IS NOT NULL");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Car_CarId",
                table: "Users",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripPoints_Trips_TripId1",
                table: "TripPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Car_CarId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CarId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TripPoints_TripId1",
                table: "TripPoints");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TripId1",
                table: "TripPoints");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Trips",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CarId",
                table: "Trips",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_DriverId",
                table: "Car",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Users_DriverId",
                table: "Car",
                column: "DriverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Car_CarId",
                table: "Trips",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
