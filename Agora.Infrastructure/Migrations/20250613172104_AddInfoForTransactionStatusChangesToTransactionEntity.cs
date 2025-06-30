using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInfoForTransactionStatusChangesToTransactionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BuyerConfirmed",
                table: "Transactions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "InitiatorId",
                table: "Transactions",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "SellerConfirmed",
                table: "Transactions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerConfirmed",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "InitiatorId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SellerConfirmed",
                table: "Transactions");
        }
    }
}
