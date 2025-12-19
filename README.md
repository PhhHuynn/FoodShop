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

Backend: ASP.NET Core 8 (Web API)
Frontend: Vue.js 3 (SPA)
Database: SQL Server + Entity Framework Core (Code-First với Migration)
Authentication: JWT + Google OAuth
Realtime Chat: SignalR (hoặc công nghệ bạn dùng cho chat realtime)

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

#### 2. Cấu hình Database
   
Mở file asm.api/appsettings.json, chỉnh sửa ConnectionStrings phù hợp với SQL Server local của bạn.

Chạy migration để tạo database:
```Bash
cd asm.api
dotnet ef database update
```

#### 3. Chạy Backend
```Bash
cd asm.api
dotnet run
```
#### 4. Chạy FrontendBashcd asm.client
```
npm install
npm run dev
```
#### 5. Truy cập ứng dụng
Mở browser vào địa chỉ frontend
API sẽ được gọi tự động từ frontend.


## Tác giả 
Huyền

Dự án Assignment môn .NET1062 - FPT Polytechnic
