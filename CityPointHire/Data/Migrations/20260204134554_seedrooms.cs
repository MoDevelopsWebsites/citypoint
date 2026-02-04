using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CityPointHire.Data.Migrations
{
    /// <inheritdoc />
    public partial class seedrooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserID",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomID", "Capacity", "Facilities", "Price", "RoomName" },
                values: new object[,]
                {
                    { 1, 8, "4K Display, Video Conferencing, Whiteboard", 45.00m, "Executive Boardroom" },
                    { 2, 40, "Projector, Sound System, Breakout Area", 65.00m, "Training Suite" },
                    { 3, 100, "Stage, Ambient Lighting, Catering Ready", 120.00m, "Grand Event Hall" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserID",
                table: "Bookings",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserID",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserID",
                table: "Bookings",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
