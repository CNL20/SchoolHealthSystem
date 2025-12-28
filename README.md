# SchoolHealthSystem - Hệ thống Quản lý Sức khỏe Học sinh

## 📋 Tổng quan

**SchoolHealthSystem** là hệ thống quản lý sức khỏe học sinh toàn diện được xây dựng bằng **ASP.NET Core 9.0** theo mô hình **Clean Architecture**. Hệ thống cung cấp các chức năng quản lý thông tin học sinh, hồ sơ sức khỏe, tiêm chủng, thuốc và kho thuốc một cách chuyên nghiệp.

## 🏗️ Kiến trúc hệ thống

### Clean Architecture Pattern

```
┌─────────────────────────────────────────────────┐
│                Controllers                       │ ← API Layer
├─────────────────────────────────────────────────┤
│                 Services                        │ ← Business Logic Layer
├─────────────────────────────────────────────────┤
│               Repositories                      │ ← Data Access Layer
├─────────────────────────────────────────────────┤
│            Models & DTOs                        │ ← Data Layer
├─────────────────────────────────────────────────┤
│            Entity Framework                     │ ← Database Layer
└─────────────────────────────────────────────────┘
```

### Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server với Entity Framework Core
- **Authentication**: JWT Bearer Token
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **API Documentation**: Swagger/OpenAPI

## 👥 Hệ thống phân quyền

### Các vai trò (Roles)

1. **🔧 Admin**: Quản trị viên hệ thống
2. **👔 Manager**: Quản lý y tế
3. **👩‍⚕️ Nurse**: Y tá
4. **👨‍👩‍👧‍👦 Parent**: Phụ huynh học sinh

### Ma trận quyền hạn

| Chức năng              | Admin | Manager    | Nurse | Parent     |
| ---------------------- | ----- | ---------- | ----- | ---------- |
| Quản lý học sinh       | ✅    | ✅         | ✅    | ❌         |
| Xem thông tin học sinh | ✅    | ✅         | ✅    | ✅(con em) |
| Tạo hồ sơ sức khỏe     | ✅    | ✅         | ✅    | ❌         |
| Xem hồ sơ sức khỏe     | ✅    | ✅         | ✅    | ✅(con em) |
| Yêu cầu thuốc          | ❌    | ❌         | ❌    | ✅         |
| Duyệt thuốc            | ✅    | ✅         | ✅    | ❌         |
| Lên lịch tiêm chủng    | ✅    | ✅         | ✅    | ❌         |
| Thực hiện tiêm chủng   | ✅    | ✅         | ✅    | ❌         |
| Quản lý kho thuốc      | ✅    | ✅         | ✅    | ❌         |
| Xóa dữ liệu            | ✅    | ✅(một số) | ❌    | ❌         |

## 🎯 Các Module chính

### 1. 👨‍🎓 Student Management Module

#### Chức năng

- Quản lý thông tin học sinh (CRUD)
- Tự động tạo mã học sinh (StudentCode)
- Tìm kiếm học sinh theo multiple criteria
- Soft delete và hard delete
- Quản lý trạng thái hoạt động

#### Luồng hoạt động

```
[Create Student] → [Validate Input] → [Auto Generate StudentCode] → [Save to DB] → [Return Response]
[Search] → [Repository Query] → [Filter by criteria] → [Return Results]
[Soft Delete] → [Mark IsActive = false] → [Hide from normal queries]
[Hard Delete] → [Remove from database] (Admin only)
```

#### API Endpoints

| Method | Endpoint                         | Description          | Permission          |
| ------ | -------------------------------- | -------------------- | ------------------- |
| POST   | `/api/students`                  | Tạo học sinh mới     | Admin,Manager,Nurse |
| GET    | `/api/students`                  | Lấy tất cả học sinh  | All                 |
| GET    | `/api/students/{id}`             | Lấy học sinh theo ID | All                 |
| PUT    | `/api/students/{id}`             | Cập nhật thông tin   | Admin,Manager,Nurse |
| DELETE | `/api/students/{id}`             | Xóa học sinh         | Admin,Manager,Nurse |
| GET    | `/api/students/search`           | Tìm kiếm học sinh    | All                 |
| POST   | `/api/students/{id}/soft-delete` | Soft delete          | Admin,Manager,Nurse |

### 2. 🏥 Health Records Module

#### Chức năng

- Quản lý hồ sơ sức khỏe học sinh
- Ghi nhận khám sức khỏe định kỳ
- Lưu trữ thông tin y tế quan trọng
- Liên kết với y tá thực hiện

#### Luồng hoạt động

```
[Create Record] → [Validate Student exists] → [Auto set NurseId from JWT] → [Save with timestamp] → [Return Response]
[Get by Student] → [Filter by StudentId] → [Include Student & Nurse info] → [Return sorted by date]
```

#### API Endpoints

| Method | Endpoint                                 | Description        | Permission          |
| ------ | ---------------------------------------- | ------------------ | ------------------- |
| POST   | `/api/healthrecords`                     | Tạo hồ sơ mới      | Admin,Manager,Nurse |
| GET    | `/api/healthrecords`                     | Lấy tất cả hồ sơ   | All                 |
| GET    | `/api/healthrecords/{id}`                | Lấy hồ sơ theo ID  | All                 |
| PUT    | `/api/healthrecords/{id}`                | Cập nhật hồ sơ     | Admin,Manager,Nurse |
| DELETE | `/api/healthrecords/{id}`                | Xóa hồ sơ          | Admin,Manager       |
| GET    | `/api/healthrecords/student/{studentId}` | Hồ sơ của học sinh | All                 |
| GET    | `/api/healthrecords/search`              | Tìm kiếm hồ sơ     | All                 |

### 3. 💊 Medication Management Module

#### Chức năng

- Workflow yêu cầu thuốc từ phụ huynh
- Hệ thống phê duyệt từ y tá/quản lý
- Quản lý trạng thái đơn thuốc
- Theo dõi lịch sử sử dụng

#### Luồng hoạt động

```
[Parent Request] → [Status: Pending] → [Nurse Review] → [Approve/Reject] → [Update Status] → [Notify Parent]

Status Flow: Pending → Approved/Rejected
- Pending: Chờ duyệt
- Approved: Đã duyệt
- Rejected: Từ chối
```

#### API Endpoints

| Method | Endpoint                              | Description          | Permission          |
| ------ | ------------------------------------- | -------------------- | ------------------- |
| POST   | `/api/medication`                     | Yêu cầu thuốc mới    | Parent              |
| GET    | `/api/medication`                     | Lấy tất cả yêu cầu   | Admin,Manager,Nurse |
| GET    | `/api/medication/{id}`                | Lấy yêu cầu theo ID  | All                 |
| PUT    | `/api/medication/{id}`                | Cập nhật yêu cầu     | Admin,Manager,Nurse |
| DELETE | `/api/medication/{id}`                | Xóa yêu cầu          | Admin,Manager       |
| PUT    | `/api/medication/{id}/approve`        | Phê duyệt yêu cầu    | Admin,Manager,Nurse |
| PUT    | `/api/medication/{id}/reject`         | Từ chối yêu cầu      | Admin,Manager,Nurse |
| GET    | `/api/medication/student/{studentId}` | Yêu cầu của học sinh | All                 |
| GET    | `/api/medication/pending`             | Yêu cầu chờ duyệt    | Admin,Manager,Nurse |

### 4. 💉 Vaccination Management Module

#### Chức năng

- Quản lý lịch tiêm chủng toàn diện
- Theo dõi trạng thái tiêm chủng
- Quản lý multiple doses
- Batch tracking cho vaccine
- Phát hiện vaccination quá hạn

#### Luồng hoạt động

```
[Schedule] → [Status: Scheduled] → [Administer] → [Status: Completed] → [Set Next Dose if needed]
                ↓
          [Can Cancel/Postpone before completion]

Status Management:
- Scheduled: Đã lên lịch
- Completed: Đã hoàn thành
- Cancelled: Đã hủy
- Postponed: Hoãn lại
- InProgress: Đang thực hiện
```

#### Tính năng nâng cao

- **Dose Tracking**: Theo dõi liều thứ mấy (1st, 2nd, booster...)
- **Batch Number**: Truy xuất nguồn gốc vaccine
- **Side Effects**: Ghi nhận tác dụng phụ
- **Expiry Management**: Kiểm tra hạn sử dụng vaccine
- **Overdue Detection**: Tự động phát hiện vaccination quá hạn

#### API Endpoints

| Method | Endpoint                           | Description            | Permission          |
| ------ | ---------------------------------- | ---------------------- | ------------------- |
| POST   | `/api/vaccination`                 | Tạo lịch tiêm mới      | Admin,Manager,Nurse |
| GET    | `/api/vaccination`                 | Lấy tất cả vaccination | All                 |
| GET    | `/api/vaccination/{id}`            | Lấy theo ID            | All                 |
| PUT    | `/api/vaccination/{id}`            | Cập nhật thông tin     | Admin,Manager,Nurse |
| DELETE | `/api/vaccination/{id}`            | Xóa vaccination        | Admin,Manager,Nurse |
| GET    | `/api/vaccination/search`          | Tìm kiếm               | All                 |
| GET    | `/api/vaccination/check-existing`  | Kiểm tra đã tiêm       | Admin,Manager,Nurse |
| PUT    | `/api/vaccination/{id}/complete`   | Hoàn thành tiêm        | Admin,Manager,Nurse |
| PUT    | `/api/vaccination/{id}/cancel`     | Hủy lịch tiêm          | Admin,Manager,Nurse |
| PUT    | `/api/vaccination/{id}/postpone`   | Hoãn lịch tiêm         | Admin,Manager,Nurse |
| GET    | `/api/vaccination/status/{status}` | Lấy theo trạng thái    | All                 |
| GET    | `/api/vaccination/scheduled`       | Lịch đã book           | All                 |
| GET    | `/api/vaccination/overdue`         | Quá hạn tiêm           | Admin,Manager,Nurse |
| GET    | `/api/vaccination/upcoming`        | Sắp đến hạn            | All                 |
| GET    | `/api/vaccination/batch/{number}`  | Theo số lô vaccine     | Admin,Manager,Nurse |

### 5. 📦 Medicine Inventory Module

#### Chức năng

- Quản lý kho thuốc toàn diện
- Stock management (nhập/xuất kho)
- Expiry date tracking
- Low stock alerts
- Auto-merge duplicate medicines

#### Luồng hoạt động

```
[Add Medicine] → [Check if exists] → [Yes: Merge quantity] / [No: Create new] → [Update inventory]
[Stock Adjustment] → [Validate quantity ≥ 0] → [Update stock] → [Check low stock threshold]
[Expiry Check] → [Daily scan] → [Alert expired/expiring medicines] → [Generate reports]
```

#### Business Logic đặc biệt

- **Auto-merge**: Tự động gộp thuốc cùng tên
- **Expiry Priority**: Chọn hạn sử dụng muộn hơn khi merge
- **Stock Validation**: Không cho phép stock âm
- **Threshold Alerts**: Cảnh báo thuốc sắp hết

#### API Endpoints

| Method | Endpoint                           | Description            | Permission          |
| ------ | ---------------------------------- | ---------------------- | ------------------- |
| POST   | `/api/inventory`                   | Thêm thuốc vào kho     | Admin,Manager,Nurse |
| GET    | `/api/inventory`                   | Lấy tất cả thuốc       | All                 |
| GET    | `/api/inventory/{id}`              | Lấy thuốc theo ID      | All                 |
| PUT    | `/api/inventory/{id}`              | Cập nhật thông tin     | Admin,Manager,Nurse |
| DELETE | `/api/inventory/{id}`              | Xóa thuốc              | Admin,Manager       |
| GET    | `/api/inventory/search`            | Tìm kiếm thuốc         | All                 |
| POST   | `/api/inventory/{id}/adjust-stock` | Điều chỉnh số lượng    | Admin,Manager,Nurse |
| GET    | `/api/inventory/low-stock`         | Thuốc sắp hết          | Admin,Manager,Nurse |
| GET    | `/api/inventory/expired`           | Thuốc đã hết hạn       | Admin,Manager,Nurse |
| GET    | `/api/inventory/expiring-soon`     | Thuốc sắp hết hạn      | Admin,Manager,Nurse |
| GET    | `/api/inventory/check-medicine`    | Kiểm tra thuốc tồn tại | All                 |

## 🔐 Authentication & Authorization

### JWT Token System

```
[Login] → [Validate Credentials] → [Generate JWT] → [Include User Role] → [Return Token]
[API Call] → [Extract JWT] → [Validate Token] → [Check Role Permission] → [Execute/Deny]
```

### Token Structure

```json
{
  "sub": "user-id",
  "email": "user@example.com",
  "role": "Admin",
  "exp": "expiration-timestamp",
  "iss": "SchoolHealthSystem"
}
```

### Security Features

- **Password Hashing**: BCrypt với salt
- **Token Expiration**: Configurable expiry time
- **Role-based Access**: Fine-grained permissions
- **HTTPS Only**: Production security

## 📊 Database Schema

### Core Tables

```sql
Users (Id, Email, PasswordHash, FullName, Role, IsActive)
Students (Id, StudentCode, FullName, ParentId, DateOfBirth, IsActive)
HealthRecords (Id, StudentId, NurseId, Name, RecordDate)
MedicationRequests (Id, StudentId, ParentId, NurseId, Status, RequestDate)
VaccinationRecords (Id, StudentId, NurseId, VaccineName, Status, ScheduledDate, ActualDate, DoseNumber, BatchNumber)
MedicineInventory (Id, MedicineName, Quantity, ExpiryDate, CreatedAt)
```

### Relationships

```
Users 1→N Students (as Parent)
Users 1→N HealthRecords (as Nurse)
Users 1→N MedicationRequests (as Parent/Nurse)
Users 1→N VaccinationRecords (as Nurse)
Students 1→N HealthRecords
Students 1→N MedicationRequests
Students 1→N VaccinationRecords
```

## 🚀 Installation & Setup

### Prerequisites

```bash
- .NET 9.0 SDK
- SQL Server (LocalDB/Express/Full)
- Visual Studio 2022 / VS Code
```

### Installation Steps

```bash
# 1. Clone repository
git clone <repository-url>
cd SchoolHealthSystem

# 2. Restore packages
dotnet restore

# 3. Setup database connection
# Update appsettings.json with your SQL Server connection string

# 4. Run migrations
dotnet ef database update

# 5. Run application
dotnet run
```

### Default Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SchoolHealthSystemDb;Trusted_Connection=true"
  },
  "JwtSettings": {
    "Key": "YourSuperSecretKeyHere",
    "Issuer": "SchoolHealthSystem",
    "Audience": "SchoolHealthSystem",
    "ExpiryMinutes": 60
  }
}
```

## 📝 API Usage Examples

### Authentication

```bash
# Login
POST /api/auth/login
Content-Type: application/json
{
  "email": "admin@school.com",
  "password": "Admin@123"
}

# Response
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": "guid",
    "email": "admin@school.com",
    "fullName": "Administrator",
    "role": "Admin"
  }
}
```

### Student Management

```bash
# Create Student
POST /api/students
Authorization: Bearer <token>
Content-Type: application/json
{
  "fullName": "Nguyễn Văn A",
  "parentId": "parent-guid",
  "dateOfBirth": "2015-05-15T00:00:00Z",
  "address": "123 Main St",
  "phoneNumber": "0123456789"
}

# Search Students
GET /api/students/search?keyword=Nguyễn
Authorization: Bearer <token>
```

### Vaccination Management

```bash
# Schedule Vaccination
POST /api/vaccination
Authorization: Bearer <token>
Content-Type: application/json
{
  "studentId": "student-guid",
  "vaccineName": "COVID-19 Pfizer",
  "scheduledDate": "2024-01-15T09:00:00Z",
  "doseNumber": 1,
  "batchNumber": "PF001",
  "expirationDate": "2024-12-31T00:00:00Z"
}

# Complete Vaccination
PUT /api/vaccination/{id}/complete
Authorization: Bearer <token>
Content-Type: application/json
{
  "actualDate": "2024-01-15T10:30:00Z",
  "batchNumber": "PF001",
  "sideEffects": "None observed",
  "nextDoseDate": "2024-02-15T09:00:00Z"
}
```

### Inventory Management

```bash
# Add Medicine
POST /api/inventory
Authorization: Bearer <token>
Content-Type: application/json
{
  "medicineName": "Paracetamol 500mg",
  "quantity": 100,
  "expiryDate": "2025-12-31T00:00:00Z"
}

# Adjust Stock
POST /api/inventory/{id}/adjust-stock
Authorization: Bearer <token>
Content-Type: application/json
{
  "adjustmentQuantity": -10,
  "reason": "Dispensed to students"
}
```

## 🔧 Configuration

### Environment Variables

```bash
ASPNETCORE_ENVIRONMENT=Development
JWT_KEY=YourSecretKey
DB_CONNECTION=YourConnectionString
```

### Custom Settings

- **JWT Expiry**: Configurable token lifetime
- **Password Policy**: Minimum complexity requirements
- **File Upload**: Maximum size and allowed types
- **Pagination**: Default page sizes
- **Low Stock Threshold**: Inventory alert levels

## 📈 Monitoring & Logging

### Logging Levels

- **Information**: API requests, successful operations
- **Warning**: Validation failures, business rule violations
- **Error**: Exceptions, database errors
- **Critical**: System failures, security breaches

### Health Checks

```bash
GET /health - System health status
GET /health/db - Database connectivity
GET /health/ready - Application readiness
```

## 🧪 Testing

### Unit Tests

```bash
dotnet test --project Tests/SchoolHealthSystem.Tests
```

### API Testing via Swagger

```
Navigate to: https://localhost:5001/swagger
- Interactive API documentation
- Test endpoints directly
- View request/response schemas
```

### Test Data

Default seeded data includes:

- Admin user: `admin@school.com / Admin@123`
- Sample students, health records, and medications

## 🚀 Deployment

### Production Deployment

```bash
# Build for production
dotnet publish -c Release -o ./publish

# Database migration in production
dotnet ef database update --environment Production

# Run application
dotnet SchoolHealthSystem.dll
```

### Docker Support

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "SchoolHealthSystem.dll"]
```

## 📚 API Documentation

Complete API documentation is available at `/swagger` endpoint when running the application.

### Rate Limiting

- **Default**: 100 requests per minute per IP
- **Authenticated**: 1000 requests per minute per user

### Response Format

All API responses follow consistent format:

```json
{
  "success": true,
  "data": {...},
  "message": "Success message",
  "errors": null
}
```

## 🔍 Troubleshooting

### Common Issues

1. **Database Connection**: Check connection string in appsettings.json
2. **Migration Errors**: Ensure SQL Server is running
3. **JWT Issues**: Verify JWT key configuration
4. **Permission Denied**: Check user role assignments

### Support

For technical support or questions:

- Review API documentation at `/swagger`
- Check application logs in `Logs/` directory
- Verify database connectivity and permissions

---

**© 2025 SchoolHealthSystem - Comprehensive Student Health Management Solution**
