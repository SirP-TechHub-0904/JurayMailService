using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatepatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailResponseStatuses_EmailLists_EmailListId",
                table: "EmailResponseStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailSendingStatuses_EmailProjects_EmailProjectId",
                table: "EmailSendingStatuses");

            migrationBuilder.RenameColumn(
                name: "EmailListId",
                table: "EmailResponseStatuses",
                newName: "EmailProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_EmailResponseStatuses_EmailListId",
                table: "EmailResponseStatuses",
                newName: "IX_EmailResponseStatuses_EmailProjectId");

            migrationBuilder.AlterColumn<long>(
                name: "EmailProjectId",
                table: "EmailSendingStatuses",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "EmailResponseStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EmailResponseStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailResponseStatuses_EmailProjects_EmailProjectId",
                table: "EmailResponseStatuses",
                column: "EmailProjectId",
                principalTable: "EmailProjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailSendingStatuses_EmailProjects_EmailProjectId",
                table: "EmailSendingStatuses",
                column: "EmailProjectId",
                principalTable: "EmailProjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailResponseStatuses_EmailProjects_EmailProjectId",
                table: "EmailResponseStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_EmailSendingStatuses_EmailProjects_EmailProjectId",
                table: "EmailSendingStatuses");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "EmailResponseStatuses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "EmailResponseStatuses");

            migrationBuilder.RenameColumn(
                name: "EmailProjectId",
                table: "EmailResponseStatuses",
                newName: "EmailListId");

            migrationBuilder.RenameIndex(
                name: "IX_EmailResponseStatuses_EmailProjectId",
                table: "EmailResponseStatuses",
                newName: "IX_EmailResponseStatuses_EmailListId");

            migrationBuilder.AlterColumn<long>(
                name: "EmailProjectId",
                table: "EmailSendingStatuses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailResponseStatuses_EmailLists_EmailListId",
                table: "EmailResponseStatuses",
                column: "EmailListId",
                principalTable: "EmailLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailSendingStatuses_EmailProjects_EmailProjectId",
                table: "EmailSendingStatuses",
                column: "EmailProjectId",
                principalTable: "EmailProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
