using System.Text;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;
using CoffeePos.Domain.Orders;
using CoffeePos.Domain.Payments;

namespace CoffeePos.Infrastructure.Printing;

public sealed class EscPosReceiptFormatter : IReceiptFormatter
{
    private const int LineWidth = 48; // Standard 80mm thermal paper width

    public ReceiptDto BuildReceiptDto(SalesTransaction transaction, Order order)
    {
        var items = order.Items.Select(i => new ReceiptLineItemDto(
            i.Name,
            i.Quantity,
            i.UnitPrice,
            i.LineTotal,
            i.Modifiers.Select(m => m.Name).ToList())).ToList();

        var taxes = new List<ReceiptTaxSummaryDto>
        {
            new ReceiptTaxSummaryDto(
                "SST (6%)",
                6.00m,
                order.Subtotal - order.DiscountTotal,
                transaction.TaxTotal)
        };

        var payments = transaction.Payments.Select(p => new ReceiptPaymentDto(
            p.PaymentMethod.ToString(),
            p.Amount,
            p.ReferenceCode)).ToList();

        return new ReceiptDto(
            transaction.ReceiptNumber,
            order.OrderNumber,
            "ARTISAN ROAST CO.",
            "SSM: 202401012345 (1234567-A)",
            "SST: W10-2024-001234",
            "Bangsar Flagship",
            "No. 12, Jalan Telawi 3, Bangsar, 59100 Kuala Lumpur",
            "+60 3-2287 1234",
            transaction.CashierName,
            "POS Terminal 01",
            transaction.CreatedAtUtc,
            order.DiningOption.ToString(),
            order.TableOrCustomer,
            items,
            transaction.Subtotal,
            transaction.DiscountTotal,
            taxes,
            transaction.GrandTotal,
            payments,
            transaction.PaidAmount,
            transaction.ChangeAmount,
            $"https://verify.artisanroast.com/rcp/{transaction.ReceiptNumber}",
            "Thank you for brewing with us!");
    }

    public ReceiptPrintPayloadDto GenerateEscPosPayload(ReceiptDto receipt, bool kickDrawer = false)
    {
        var builder = new EscPosBuilder();
        builder.Initialize();

        if (kickDrawer)
        {
            builder.KickDrawer();
        }

        // 1. Header
        builder.AlignCenter()
            .SetBold(true)
            .SetDoubleHeight(true)
            .AppendLine(receipt.MerchantName)
            .SetDoubleHeight(false)
            .SetBold(false)
            .AppendLine(receipt.CompanyRegistrationNumber)
            .AppendLine($"SST ID: {receipt.SstRegistrationNumber}")
            .AppendLine(receipt.OutletName)
            .AppendLine(receipt.OutletAddress)
            .AppendLine($"Tel: {receipt.OutletPhone}")
            .AppendDivider('=');

        // 2. Transaction Meta
        builder.AlignLeft()
            .AppendTwoColumnRow($"Receipt: {receipt.ReceiptNumber}", $"Order: {receipt.OrderNumber}")
            .AppendTwoColumnRow($"Date: {receipt.TransactionTimeUtc:yyyy-MM-dd HH:mm}", $"Type: {receipt.DiningOption}")
            .AppendTwoColumnRow($"Cashier: {receipt.CashierName}", receipt.TableOrCustomer != null ? $"Table: {receipt.TableOrCustomer}" : "")
            .AppendDivider('-');

        // 3. Line Items
        builder.AppendLine("Item                           Qty      Total (RM)")
            .AppendDivider('-');

        foreach (var item in receipt.Items)
        {
            var leftPart = $"{item.Quantity}x {item.Name}";
            if (leftPart.Length > 36) leftPart = leftPart.Substring(0, 36);
            var rightPart = item.LineTotal.ToString("F2");
            builder.AppendTwoColumnRow(leftPart, rightPart);

            if (item.Modifiers != null && item.Modifiers.Count > 0)
            {
                foreach (var mod in item.Modifiers)
                {
                    builder.AppendLine($"  + {mod}");
                }
            }
        }

        builder.AppendDivider('-');

        // 4. Totals & Taxes
        builder.AppendTwoColumnRow("Subtotal", $"RM {receipt.Subtotal:F2}")
            .AppendTwoColumnRow("SST (6%)", $"RM {receipt.TaxSummary.Sum(t => t.TaxAmount):F2}");

        if (receipt.DiscountsTotal > 0)
        {
            builder.AppendTwoColumnRow("Discount", $"-RM {receipt.DiscountsTotal:F2}");
        }

        builder.SetBold(true)
            .AppendTwoColumnRow("TOTAL", $"RM {receipt.GrandTotal:F2}")
            .SetBold(false)
            .AppendDivider('-');

        // 5. Payment Tender & Change
        foreach (var pay in receipt.Payments)
        {
            builder.AppendTwoColumnRow($"Tender ({pay.Method})", $"RM {pay.Amount:F2}");
        }

        if (receipt.ChangeGiven > 0)
        {
            builder.AppendTwoColumnRow("Change Given", $"RM {receipt.ChangeGiven:F2}");
        }

        builder.AppendDivider('=');

        // 6. Footer & Verification URL
        builder.AlignCenter()
            .AppendLine("Scan to Verify / Digital E-Receipt:")
            .AppendLine(receipt.QrVerificationUrl ?? "")
            .AppendLine("")
            .AppendLine(receipt.FooterNotes ?? "Thank You!")
            .FeedAndCut(4);

        var bytes = builder.ToByteArray();
        var base64 = builder.ToBase64();
        var plainText = GeneratePlainTextReceipt(receipt);

        return new ReceiptPrintPayloadDto(
            receipt.ReceiptNumber,
            base64,
            plainText,
            bytes.Length,
            kickDrawer);
    }

    public string GeneratePlainTextReceipt(ReceiptDto receipt)
    {
        var sb = new StringBuilder();
        sb.AppendLine("================================================");
        sb.AppendLine($"           {receipt.MerchantName}           ");
        sb.AppendLine($"         {receipt.CompanyRegistrationNumber}         ");
        sb.AppendLine($"          SST: {receipt.SstRegistrationNumber}          ");
        sb.AppendLine($"             {receipt.OutletName}             ");
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine($"Receipt: {receipt.ReceiptNumber}  Order: {receipt.OrderNumber}");
        sb.AppendLine($"Date: {receipt.TransactionTimeUtc:yyyy-MM-dd HH:mm}  Cashier: {receipt.CashierName}");
        if (!string.IsNullOrEmpty(receipt.TableOrCustomer))
        {
            sb.AppendLine($"Reference: {receipt.TableOrCustomer} ({receipt.DiningOption})");
        }
        sb.AppendLine("------------------------------------------------");

        foreach (var item in receipt.Items)
        {
            var line = $"{item.Quantity}x {item.Name}".PadRight(38) + item.LineTotal.ToString("F2").PadLeft(10);
            sb.AppendLine(line);
            if (item.Modifiers != null)
            {
                foreach (var mod in item.Modifiers)
                {
                    sb.AppendLine($"  ↳ {mod}");
                }
            }
        }

        sb.AppendLine("------------------------------------------------");
        sb.AppendLine("Subtotal".PadRight(38) + receipt.Subtotal.ToString("F2").PadLeft(10));
        sb.AppendLine("SST (6%)".PadRight(38) + receipt.TaxSummary.Sum(t => t.TaxAmount).ToString("F2").PadLeft(10));
        sb.AppendLine("GRAND TOTAL".PadRight(38) + $"RM {receipt.GrandTotal:F2}".PadLeft(10));
        sb.AppendLine("================================================");
        sb.AppendLine($"Paid: RM {receipt.TotalPaid:F2}  Change: RM {receipt.ChangeGiven:F2}");
        sb.AppendLine("           " + (receipt.FooterNotes ?? "Thank you!") + "           ");
        sb.AppendLine("================================================");

        return sb.ToString();
    }
}
