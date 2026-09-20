using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodOrderingAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFoodImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/margherita,pizza");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/vegetable,pizza");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/pepperoni,pizza");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/veggie,burger");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/chicken,burger");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/paneer,curry");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/biryani");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/lentil,curry");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/chocolate,brownie");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/gulabjamun");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 11,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/icedcoffee");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "https://loremflickr.com/400/300/lemonade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "images/margherita.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "images/farmhouse.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "images/pepperoni.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "images/veg-burger.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "images/zinger.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "images/paneer.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "images/biryani.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "images/dal.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "images/brownie.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "images/gulab-jamun.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 11,
                column: "ImageUrl",
                value: "images/cold-coffee.jpg");

            migrationBuilder.UpdateData(
                table: "FoodItems",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "images/lemonade.jpg");
        }
    }
}
