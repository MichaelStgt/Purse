---
name: Precision Finance
colors:
  surface: '#f7f9fb'
  surface-dim: '#d8dadc'
  surface-bright: '#f7f9fb'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f2f4f6'
  surface-container: '#eceef0'
  surface-container-high: '#e6e8ea'
  surface-container-highest: '#e0e3e5'
  on-surface: '#191c1e'
  on-surface-variant: '#45464d'
  inverse-surface: '#2d3133'
  inverse-on-surface: '#eff1f3'
  outline: '#76777d'
  outline-variant: '#c6c6cd'
  surface-tint: '#565e74'
  primary: '#000000'
  on-primary: '#ffffff'
  primary-container: '#131b2e'
  on-primary-container: '#7c839b'
  inverse-primary: '#bec6e0'
  secondary: '#0058be'
  on-secondary: '#ffffff'
  secondary-container: '#2170e4'
  on-secondary-container: '#fefcff'
  tertiary: '#000000'
  on-tertiary: '#ffffff'
  tertiary-container: '#002113'
  on-tertiary-container: '#009668'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#dae2fd'
  primary-fixed-dim: '#bec6e0'
  on-primary-fixed: '#131b2e'
  on-primary-fixed-variant: '#3f465c'
  secondary-fixed: '#d8e2ff'
  secondary-fixed-dim: '#adc6ff'
  on-secondary-fixed: '#001a42'
  on-secondary-fixed-variant: '#004395'
  tertiary-fixed: '#6ffbbe'
  tertiary-fixed-dim: '#4edea3'
  on-tertiary-fixed: '#002113'
  on-tertiary-fixed-variant: '#005236'
  background: '#f7f9fb'
  on-background: '#191c1e'
  surface-variant: '#e0e3e5'
  expense-red: '#EF4444'
  income-green: '#10B981'
  transfer-blue: '#6366F1'
  surface-border: '#E2E8F0'
  data-viz-amber: '#F59E0B'
typography:
  display-lg:
    fontFamily: Hanken Grotesk
    fontSize: 48px
    fontWeight: '700'
    lineHeight: 56px
    letterSpacing: -0.02em
  headline-md:
    fontFamily: Hanken Grotesk
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
  headline-sm:
    fontFamily: Hanken Grotesk
    fontSize: 18px
    fontWeight: '600'
    lineHeight: 24px
  body-lg:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-md:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  label-caps:
    fontFamily: Inter
    fontSize: 12px
    fontWeight: '600'
    lineHeight: 16px
    letterSpacing: 0.05em
  data-mono:
    fontFamily: JetBrains Mono
    fontSize: 14px
    fontWeight: '500'
    lineHeight: 20px
  headline-md-mobile:
    fontFamily: Hanken Grotesk
    fontSize: 20px
    fontWeight: '600'
    lineHeight: 28px
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  unit: 4px
  container-padding: 24px
  gutter: 16px
  stack-sm: 8px
  stack-md: 16px
  stack-lg: 32px
---

## Brand & Style

The design system is built for a high-utility financial environment where clarity and speed of information processing are paramount. The brand personality is **authoritative, precise, and reassuring**, aimed at users who manage complex personal or business finances and require a tool that feels like a professional instrument rather than a toy.

The design style is **Corporate Modern with a focus on Information Density**. It utilizes a refined "Systematic Minimalist" approach:
- **Functional Clarity:** Generous whitespace is used strategically to separate data groups, not just for aesthetics.
- **Structural Integrity:** Using subtle tonal layering and crisp geometry to define boundaries without visual clutter.
- **Micro-Interactions:** Subtle haptic-like transitions that provide immediate feedback during complex tasks like transaction splitting or budget adjustments.
- **Intentionality:** Every element on the screen serves a purpose for data entry or analysis, evoking a sense of calm control over one's capital.

## Colors

The color palette is anchored by a deep "Midnight Navy" (`#0F172A`) to establish authority and trust. The primary action color is a vibrant "Precision Blue" (`#3B82F6`), which provides high visibility for interactive elements.

- **Semantic Logic:** Financial status is immediately identifiable through a strict semantic system. Income and positive growth use a refined Emerald (`#10B981`), while expenses and alerts use a sharp Coral Red (`#EF4444`). 
- **Neutral Foundation:** We use a cool-toned Slate scale for backgrounds (`#F8FAFC`) and borders (`#E2E8F0`) to maintain a clean, airy feel that prevents the UI from feeling heavy despite high data density.
- **Data Visualization:** A secondary palette of Indigo and Amber is reserved for charts and categorization to ensure categorical distinction during complex split-transaction views.

## Typography

Typography in this design system is optimized for **legibility and tabular alignment**. 

- **Headlines:** **Hanken Grotesk** provides a clean, modern, and slightly technical feel for major headings and balance displays.
- **Body:** **Inter** is used for all UI text and inputs due to its exceptional readability at small sizes and high x-height.
- **Data/Monospaced:** **JetBrains Mono** is strategically employed for numerical data, transaction lists, and currency amounts. This ensures that decimal points align perfectly in vertical columns, making financial auditing effortless for the user.
- **Scale:** On mobile devices, headline sizes are reduced to ensure large currency figures do not wrap awkwardly.

## Layout & Spacing

The layout utilizes a **12-column Fixed Grid** for desktop (max-width 1280px) and a **Fluid 4-column Grid** for mobile. 

The spacing rhythm is based on a **4px baseline grid**. 
- **Information Density:** For complex data entry like split transactions, the spacing compresses to "stack-sm" (8px) to allow more line items to be visible on a single screen without scrolling. 
- **Safe Areas:** Broad margins (24px) are maintained at the screen edges to provide "visual breathing room" against the technical nature of the content.
- **Mobile Reflow:** For split-transaction entry on mobile, the layout shifts from a horizontal row (Amount | Category | Note) to a stacked card format to ensure touch targets remain large and accessible.

## Elevation & Depth

This design system avoids heavy shadows in favor of **Tonal Layering and Low-Contrast Outlines**. 

- **Surface Tiers:** The base background is the lightest neutral. Primary content containers (like a bank account card) are pure white with a 1px border (`#E2E8F0`). 
- **Interaction Depth:** Active states for inputs and cards use a subtle "Ambient Lift"—a very soft, 4px blur shadow with 5% opacity—to indicate focus without breaking the flat, professional aesthetic.
- **Modal Overlays:** For transaction splitting, a backdrop blur (12px) is used to dim the background, keeping the user’s focus entirely on the arithmetic of the split.

## Shapes

The shape language is **Soft (0.25rem)**. This provides a professional, structured appearance that feels modern but remains "serious." 

- **Interactive Elements:** Buttons and Input fields use a 4px corner radius.
- **Containers:** Large dashboard cards use "rounded-lg" (8px) to create a clear distinction between the page background and the content modules.
- **Data Tags:** Small category chips use a 100px "pill" radius to provide a visual break from the otherwise rectangular grid of financial data.

## Components

- **Buttons:** Primary buttons are solid "Midnight Navy" or "Precision Blue." Secondary buttons use a ghost style with a 1px border. The "Add Split" action in transaction views uses a subtle dashed-border button style to indicate an additive process.
- **Input Fields:** Fields utilize a "Float Label" pattern to maintain context even when filled. For currency entry, the currency symbol is fixed at the lead, and font-weight for the value is increased to 600.
- **Transaction Splitter:** A specialized component that uses a vertical "tree" line to visually connect split items to the parent transaction, using a "data-mono" font for the math.
- **Chips:** Categorization chips use low-saturation background tints of their assigned color (e.g., a light green background for "Income" with dark green text) to ensure they don't compete with primary actions.
- **Cards:** Used as the primary data container. Cards are flat with a 1px border. No shadows are used unless the card is being "dragged" or reordered.
- **Data Tables:** Row heights are optimized at 48px for high density. On hover, rows highlight in a very faint blue (`#F1F5F9`) to help the eye track across complex financial rows.