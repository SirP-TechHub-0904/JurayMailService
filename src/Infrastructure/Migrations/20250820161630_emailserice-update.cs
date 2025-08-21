using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class emailsericeupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailSendingStatuses_EmailLists_EmailListId",
                table: "EmailSendingStatuses");

            migrationBuilder.DropIndex(
                name: "IX_EmailSendingStatuses_EmailListId",
                table: "EmailSendingStatuses");

            migrationBuilder.DropColumn(
                name: "EmailListId",
                table: "EmailSendingStatuses");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "EmailSendingStatuses");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "EmailSendingStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EmailSendingStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "EmailSendingStatuses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "EmailSendingStatuses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "EmailSendingStatuses");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "EmailSendingStatuses");

            migrationBuilder.AddColumn<long>(
                name: "EmailListId",
                table: "EmailSendingStatuses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "EmailSendingStatuses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EmailSendingStatuses_EmailListId",
                table: "EmailSendingStatuses",
                column: "EmailListId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailSendingStatuses_EmailLists_EmailListId",
                table: "EmailSendingStatuses",
                column: "EmailListId",
                principalTable: "EmailLists",
                principalColumn: "Id");
        }
    }
}
