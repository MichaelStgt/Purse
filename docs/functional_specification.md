# Purse App: Functional Specification & Product Design

## 1. Product Vision
"Purse" is a local-first, privacy-centric personal finance tracker designed to provide granular financial awareness through frictionless manual entry, precise receipt splitting, and proactive budget tracking.

## 2. Core User Workflows

### 2.1 Dashboard & Immediate Insights
- **The "Safe to Spend" View:** The landing page immediately answers "How much money do I have left?" by subtracting upcoming scheduled bills and savings goals from the current balance.
- **Dynamic Filtering:** Users can filter the dashboard view by timeframe (Current Month, Last 30 Days, Year-to-Date) and by specific categories to isolate spending trends.
- **Category Budget Overview:** A visual summary showing spend vs. income vs. remaining budget for active categories.

### 2.2 Frictionless & Comprehensive Data Entry
- **Rich Media Attachments:** Users can attach context to any transaction by taking a photo of a paper receipt, uploading a file (e.g., a PDF rental contract), or attaching a screenshot of a digital receipt (e.g., from a supermarket app).
- **Contextual Notes:** Every transaction supports free-text notes for future searchability.

### 2.3 Advanced Split Transactions (The Real-World Receipt)
- **Multi-Category Splitting:** A single transaction (e.g., a supermarket run) can be split into multiple line items assigned to different categories (e.g., Groceries, Household Goods).
- **Mixed-Sign Line Items:** Users can combine expenses and income within the same receipt. For example, logging a €50 Grocery expense and a €3 Bottle Deposit (*Pfand*) return or coupon deduction in a single transaction record, ensuring category budgets are perfectly accurate.

### 2.4 Subscriptions & Recurring Bills
- **Subscription Tracking:** Users can log recurring expenses (Netflix, Rent) or recurring income (Salary). 
- **Upcoming Alerts:** The app anticipates these recurring items and factors them into the dashboard's "Safe to Spend" calculations to prevent accidental overspending.

### 2.5 Security & Future-Proofing
- **Biometric Lock:** To protect financial privacy, the app prompts for device-native biometric authentication (Fingerprint/FaceID) upon launch.
- **Multi-Currency Support:** (Future Phase) Ability to log transactions in foreign currencies during travel, maintaining a separate balance or automatically converting to the base currency.

### 2.6 Visual Indicators & Net Calculations
- **Net Amount Display:** Transactions display a single, net calculated total.
- **Color & Iconography:** Visual styling is driven dynamically by the net amount.
  - **Positive Net (Income):** Displayed in 'Basil' (Green) with an upward-pointing Material Symbol icon.
  - **Negative Net (Expense):** Displayed in 'Tomato' (Red) with a downward-pointing Material Symbol icon.
- **Implementation:** This is strictly handled via XAML `IValueConverter` implementations, keeping UI logic out of the ViewModels.

### 2.7 Transaction Line Item Management
- **Modal Editing:** Adding or editing a `TransactionLineItem` must be handled via a modal Popup (using the MAUI Community Toolkit), keeping the user anchored to the main `TransactionDetailView`.
- **Inline Category Creation:** The Category selection utilizes an editable combobox (Syncfusion `SfComboBox`). If a user types a category that does not exist, the UI will display inline toggle buttons (Expense / Income) within the same popup. Selecting a type instantly creates the category in the background without forcing the user to navigate to a separate creation page.

### 2.8 Design Philosophy: Graceful Optionality
- **Zero Forced Friction:** The user may use advanced functionality, but must never be forced to if it is not strictly necessary.
- **Sensible Defaults:** When a user creates a Category inline during a transaction, the system will automatically assign a neutral default color and a generic default icon. Customization of these visual elements is strictly opt-in and will be handled later via dedicated settings or master data views, ensuring the transaction entry flow remains completely uninterrupted.
