using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class emailsericeupdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GroupSendingProjectId",
                table: "EmailSendingStatuses",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailSendingStatuses_GroupSendingProjectId",
                table: "EmailSendingStatuses",
                column: "GroupSendingProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailSendingStatuses_GroupSendingProjects_GroupSendingProjectId",
                table: "EmailSendingStatuses",
                column: "GroupSendingProjectId",
                principalTable: "GroupSendingProjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailSendingStatuses_GroupSendingProjects_GroupSendingProjectId",
                table: "EmailSendingStatuses");

            migrationBuilder.DropIndex(
                name: "IX_EmailSendingStatuses_GroupSendingProjectId",
                table: "EmailSendingStatuses");

            migrationBuilder.DropColumn(
                name: "GroupSendingProjectId",
                table: "EmailSendingStatuses");
        }
    }
}
