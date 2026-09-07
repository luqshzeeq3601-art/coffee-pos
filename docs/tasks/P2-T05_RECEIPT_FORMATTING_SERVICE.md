# Task Capsule: P2-T05 — 80mm ESC/POS Receipt Formatting & Digital Receipt Service

## 1. Objective & Observable Outcomes
- **Objective:** Implement hardware-grade 80mm ESC/POS byte-level command formatting, Malaysian SST tax compliance receipts, digital receipt DTOs, and the Roast Ledger `ReceiptView` thermal component.
- **Outcome:** Complete receipt rendering pipeline across backend APIs (`CoffeePos.ApiHost`), infrastructure print builder (`CoffeePos.Infrastructure`), contracts (`@coffee-pos/contracts`), and UI design system (`packages/ui`).

## 2. Files & Ownership
- `packages/contracts/src/receipts.ts`: Exported `ReceiptDto`, `ReceiptPrintPayloadDto`, `ReceiptLineItemDto`, `ReceiptTaxSummaryDto`.
- `packages/contracts/src/index.ts`: Barrel export.
- `services/api/src/CoffeePos.Application/DTOs/ReceiptDtos.cs`: Application DTO records.
- `services/api/src/CoffeePos.Application/Interfaces/IReceiptServices.cs`: `IReceiptFormatter`, `IReceiptService` interfaces.
- `services/api/src/CoffeePos.Infrastructure/Printing/EscPosBuilder.cs`: Low-level ESC/POS byte generator (init, bold, alignment, cut, drawer kick).
- `services/api/src/CoffeePos.Infrastructure/Printing/EscPosReceiptFormatter.cs`: Business formatting for 80mm receipts and plaintext receipts.
- `services/api/src/CoffeePos.Infrastructure/Services/ReceiptService.cs`: Orchestrates receipt data retrieval and ESC/POS payload generation.
- `services/api/src/CoffeePos.ApiHost/Endpoints/ReceiptEndpoints.cs`: Minimal API endpoints (`GET /api/v1/receipts/{transactionId}`, `GET /api/v1/receipts/{transactionId}/escpos`).
- `services/api/src/CoffeePos.ApiHost/Program.cs`: Dependency injection registration and endpoint mapping.
- `packages/ui/src/components/ReceiptView.tsx` & `ReceiptView.css`: 80mm thermal receipt visual preview component.
- `packages/ui/src/gallery/ComponentGallery.tsx`: Added Section 08 for `ReceiptView`.
- `packages/ui/src/index.ts`: Barrel export.

## 3. Interfaces & Invariants
- **Fixed Width:** 48 monospace characters per line for 80mm thermal paper.
- **Malaysian SST Compliance:** Full merchant header (SSM, SST ID), tax breakdown (6% SST), sequence, and QR verification link.
- **Drawer Kick:** Pin 2 standard ESC/POS pulse (`ESC p 0 25 250`).

## 4. Verification Evidence
- TypeScript compiler across 5 packages: `bun x tsc` returned code 0 (0 errors).
- Architectural boundaries check: `npm run check:boundaries` passed.
- .NET project references check: `pwsh scripts/check-dotnet-references.ps1` passed.
