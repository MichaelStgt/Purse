---
name: Precision Dark
colors:
  surface: '#0b1326'
  surface-dim: '#0b1326'
  surface-bright: '#31394d'
  surface-container-lowest: '#060e20'
  surface-container-low: '#131b2e'
  surface-container: '#171f33'
  surface-container-high: '#222a3d'
  surface-container-highest: '#2d3449'
  on-surface: '#dae2fd'
  on-surface-variant: '#bdc8d1'
  inverse-surface: '#dae2fd'
  inverse-on-surface: '#283044'
  outline: '#87929a'
  outline-variant: '#3e484f'
  surface-tint: '#7bd0ff'
  primary: '#8ed5ff'
  on-primary: '#00354a'
  primary-container: '#38bdf8'
  on-primary-container: '#004965'
  inverse-primary: '#00668a'
  secondary: '#b9c8de'
  on-secondary: '#233143'
  secondary-container: '#39485a'
  on-secondary-container: '#a7b6cc'
  tertiary: '#c2cde5'
  on-tertiary: '#263143'
  tertiary-container: '#a7b2c9'
  on-tertiary-container: '#394458'
  error: '#ffb4ab'
  on-error: '#690005'
  error-container: '#93000a'
  on-error-container: '#ffdad6'
  primary-fixed: '#c4e7ff'
  primary-fixed-dim: '#7bd0ff'
  on-primary-fixed: '#001e2c'
  on-primary-fixed-variant: '#004c69'
  secondary-fixed: '#d4e4fa'
  secondary-fixed-dim: '#b9c8de'
  on-secondary-fixed: '#0d1c2d'
  on-secondary-fixed-variant: '#39485a'
  tertiary-fixed: '#d8e3fb'
  tertiary-fixed-dim: '#bcc7de'
  on-tertiary-fixed: '#111c2d'
  on-tertiary-fixed-variant: '#3c475a'
  background: '#0b1326'
  on-background: '#dae2fd'
  surface-variant: '#2d3449'
typography:
  headline-xl:
    fontFamily: Hanken Grotesk
    fontSize: 48px
    fontWeight: '700'
    lineHeight: 56px
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Hanken Grotesk
    fontSize: 32px
    fontWeight: '600'
    lineHeight: 40px
    letterSpacing: -0.01em
  headline-lg-mobile:
    fontFamily: Hanken Grotesk
    fontSize: 24px
    fontWeight: '600'
    lineHeight: 32px
  body-md:
    fontFamily: Inter
    fontSize: 16px
    fontWeight: '400'
    lineHeight: 24px
  body-sm:
    fontFamily: Inter
    fontSize: 14px
    fontWeight: '400'
    lineHeight: 20px
  label-mono:
    fontFamily: JetBrains Mono
    fontSize: 12px
    fontWeight: '500'
    lineHeight: 16px
    letterSpacing: 0.05em
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  unit: 4px
  gutter: 16px
  margin-mobile: 16px
  margin-desktop: 32px
  container-max: 1280px
---

## Brand & Style

The design system is a high-performance, dark-themed environment for financial management. It targets professionals who require deep focus, high readability, and a sense of institutional security. The aesthetic is a blend of **Corporate Modern** and **Minimalism**, emphasizing clarity and precision over decorative elements. 

The user experience should feel analytical and calm. By utilizing a deep, non-pure-black background, the interface reduces eye strain during long sessions while allowing vibrant status indicators to guide the user's attention to critical data movements.

## Colors

This design system utilizes a sophisticated dark palette anchored by a Charcoal/Navy base.

- **Surface Layering**: The primary background is `#0f172a`. Elevated containers use `#1e293b` to create a hierarchical distinction.
- **Accents**: The primary brand color is a bright Sky Blue, providing high contrast against the dark background. 
- **Functional Colors**: Status indicators are highly saturated. Income (Success) uses vibrant green, Expenses (Error) uses crisp red, and Pending (Warning) uses a warm amber.
- **Typography Contrast**: Primary text is nearly white (`#f8fafc`) to ensure maximum legibility, while secondary text is muted to a cool slate.

## Typography

Typography is systematic and functional. **Hanken Grotesk** provides a sharp, contemporary feel for headlines, reflecting professional precision. **Inter** is used for all body text to maintain high legibility in dense data environments. **JetBrains Mono** is reserved for financial figures, transaction IDs, and tabular data to ensure character alignment and a technical "fintech" feel.

On mobile devices, headline sizes scale down significantly to preserve screen real estate, while body text remains constant to ensure accessibility.

## Layout & Spacing

The design system employs a **Fluid Grid** model based on a 4px baseline shift. 

- **Desktop**: A 12-column grid with 16px gutters. Outer margins are set to 32px or centered with a max-width of 1280px.
- **Tablet**: An 8-column grid with 16px gutters.
- **Mobile**: A 4-column grid with 16px margins. 

Spacing between unrelated sections should use 32px or 48px increments, while grouped elements (like form fields) should use 8px or 12px.

## Elevation & Depth

Depth in this dark mode environment is achieved through **Tonal Layers** and **Subtle Outlines** rather than heavy shadows.

- **Base Layer**: `#0f172a` (Default background).
- **Raised Layer**: `#1e293b` (Used for cards and sidebars).
- **Interactive Layer**: `#334155` (Used for hover states on list items).
- **Borders**: All containers feature a 1px solid border of `#1e293b`. For active or focused states, the border transitions to the primary sky blue or a 50% opacity variant.
- **Shadows**: Shadows are avoided unless used for transient elements like Modals or Popovers, in which case they are ultra-dark (`#000000` at 40% opacity) with a large blur radius (24px) to create a subtle glow-void effect.

## Shapes

The shape language is **Soft** and restrained. A 4px radius (`0.25rem`) is the standard for most components (buttons, inputs, smaller cards), communicating a balance between technical rigidity and modern approachability. Larger dashboard widgets may use the `rounded-lg` (8px) setting for a slightly smoother aesthetic.

## Components

- **Buttons**: Primary buttons use a solid sky-blue fill with dark text for maximum contrast. Secondary buttons are outlined with a light slate border.
- **Input Fields**: Backgrounds use the `#1e293b` surface with a subtle border. Focus states must trigger a high-contrast sky-blue ring.
- **Cards**: Cards should not have shadows; instead, use a slightly lighter background than the canvas and a thin border.
- **Chips**: Use for transaction categories. They feature low-opacity background tints of the status colors (e.g., Green at 10% opacity) with high-contrast text labels.
- **Lists**: Financial transaction lists use alternating row colors or subtle dividers (`#1e293b`). Hover states use a lighter slate tint.
- **Data Visualizations**: Charts should use a palette of sky blue, emerald, and amber, ensuring they are the brightest elements on the page.