using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CityPointHire.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomID", "Capacity", "Facilities", "Price", "RoomName" },
                values: new object[,]
                {
                    { 4, 15, "Interactive Screens, Flexible Seating", 55.00m, "Innovation Lab" },
                    { 5, 20, "Smart Board, Natural Lighting", 50.00m, "Creative Studio" },
                    { 6, 12, "HD Display, Conference Phone", 40.00m, "Conference Room A" },
                    { 7, 10, "Whiteboard, HDMI Setup", 35.00m, "Conference Room B" },
                    { 8, 25, "Projector, Flip Charts", 60.00m, "Workshop Room 1" },
                    { 9, 30, "Dual Screens, Sound System", 70.00m, "Workshop Room 2" },
                    { 10, 60, "Tiered Seating, AV System", 90.00m, "Seminar Hall" },
                    { 11, 4, "Private Space, Desk Setup", 20.00m, "Interview Room 1" },
                    { 12, 4, "Quiet Zone, Office Desk", 20.00m, "Interview Room 2" },
                    { 13, 18, "Modular Tables, Smart TV", 55.00m, "Team Collaboration Hub" },
                    { 14, 14, "Wall Whiteboards, Screen Casting", 48.00m, "Strategy Room" },
                    { 15, 35, "Comfort Seating, Refreshment Area", 75.00m, "Networking Lounge" },
                    { 16, 150, "Stage, Full AV, Lighting Rig", 150.00m, "Auditorium" },
                    { 17, 12, "Recording Equipment, Editing PC", 65.00m, "Digital Media Room" },
                    { 18, 6, "Premium Seating, Smart Display", 50.00m, "Executive Meeting Room" },
                    { 19, 45, "Open Plan, Projector", 80.00m, "Community Space" },
                    { 20, 3, "Confidential Setup, Desk & Chairs", 25.00m, "Private Consultation Room" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "RoomID",
                keyValue: 20);
        }
    }
}
