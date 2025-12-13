export interface Cart {
  cartDetails: CartDetail[];
}

export interface CartDetail {
  id: number;
  quantity: number;
  price: number;
  productName: string;
  productId: number;
}

export interface CartDetailCreateOrUpdate {
  quantity: number;
  productId: number;
}
