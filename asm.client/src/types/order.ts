export interface Order {
  id: number;
  shippingAddress: string;
  status: OrderStatus;
  totalAmount: number;
  createdAt: string;
  updatedAt?: string;
  userId: string;
  discountAmount: string;
  paymentMethod: PaymentMethod;
  orderDetails?: OrderDetail[] | null;
}

export interface OrderDetail {
  id: number;
  quantity: number;
  unitPrice: number;
  productId?: number | null;
}

export enum PaymentMethod {
  COD = 1,
  Momo = 2,
}

export enum OrderStatus {
  Pending = 1, // làm món
  Shipping = 2, // đang giao hàng
  Delivered = 3, // đã giao
}
