# CookStore - Ứng dụng đặt món ăn online

Ứng dụng web bán đồ ăn nhanh, cho phép khách hàng duyệt menu, lọc sản phẩm, thêm vào giỏ hàng, thanh toán và nhắn tin với nhân viên để xin hỗ trợ. Phía admin có dashboard đầy đủ để quản lý sản phẩm, đơn hàng, loại món, người dùng và theo dõi cuộc hội thoại.

### Trang chủ (khách hàng)

![Trang chủ](https://github.com/user-attachments/assets/88ef5b55-9dbb-477d-8003-86dff4cdf5fa)

### Trang đăng nhập

![Trang đăng nhập](https://github.com/user-attachments/assets/61c737f1-d72d-4dc1-8fe1-b0625af4f334)

### Dashboard quản lý Admin

![Dashboard Admin](https://github.com/user-attachments/assets/3b621537-9960-4279-9bda-fac8cba0ec80)


## Tính năng chính
### Phía khách hàng

- Đăng ký / Đăng nhập (hỗ trợ Google Login)
- Cập nhật hồ sơ, đổi mật khẩu
- Lọc và tìm kiếm sản phẩm theo loại, giá,...
- Thêm/sửa/xóa sản phẩm trong giỏ hàng
- Thanh toán đơn hàng
- Chat realtime với nhân viên hỗ trợ

### Phía Admin / Nhân viên

Dashboard tổng quan:
- Quản lý sản phẩm (CRUD)
- Quản lý loại món ăn
- Quản lý người dùng
- Quản lý đơn hàng và hóa đơn
- Theo dõi và trả lời chat từ khách hàng


## Công nghệ sử dụng 

- Backend: ASP.NET Core 8 (Web API)
- Frontend: Vue.js 3 (SPA)
- Database: SQL Server + Entity Framework Core (Code-First với Migration)
- Authentication: JWT + Google OAuth
- Realtime Chat: SignalR (hoặc công nghệ bạn dùng cho chat realtime)

## Cài đặt & Chạy local 
### Yêu cầu

- .NET 8 SDK: https://dotnet.microsoft.com/download
- Node.js 18+: https://nodejs.org
- Microsoft SQL Server (SQL Server Express hoặc LocalDB khuyến nghị cho local)

### Các bước

#### 1. Clone repository
   
```Bash
clone https://github.com/PhhHuynn/FoodShop.git
cd foodshop-net1062
```

#### 2. Tạo file cấu hình local
   
Tạo file ASM.Server/appsettings.json

Mở file và điền các thông tin sau:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "AllowedHosts": "*",

  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=NET106-ASM;Trusted_Connection=True;TrustServerCertificate=True;"
  },

  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE",
    "DurationInMinutes": 60
  },

  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    }
  },

  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "RealIpHeader": "X-Real-IP",
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 60
      }
    ]
  }
}

```

#### 3. Chạy Backend
```Bash
cd asm.api
dotnet run
```
#### 4. Chạy Frontend
```Bash
cd asm.client
npm install
npm run dev
```
#### 5. Truy cập ứng dụng
Mở browser vào địa chỉ frontend
API sẽ được gọi tự động từ frontend.

