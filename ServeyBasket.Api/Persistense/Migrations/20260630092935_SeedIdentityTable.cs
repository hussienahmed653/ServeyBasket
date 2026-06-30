using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ServeyBasket.Persistense.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019f1769-9fe4-74f3-b80c-372d3c1ba553", "019f1769-9fe4-74f3-b80c-372e73cd856b", false, false, "Admin", "ADMIN" },
                    { "019f1769-9fe4-74f3-b80c-372f1915db57", "019f1769-9fe4-74f3-b80c-3731a4ab27d2", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "019f1769-9fe4-74f3-b80c-372ae0d33020", 0, "019f1769-9fe4-74f3-b80c-372c165521c8", "admin@servey-basket.com", true, "ServeyBasket", "Admin", false, null, "ADMIN@SERVEY-BASKET.COM", "ADMIN@SERVEY-BASKET.COM", "AQAAAAIAAYagAAAAEGCP/sWoqFPurw4exJ63As57/UcuSDSnGkZLNaf49O+0PYzl3dZPfUdaNPRtgTMKxw==", null, false, "A5092A54E07E4F549D8640348058C178", false, "admin@servey-basket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permissions", "polls:read", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 2, "Permissions", "polls:add", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 3, "Permissions", "polls:update", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 4, "Permissions", "polls:delete", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 5, "Permissions", "questions:read", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 6, "Permissions", "questions:add", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 7, "Permissions", "questions:update", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 8, "Permissions", "users:read", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 9, "Permissions", "users:add", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 10, "Permissions", "users:update", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 11, "Permissions", "roles:read", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 12, "Permissions", "roles:add", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 13, "Permissions", "roles:update", "019f1769-9fe4-74f3-b80c-372d3c1ba553" },
                    { 14, "Permissions", "results:read", "019f1769-9fe4-74f3-b80c-372d3c1ba553" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "019f1769-9fe4-74f3-b80c-372d3c1ba553", "019f1769-9fe4-74f3-b80c-372ae0d33020" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019f1769-9fe4-74f3-b80c-372f1915db57");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019f1769-9fe4-74f3-b80c-372d3c1ba553", "019f1769-9fe4-74f3-b80c-372ae0d33020" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "019f1769-9fe4-74f3-b80c-372d3c1ba553");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "019f1769-9fe4-74f3-b80c-372ae0d33020");
        }
    }
}
