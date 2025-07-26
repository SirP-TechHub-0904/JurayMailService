using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedplan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed Plans
            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Name", "Price", "MonthlyLimit" },
                values: new object[,]
                {
                    { 1, "Free Plan", 0m, 50 },
                    { 2, "Pro Plan", 2000m, 500 },
                    { 3, "Premium Plan", 4500m, 1000 }
                });

            // Seed PlanFeatures
            migrationBuilder.InsertData(
                table: "PlanFeatures",
                columns: new[] { "Id", "PlanId", "Description", "IsHighlighted" },
                values: new object[,]
                {
                    // Free Plan Features
                    { 1, 1, "50 Emails Monthly", true },
                    { 2, 1, "50 SMS Monthly", true },
                    { 3, 1, "Use @@jbuzz.africa Sender", true },
                    { 4, 1, "Basic Logs & Stats", true },

                    // Pro Plan Features
                    { 5, 2, "500 Emails Monthly", true },
                    { 6, 2, "500 SMS Monthly", true },
                    { 7, 2, "Custom SMTP Allowed", true },
                    { 8, 2, "Delivery Reports Included", true },

                    // Premium Plan Features
                    { 9, 3, "1,000 Emails Monthly", true },
                    { 10, 3, "1,000 SMS Monthly", true },
                    { 11, 3, "Unlimited Campaigns", true },
                    { 12, 3, "Priority Support", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete features first
            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValues: Enumerable.Range(1, 12).Cast<object>().ToArray());

            // Then delete plans
            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3 });
        }
    }
}
