using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixingBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Severity",
                table: "NotificationPreferences",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "NotificationPreferences",
                keyColumn: "Id",
                keyValue: new Guid("10203a03-79bb-4d68-925d-ce6114ea0fad"),
                column: "Severity",
                value: "Information");

            migrationBuilder.UpdateData(
                table: "NotificationPreferences",
                keyColumn: "Id",
                keyValue: new Guid("6f71ffff-9acc-4c7b-8553-5e82664864a0"),
                column: "Severity",
                value: "Information");

            migrationBuilder.UpdateData(
                table: "NotificationPreferences",
                keyColumn: "Id",
                keyValue: new Guid("8bc1eebf-f71d-46ec-b5b5-fd86553d6efe"),
                column: "Severity",
                value: "Warning");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Severity",
                table: "NotificationPreferences",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "NotificationPreferences",
                keyColumn: "Id",
                keyValue: new Guid("10203a03-79bb-4d68-925d-ce6114ea0fad"),
                column: "Severity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "NotificationPreferences",
                keyColumn: "Id",
                keyValue: new Guid("6f71ffff-9acc-4c7b-8553-5e82664864a0"),
                column: "Severity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "NotificationPreferences",
                keyColumn: "Id",
                keyValue: new Guid("8bc1eebf-f71d-46ec-b5b5-fd86553d6efe"),
                column: "Severity",
                value: 1);
        }
    }
}
