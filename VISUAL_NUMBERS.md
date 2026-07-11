# Visual Numbers — Dot-Based Counting System

A visual counting system that represents numeric values using distinct dot shapes, scaling over 10+ orders of magnitude. Each shape corresponds to a power of 10. Up to 9 dots of each shape are drawn, representing the digit at that magnitude (like place-value notation made visual).

## Shape Table

| Magnitude | Value         | Shape    | Description                       |
|-----------|---------------|----------|-----------------------------------|
| 10^0      | 1             | SmallDot | Filled small circle               |
| 10^1      | 10            | Circle   | Hollow circle outline             |
| 10^2      | 100           | Square   | Hollow square outline             |
| 10^3      | 1,000         | Diamond  | 45° rotated square outline        |
| 10^4      | 10,000        | Triangle | Equilateral triangle outline      |
| 10^5      | 100,000       | Star     | 4-ray asterisk (8 lines)          |
| 10^6      | 1,000,000     | Pentagon | 5-sided polygon outline           |
| 10^7      | 10,000,000    | Hexagon  | 6-sided polygon outline           |
| 10^8      | 100,000,000   | Cross    | Plus-sign shaped outline          |
| 10^9      | 1,000,000,000 | Ring     | Double-circle (thick ring)        |
| 10^10     | 10,000,000,000| Octagon  | 8-sided polygon outline           |

## Display Grid

Dots are always displayed in a **3×3 grid** (9 cells maximum). Dots fill the grid left-to-right, top-to-bottom, from highest magnitude to lowest. If the full decomposition would exceed 9 dots, the least-significant dots are omitted (the value is effectively rounded down).

### Reading Example

The value **4,321** would be displayed as:

```
◆◆◆◆ ■■■ ○○
 4×   3×  2×
1000  100  10
```

(The single units dot is omitted to fit within 9 cells: 4+3+2 = 9.)

The value **321** (6 dots total) would be displayed as:

```
■■■ ○○ •
3×  2×  1×
100  10  1
```

(Only 6 of 9 cells used; no rounding needed.)

## Minimum Display Rule

Any positive value always displays at least one SmallDot. This ensures the player can distinguish "zero" (empty grid) from "non-zero but small" (single dot). Values in the range (0, 1) show a single SmallDot.

## Design Rationale

- **Distinct shapes**: Each magnitude has a unique silhouette, readable at a glance without counting.
- **Digit repetition**: Repeating a shape 1–9 times gives the digit at that place value.
- **No text needed**: Works at any zoom level and doesn't require font rendering.
- **Graceful scaling**: A value of 10 billion shows as a single octagon; a value of 5 shows as 5 tiny dots. The visual complexity grows logarithmically with the value.
- **Color-coded**: All dots render in a consistent green (#B4FFB4) to associate with the alien biomass theme.

## Usage

```csharp
// Decompose a value into a list of shapes
List<DotShape> dots = DotCounter.Decompose(biomassValue);

// Draw a single dot shape at screen coordinates
DotCounter.DrawDot(renderer, shape, centerX, centerY, size);
```

The `GlobeRenderer` draws dots at each infected country's centroid, with the dot count reflecting that zone's current biomass.

## Maximum Representable Value

With 9 dots per magnitude across 11 magnitudes: 99,999,999,999 (~100 billion). Values beyond this cap each digit at 9.
