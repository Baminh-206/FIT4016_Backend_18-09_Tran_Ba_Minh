using System.ComponentModel.DataAnnotations;

namespace OrderManagementApp;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [RegularExpression(@"^ORD-\d{8}-\d{4}$",
        ErrorMessage = "Order Number phải có dạng ORD-YYYYMMDD-XXXX")]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Product? Product { get; set; }
}
