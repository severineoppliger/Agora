using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnumValueToTransactionStatusStringValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EnumValue",
                table: "TransactionsStatus",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EnumValue",
                value: "Pending");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EnumValue",
                value: "Accepted");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EnumValue",
                value: "Refused");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EnumValue",
                value: "Cancelled");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 5L,
                column: "EnumValue",
                value: "Failed");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 6L,
                column: "EnumValue",
                value: "PartiallyValidated");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 7L,
                column: "EnumValue",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 8L,
                column: "EnumValue",
                value: "InDispute");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 9L,
                column: "EnumValue",
                value: "ResolvedAccepted");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 10L,
                column: "EnumValue",
                value: "ResolvedCancelled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EnumValue",
                table: "TransactionsStatus",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
    }
}
