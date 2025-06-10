using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Transactions",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Description",
                value: "Le service a été réalisé et la transaction a été validée soit par les deux parties,soit uniquement par l'une d'entre elles si un délai de validation automatique s'est écoulé(par exemple X jours) sans objection de l'autre partie. Les points sont transférés de l'acheteur au vendeur.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Transactions");

            migrationBuilder.UpdateData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Description",
                value: "Le service a été effectué et validé par les deux partis. Les points sont transférés de l'acheteur au vendeur.");
        }
    }
}
