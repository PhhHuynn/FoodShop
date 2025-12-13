export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  isAvailable: boolean;
  createdAt: string;
  updatedAt?: string | null;
  deletedAt?: string | null;
  averageRating: number;
  fImageFile?: File | null;
}
