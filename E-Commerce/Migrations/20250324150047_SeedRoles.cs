using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Define the GUIDs for the roles
            var userRoleId = Guid.NewGuid().ToString();
            var adminRoleId = Guid.NewGuid().ToString();
            var instructorRoleId = Guid.NewGuid().ToString();

            // Insert the User role
            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { userRoleId, "User", "USER", Guid.NewGuid().ToString() }
            );

            // Insert the Admin role
            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { adminRoleId, "Admin", "ADMIN", Guid.NewGuid().ToString() }
            );

            migrationBuilder.InsertData(
                schema: "security",
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] { instructorRoleId, "Supplier", "SUPPLIER", Guid.NewGuid().ToString() }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete the User role
            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Admin"
            );

            // Delete the Admin role
            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Name",
                keyValue: "User"
            );

            migrationBuilder.DeleteData(
                schema: "security",
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Supplier"
            );

        }
    }
}
