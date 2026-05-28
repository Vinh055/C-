using System;

namespace CoreBankingSystem
{
    // ==========================================
    // 1. LỚP BANK ACCOUNT 
    // ==========================================
    public class BankAccount
    {
        // CONST: Hằng số (Tên ngân hàng không bao giờ thay đổi)
        public const string BankName = "SmartEdu CoreBank";

        // STATIC FIELD: Biến tĩnh (Lãi suất chung cho mọi tài khoản)
        public static decimal InterestRate = 0.05m; // 5% / năm

        // Biến tĩnh private để đếm tổng số tài khoản đã được tạo ra
        private static int _totalAccountsCreated = 0;

        private decimal _balance;
        
        public string AccountNumber { get; private set; }
        public string OwnerName { get; set; }
        
        public decimal Balance
        {
            get { return _balance; }
        }
        
        public BankAccount (string accNum, string owner, decimal initialDeposit)
        {
            AccountNumber = accNum;
            OwnerName = owner;
            if (initialDeposit >= 0)
                _balance = initialDeposit;
            else
                _balance = 0; 

            // Tăng số lượng tài khoản toàn cục khi khởi tạo thành công
            _totalAccountsCreated++;
        }
        
        public void Deposit (decimal amount)
        {
            if (amount > 0) 
            {
                _balance += amount;
                Console.WriteLine($"[+] Đã nạp {amount:N0} VND. Số dư mới: {_balance:N0} VND");
            }
        }
        
        public bool Withdraw (decimal amount)
        {
            if (amount > 0 && amount <= _balance)
            {
                _balance -= amount;
                Console.WriteLine($"[-] Đã rút {amount:N0} VND. Số dư mới: {_balance:N0} VND");
                return true; 
            }
            Console.WriteLine($"Lỗi: Số dư không đủ hoặc số tiền rút không hợp lệ.");
            return false; 
        }
        
        // STATIC METHOD: Phương thức tĩnh để lấy dữ liệu tĩnh
        public static int GetTotalAccounts()
        {
            return _totalAccountsCreated;
        }

        public void PrintStatement()
        {
            Console.WriteLine($"--- Ngân hàng {BankName} ---");
            Console.WriteLine($"Tài khoản: {AccountNumber} | Chủ thẻ: {OwnerName} | Số dư: {Balance:N0} VND");
            Console.WriteLine($"Lãi suất hiện hành: {InterestRate * 100}%");
        }
    }

    // ==========================================
    // 2. LỚP CUSTOMER
    // ==========================================
    public class Customer
    {
        // 1. Hằng số quy định độ tuổi tối thiểu mở thẻ là 15 tuổi.
        public const int MinimumAge = 15;

        // 2. Biến tĩnh đếm tổng số khách hàng VIP toàn cục
        public static int TotalVipCustomers = 0;

        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public bool IsVip { get; set; }
        
        private int _birthYear;
        
        public int BirthYear
        {
            get { return _birthYear; }
            set
            {
                if (value >= 1900 && value <= DateTime.Now.Year)
                {
                    _birthYear = value;
                }
                else
                {
                    Console.WriteLine("Lỗi: Năm sinh không hợp lệ. Đã gán mặc định là 2000.");
                    _birthYear = 2000;
                }
            }
        }
        
        // Cập nhật Constructor để nhận diện khách hàng VIP và kiểm tra tuổi
        public Customer (string id, string name, int year, bool isVip = false)
        {
            CustomerId = id;
            FullName = name;
            BirthYear = year; 
            IsVip = isVip;

            // Kiểm tra điều kiện độ tuổi tối thiểu mở thẻ
            int currentAge = DateTime.Now.Year - BirthYear;
            if (currentAge < MinimumAge)
            {
                Console.WriteLine($"[CẢNH BÁO]: Khách hàng {FullName} chưa đủ {MinimumAge} tuổi để mở thẻ hợp lệ!");
            }

            // Nếu khách hàng là VIP, tăng biến đếm tổng cục lên 1
            if (IsVip)
            {
                TotalVipCustomers++;
            }
        }
        
        public void DisplayInfo()
        {
            int age = DateTime.Now.Year - BirthYear;
            Console.WriteLine($"Khách hàng: {FullName} | Mã KH: {CustomerId} | Tuổi: {age} | VIP: {(IsVip ? "Có" : "Không")}");
        }

        // 3. Phương thức tĩnh dùng để in ra tổng số VIP (Không dùng từ khóa 'this')
        public static void PrintVipStatistics()
        {
            Console.WriteLine($"[HỆ THỐNG]: Đang phục vụ {TotalVipCustomers} khách hàng VIP toàn hệ thống.");
        }
    }

    // ==========================================
    // 3. LỚP EMPLOYEE 
    // ==========================================
    public class Employee
    {
        public const string CompanyCode = "SMART";
        public const decimal TaxRate = 0.1m; 
        private static int _employeeCounter = 0; 

        public string EmployeeId { get; private set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }

        private decimal _baseSalary;

        public decimal BaseSalary
        {
            get { return _baseSalary; }
            set
            {
                if (value < 0)
                {
                    Console.WriteLine("Cảnh báo: Mức lương gán vào không được âm. Hệ thống tự động đặt về 0 VND.");
                    _baseSalary = 0;
                }
                else
                {
                    _baseSalary = value;
                }
            }
        }

        public decimal NetSalary
        {
            get { return _baseSalary * (1 - TaxRate); }
        }

        public Employee(string name, string position, decimal baseSalary)
        {
            EmployeeName = name;
            Position = position;
            BaseSalary = baseSalary;

            _employeeCounter++;
            EmployeeId = $"{CompanyCode}{_employeeCounter:D3}";
        }

        public void PrintEmployeeDetails()
        {
            Console.WriteLine($"Mã NV: {EmployeeId} | Tên: {EmployeeName} | Vị trí: {Position} | Lương thực lãnh: {NetSalary:N0} VND");
        }
    }

    // ==========================================
    // 4. CHƯƠNG TRÌNH CHÍNH 
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            // =========================================================
            // 1. KIỂM CHỨNG THÀNH VIÊN TĨNH BANKACCOUNT
            // =========================================================
            Console.WriteLine("=== 1. KIỂM TRA THÀNH VIÊN TĨNH (BANK ACCOUNT) ===");
            Console.WriteLine($"Chào mừng đến với {BankAccount.BankName}");
            Console.WriteLine($"Tổng số tài khoản ban đầu: {BankAccount.GetTotalAccounts()}\n");

            BankAccount acc1 = new BankAccount("001", "Nguyễn Văn A", 500000m);
            BankAccount acc2 = new BankAccount("002", "Trần Thị B", 1000000m);

            acc1.PrintStatement();
            acc2.PrintStatement();
            Console.WriteLine($"Tổng số tài khoản sau khi tạo: {BankAccount.GetTotalAccounts()}");

            // =========================================================
            // 2. KIỂM CHỨNG PHẦN THỰC HÀNH CÓ HƯỚNG DẪN (CUSTOMER)
            // =========================================================
            Console.WriteLine("\n=== 2. KIỂM TRA PHẦN THỰC HÀNH CÓ HƯỚNG DẪN (CUSTOMER) ===");
            Console.WriteLine($"Quy định độ tuổi tối thiểu mở tài khoản: {Customer.MinimumAge} tuổi.");
            
            // Gọi phương thức tĩnh khi chưa có khách hàng VIP nào
            Customer.PrintVipStatistics();

            // Tạo các khách hàng (Bao gồm cả khách hàng VIP và khách hàng chưa đủ tuổi)
            Console.WriteLine("\n-> Tiến hành khởi tạo danh sách khách hàng:");
            Customer cus1 = new Customer("CUS001", "Lê Thế Vinh", 2005, isVip: true);   // Hợp lệ & VIP
            Customer cus2 = new Customer("CUS002", "Lê Tương Lai", 2018, isVip: false); // Cảnh báo chưa đủ tuổi (8 tuổi)
            Customer cus3 = new Customer("CUS003", "Phan Hoàng Yến", 1998, isVip: true); // Hợp lệ & VIP

            Console.WriteLine("\n-> Thông tin chi tiết khách hàng:");
            cus1.DisplayInfo();
            cus2.DisplayInfo();
            cus3.DisplayInfo();

            // Gọi lại phương thức tĩnh để kiểm tra tổng số VIP (Kết quả kỳ vọng: 2)
            Console.WriteLine();
            Customer.PrintVipStatistics();

            // =========================================================
            // 3. KIỂM CHỨNG AUTO-INCREMENT ID (EMPLOYEE)
            // =========================================================
            Console.WriteLine("\n=== 3. KIỂM TRA TỰ ĐỘNG CẤP MÃ NỐI TIẾP (EMPLOYEE) ===");
            Employee emp1 = new Employee("Trần Văn Hùng", "Giao dịch viên", 8000000m);
            Employee emp2 = new Employee("Lê Thị Mai", "Kiểm soát viên", 15000000m);
            Employee emp3 = new Employee("Phạm Minh Tuấn", "Quản lý chi nhánh", 25000000m);

            emp1.PrintEmployeeDetails();
            emp2.PrintEmployeeDetails();
            emp3.PrintEmployeeDetails();
            
            Console.ReadLine();
        }
    }
}