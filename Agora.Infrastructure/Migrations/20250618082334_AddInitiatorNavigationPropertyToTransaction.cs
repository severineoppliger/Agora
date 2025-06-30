using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInitiatorNavigationPropertyToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InitiatorId",
                table: "Transactions",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_InitiatorId",
                table: "Transactions",
                column: "InitiatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_InitiatorId",
                table: "Transactions",
                column: "InitiatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_InitiatorId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_InitiatorId",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "InitiatorId",
                table: "Transactions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
