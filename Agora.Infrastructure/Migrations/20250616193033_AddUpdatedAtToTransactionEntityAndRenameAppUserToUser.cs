using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedAtToTransactionEntityAndRenameAppUserToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Transactions",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Posts",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Transactions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Posts",
                type: "datetime(6)",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);
        }
    }
}
