export interface User {
  email: string;
  password: string;
  status: UserStatus;
  role: string;
  fullName?: string;
  address?: string;
}

export interface LoginUser {
  email: string;
  password: string;
}

export enum UserStatus {
  Active = 1, // người dùng hoạt động bình thường
  Inactive = 2, // khách tạm nghỉ hoặc bạn khóa tài khoản do yêu cầu của họ (xóa nhưng ko muốn mất dữ liệu
  Banned = 3, // khóa tài khoản vi phạm (bom hàng,...)
  Pending = 4,
}

export interface AuthResponse {
  token: string;
  user: {
    id: string;
    email: string;
    name: string;
  };
}
