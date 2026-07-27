using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NotificationService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedNotificationPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NotificationPreferences",
                columns: new[] { "Id", "NotificationType", "Severity" },
                values: new object[,]
                {
                    { new Guid("10203a03-79bb-4d68-925d-ce6114ea0fad"), "ProductCreated", 0 },
                    { new Guid("6f71ffff-9acc-4c7b-8553-5e82664864a0"), "FileUploaded", 0 },
                    { new Guid("8bc1eebf-f71d-46ec-b5b5-fd86553d6efe"), "LowStock", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationPreferences",
                keyColumn: "Id",
                keyValue: new Guid("10203a03-79bb-4d68-925d-ce6114ea0fad"));

            migrationBuilder.DeleteData(
                table: "NotificationPreferences",
                keyColumn: "Id",
                keyValue: new Guid("6f71ffff-9acc-4c7b-8553-5e82664864a0"));

            migrationBuilder.DeleteData(
                table: "NotificationPreferences",
                keyColumn: "Id",
                keyValue: new Guid("8bc1eebf-f71d-46ec-b5b5-fd86553d6efe"));
        }
    }
}
