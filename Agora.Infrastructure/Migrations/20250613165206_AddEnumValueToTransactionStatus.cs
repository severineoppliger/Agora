using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnumValueToTransactionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnumValue",
                table: "TransactionsStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EnumValue",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EnumValue",
                value: 3);

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EnumValue",
                value: 4);

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EnumValue",
                value: 2);

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 5L,
                column: "EnumValue",
                value: 5);

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 6L,
                column: "EnumValue",
                value: 6);

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 7L,
                column: "EnumValue",
                value: 7);

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 8L,
                column: "EnumValue",
                value: 8);

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 9L,
                column: "EnumValue",
                value: 9);

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 10L,
                column: "EnumValue",
                value: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnumValue",
                table: "TransactionsStatus");
        }
    }
}
