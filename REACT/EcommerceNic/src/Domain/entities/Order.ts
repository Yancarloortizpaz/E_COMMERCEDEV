export interface OrderDetail {
  id: number;
  orderId: number;
  productVariableId: number;
  productVariableName?: string; // Mapped from related Tbl_ProductVariables for UI convenience
  productName?: string;         // Mapped from related Tbl_Products for UI convenience
  price: number;
  quantity: number;
  discount: number;
  subtotal: number;
  tax: number;
  total: number;
  currencyId: number;
  currencyISO?: string;         // Mapped from related Tbl_Currencies
}

export interface Order {
  id: number;
  userId: number;
  userFullName?: string;         // Mapped from related Tbl_Users
  deliveryAddressId: number;
  deliveryAddressDescription?: string; // Mapped from related Tbl_UserAddress
  paymentMethodId: number;
  paymentMethodName?: string;    // Mapped from related Tbl_PaymentMethodTypes
  subtotal: number;
  discount: number;
  shipping: number;
  tax: number;
  total: number;
  currencyId: number;
  currencyISO?: string;         // Mapped from related Tbl_Currencies
  creationDate: string;         // ISO string
  statusId: number;
  statusName?: string;          // Mapped from related Tbl_Status
  details?: OrderDetail[];
}
