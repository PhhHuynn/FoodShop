export interface Dashboard {
  totalUsers: number;
  totalFoods: number;
  totalCombos: number;
  totalCategories: number;
  orders: OrderStat;
}

export interface OrderStat {
  pending: number;
  shipping: number;
  delivered: number;
}
