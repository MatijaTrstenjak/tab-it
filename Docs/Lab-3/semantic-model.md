# Tab-It - Semantic Database Model

## Models & Tables

1. **Role**
   - **Properties:** `Id` (PK), `Name`, `Description`
   - **Relations:** 1-N with `User` (`Role.Users`)

2. **User**
   - **Properties:** `Id` (PK), `FirstName`, `LastName`, `Username`, `Email`, `PasswordHash`, `CreatedAt`, `IsActive`, `RoleId` (FK)
   - **Relations:** 
     - N-1 with `Role` (`User.Role`)
     - 1-N with `CustomerTab` (`User.Tabs`)

3. **ProductCategory**
   - **Properties:** `Id` (PK), `Name`, `Description`
   - **Relations:** 1-N with `Product` (`ProductCategory.Products`)

4. **Product**
   - **Properties:** `Id` (PK), `Name`, `Sku`, `UnitPrice`, `IsAlcoholic`, `AvailableQuantity`, `LastRestockedAt`, `ProductCategoryId` (FK)
   - **Relations:**
     - N-1 with `ProductCategory` (`Product.Category`)
     - 1-N with `OrderItem` (`Product.OrderItems`) - *Forms logical bridge for N-N with Order*

5. **CustomerTab**
   - **Properties:** `Id` (PK), `TabCode`, `TableNumber`, `OpenedAt`, `ClosedAt`, `Status`, `Notes`, `OpenedByUserId` (FK)
   - **Relations:**
     - N-1 with `User` (`CustomerTab.OpenedByUser`)
     - 1-N with `Order` (`CustomerTab.Orders`)

6. **Order**
   - **Properties:** `Id` (PK), `OrderNumber`, `OrderedAt`, `Status`, `Subtotal`, `DiscountPercent`, `Total`, `CustomerTabId` (FK)
   - **Relations:**
     - N-1 with `CustomerTab` (`Order.CustomerTab`)
     - 1-N with `OrderItem` (`Order.Items`) - *Forms logical bridge for N-N with Product*

7. **OrderItem**
   - **Properties:** `Id` (PK), `Quantity`, `UnitPrice`, `LineTotal`, `ItemNote`, `OrderId` (FK), `ProductId` (FK)
   - **Relations:**
     - N-1 with `Order` (`OrderItem.Order`)
     - N-1 with `Product` (`OrderItem.Product`)
