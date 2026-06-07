## 📊 **Complete Example: Creating a Product Variant with Attributes**

### **Scenario:**
Product: **Laptop**
- Attribute: **Color** (AttributeId = 1)
- Attribute Value: **Red** (AttributeValueId = 1)

---

## **STEP-BY-STEP DATA FLOW**

### **STEP 1: Initial Setup (Already Done)**

**ProductAttributes Table:**
```
| Id | ProductId | Name  |
|----|-----------|-------|
| 1  | 5         | Color |
```

**AttributeValues Table:**
```
| Id | ProductAttributeId | Value |
|----|-------------------|-------|
| 1  | 1                 | Red   |
```

**Products Table:**
```
| Id | Name   | MerchantId | BasePrice | Status | CreatedAt           | UpdatedAt           |
|----|--------|------------|-----------|--------|---------------------|---------------------|
| 5  | Laptop | 1          | 999.99    | Active | 2024-06-06 10:00:00 | 2024-06-06 10:00:00 |
```

---

### **STEP 2: Create a Product Variant**

**HTTP Request:**
```http
POST /api/variants
Authorization: Bearer {valid_jwt_token}
Content-Type: application/json

{
  "productId": 5,
  "sku": "LAPTOP-RED-001",
  "quantity": 50,
  "priceOverride": 899.99,
  "isActive": true
}
```

**What Happens in CreateProductVariantCommandHandler:**

1. ✅ Get current user from context
2. ✅ Validate user is authenticated
3. ✅ Fetch product (Id = 5) with its merchant
4. ✅ Verify user owns this product (Merchant.UserId == user.Id)
5. ✅ Check if SKU "LAPTOP-RED-001" already exists (must be unique)
6. ✅ Create new ProductVariant object:

```csharp
var productVariant = new ProductVariant
{
    ProductId = 5,
    SKU = "LAPTOP-RED-001",
    Quantity = 50,
    PriceOverride = 899.99,
    IsActive = true,
    CreatedAt = DateTime.UtcNow,      // 2024-06-06 12:30:45.123
    UpdatedAt = DateTime.UtcNow       // 2024-06-06 12:30:45.123
};
```

7. ✅ Save to database
8. ✅ Return variant Id (e.g., 10)

---

### **STEP 3: Database State After Creating Variant**

**ProductVariants Table (NEW ROW ADDED):**
```
| Id | ProductId | SKU                | Quantity | PriceOverride | IsActive | CreatedAt                 | UpdatedAt                 |
|----|-----------|--------------------|-----------|-----------|----|---------------------------|---------------------------|
| 1  | 5         | LAPTOP-BLUE-001    | 30        | NULL      | 1  | 2024-06-06 10:15:00.000  | 2024-06-06 10:15:00.000  |
| 2  | 5         | LAPTOP-BLACK-001   | 40        | 799.99    | 1  | 2024-06-06 11:00:00.000  | 2024-06-06 11:00:00.000  |
| 10 | 5         | LAPTOP-RED-001     | 50        | 899.99    | 1  | 2024-06-06 12:30:45.123  | 2024-06-06 12:30:45.123  | ← NEW VARIANT
```

**VariantAttributeValues Table (STILL EMPTY FOR THIS VARIANT):**
```
| Id | ProductVariantId | AttributeValueId |
|----|------------------|------------------|
| 1  | 1                | 2                | (Blue variant has Green color)
| 2  | 1                | 3                | (Blue variant has Small size)
| 3  | 2                | 1                | (Black variant has Red color)
| 4  | 2                | 4                | (Black variant has Large size)
|    |                  |                  | ← NO RECORD FOR LAPTOP-RED-001 YET
```

**Key Point:** 
- ✅ **ProductVariants row is created immediately**
- ❌ **VariantAttributeValues is NOT auto-filled**
- You need to manually link attribute values to the variant

---

### **STEP 4: Link Variant to Attribute Value**

**To connect the Red color to the newly created variant, you need a NEW ENDPOINT:**

*This endpoint doesn't exist yet in your code. You need to create it.*

**Proposed Request:**
```http
POST /api/variantattributevalues
Authorization: Bearer {valid_jwt_token}
Content-Type: application/json

{
  "productVariantId": 10,
  "attributeValueId": 1
}
```

**What Should Happen:**

1. Create a VariantAttributeValue record:
```csharp
var variantAttributeValue = new VariantAttributeValue
{
    ProductVariantId = 10,      // The Red laptop variant
    AttributeValueId = 1        // Red color
};
await _dbContext.VariantAttributeValues.AddAsync(variantAttributeValue);
await _dbContext.SaveChangesAsync();
```

2. **VariantAttributeValues Table (NOW UPDATED):**
```
| Id | ProductVariantId | AttributeValueId |
|----|------------------|------------------|
| 1  | 1                | 2                |
| 2  | 1                | 3                |
| 3  | 2                | 1                |
| 4  | 2                | 4                |
| 5  | 10               | 1                | ← NEW LINK: Red laptop variant has Red color attribute
```

---

## **📋 Summary Table: Data Relationships**

### **Scenario: Red Laptop Variant with Different Sizes**

If you want the Red Laptop to have multiple sizes (Small, Medium, Large), you'd need to:

1. Create attribute: **Size**
2. Create attribute values: **Small**, **Medium**, **Large**
3. Create ONE variant: **LAPTOP-RED-001**
4. Link it to multiple attribute values:

**VariantAttributeValues Table (Final State):**
```
| Id | ProductVariantId | AttributeValueId | Meaning                |
|----|------------------|------------------|------------------------|
| 5  | 10               | 1                | Red laptop has Red     |
| 6  | 10               | 5                | Red laptop has Small   |
| 7  | 10               | 6                | Red laptop has Medium  |
| 8  | 10               | 7                | Red laptop has Large   |
```

**Query Example:**
```sql
-- Get all attribute values for variant LAPTOP-RED-001
SELECT av.* 
FROM VariantAttributeValues vav
JOIN AttributeValues av ON vav.AttributeValueId = av.Id
WHERE vav.ProductVariantId = 10;

-- Result:
-- Red, Small, Medium, Large
```

---

## **🔄 Data Flow Diagram**

```
Product "Laptop"
    ↓
ProductAttribute "Color"
    ↓
AttributeValue "Red"
    ↓
ProductVariant "LAPTOP-RED-001"  ← Created by your endpoint
    ↓
VariantAttributeValue (Link) ← NOT auto-created, needs manual linking
```

---

## **❌ What Gets Auto-Filled in ProductVariants Table:**

| Column | Auto-Filled? | How |
|--------|-------------|-----|
| `CreatedAt` | ✅ YES | `GETUTCDATE()` SQL default |
| `UpdatedAt` | ✅ YES | `GETUTCDATE()` SQL default |
| `IsActive` | ❌ NO | Uses value from command (defaults to `true` in config) |
| `Quantity` | ❌ NO | Uses value from command (defaults to `0` in config) |
| `PriceOverride` | ❌ NO | Uses value from command (can be NULL) |
| `SKU` | ❌ NO | Must be provided by user |
| `ProductId` | ❌ NO | Must be provided by user |

---

## **⚠️ VariantAttributeValue Table - IMPORTANT**

**VariantAttributeValue is NOT auto-filled with any data.**

It has:
- **Unique Constraint:** `(ProductVariantId, AttributeValueId)` - prevents duplicate attribute assignment
- **No auto-fill fields** - must be manually populated

```csharp
// Example: What happens if you try to create duplicate
VariantAttributeValue vav1 = new() 
{ ProductVariantId = 10, AttributeValueId = 1 };

VariantAttributeValue vav2 = new() 
{ ProductVariantId = 10, AttributeValueId = 1 };

// If you try to add both, database will throw:
// "Cannot insert duplicate key row in object 'dbo.VariantAttributeValues'"
```

---

## **🎯 What You Need to Implement**

To complete the variant creation flow, you should create:

1. **CreateVariantAttributeValueCommand** - Link attribute value to variant
2. **VariantAttributeValuesController** - Handle the linking endpoint
3. **GetVariantWithAttributesQuery** - Fetch variant with all its attribute values

---

## **📝 Complete Example JSON Flow**

```json
// Step 1: Create Variant
POST /api/variants
{
  "productId": 5,
  "sku": "LAPTOP-RED-001",
  "quantity": 50,
  "priceOverride": 899.99,
  "isActive": true
}
Response: { "id": 10 }

// Step 2: Link Red Color to Variant
POST /api/variantattributevalues
{
  "productVariantId": 10,
  "attributeValueId": 1
}
Response: { "id": 5 }

// Step 3: Link Small Size to Variant
POST /api/variantattributevalues
{
  "productVariantId": 10,
  "attributeValueId": 5
}
Response: { "id": 6 }

// Step 4: Get Variant with All Attributes
GET /api/variants/10/with-attributes
Response: {
  "id": 10,
  "productId": 5,
  "sku": "LAPTOP-RED-001",
  "quantity": 50,
  "priceOverride": 899.99,
  "isActive": true,
  "createdAt": "2024-06-06T12:30:45.123Z",
  "updatedAt": "2024-06-06T12:30:45.123Z",
  "variantAttributes": [
    { "id": 5, "attributeValueId": 1, "value": "Red" },
    { "id": 6, "attributeValueId": 5, "value": "Small" }
  ]
}
```

---

