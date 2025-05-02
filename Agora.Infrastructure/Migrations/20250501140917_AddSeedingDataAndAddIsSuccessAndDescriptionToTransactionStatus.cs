using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Agora.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedingDataAndAddIsSuccessAndDescriptionToTransactionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TransactionsStatus",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "PostCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Cours d'appui" },
                    { 2L, "Covoiturage" },
                    { 3L, "Informatique" },
                    { 4L, "Arts et culture" },
                    { 5L, "Démarches administratives" },
                    { 6L, "Déménagement" },
                    { 7L, "Services du quotidien" },
                    { 8L, "Autres" }
                });

            migrationBuilder.InsertData(
                table: "TransactionsStatus",
                columns: new[] { "Id", "Description", "IsFinal", "IsSuccess", "Name" },
                values: new object[,]
                {
                    { 1L, "La demande d'échange a été initiée par un des deux utilisateurs mais pas encore acceptée par l'autre utilisateur.", false, false, "En attente" },
                    { 2L, "La demande d'échange a été acceptée par l'autre utilisateur. La transaction peut avoir lieu.", false, false, "Acceptée" },
                    { 3L, "La demande d'échange a été refusée par l'autre utilisateur. La transaction n'aura donc pas lieu.", true, false, "Refusée" },
                    { 4L, "La demande d'échange a été annulée par l’un des deux utilisateurs.", false, false, "Annulée" },
                    { 5L, "Le service n'a pas pu être réalisé, malgré la confirmation de la demande d'échange.", true, false, "Échouée" },
                    { 6L, "Le service est en train d’être réalisé.", false, false, "En cours" },
                    { 7L, "Le service a été réalisé et validé par un seul utilisateur, en attente de confirmation de l'autre.", false, false, "Partiellement validée" },
                    { 8L, "Le service a été effectué et validé par les deux parties. Les points sont transférés de l'acheteur au vendeur.", true, true, "Terminée" },
                    { 9L, "Le service a été effectué mais un désaccord a été signalé sur la transaction.", false, false, "En litige" },
                    { 10L, "Le litige a été résolu et les valeurs actuelles de la transaction ont été acceptées par les deux partis.", true, true, "Résolue et acceptée" },
                    { 11L, "La résolution du litige s'est soldé par l'annulation de la transaction.", true, false, "Résolue et annulée" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionsStatus_Name",
                table: "TransactionsStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCategories_Name",
                table: "PostCategories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TransactionsStatus_Name",
                table: "TransactionsStatus");

            migrationBuilder.DropIndex(
                name: "IX_PostCategories_Name",
                table: "PostCategories");

            migrationBuilder.DeleteData(
                table: "PostCategories",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "PostCategories",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "PostCategories",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "PostCategories",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "PostCategories",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "PostCategories",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "PostCategories",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "PostCategories",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "TransactionsStatus",
                keyColumn: "Id",
                keyValue: 11L);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TransactionsStatus");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
