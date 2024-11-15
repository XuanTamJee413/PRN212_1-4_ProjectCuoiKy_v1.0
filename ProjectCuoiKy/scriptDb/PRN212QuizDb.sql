-- Bảng Users lưu thông tin người dùng
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(10) CHECK (Role IN ('Student', 'Teacher', 'Admin')) DEFAULT 'Student',
    IsActive BIT DEFAULT 1 -- Trạng thái người dùng (1: active, 0: blocked)
);

-- Bảng Courses lưu danh sách các khóa học
CREATE TABLE Courses (
    CourseId INT PRIMARY KEY IDENTITY,
    CourseName NVARCHAR(100) NOT NULL,
    CreatorId INT NOT NULL, -- Người tạo (khóa ngoại tới Users)
    FOREIGN KEY (CreatorId) REFERENCES Users(UserId)
);

-- Bảng Terms lưu các thuật ngữ và định nghĩa
CREATE TABLE Terms (
    TermId INT PRIMARY KEY IDENTITY,
    CourseId INT NOT NULL, -- ID khóa học chứa thuật ngữ này (khóa ngoại tới Courses)
    TermText NVARCHAR(100) NOT NULL,
    CorrectAnswer NVARCHAR(255) NOT NULL,
    WrongAnswer1 NVARCHAR(255) NOT NULL,
    WrongAnswer2 NVARCHAR(255) NOT NULL,
    WrongAnswer3 NVARCHAR(255) NOT NULL,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);

-- Bảng Tests lưu thông tin bài kiểm tra
CREATE TABLE Tests (
    TestId INT PRIMARY KEY IDENTITY,
    CourseId INT NOT NULL, -- ID khóa học liên kết (khóa ngoại tới Courses)
    CreatorId INT NOT NULL, -- Người tạo bài kiểm tra (khóa ngoại tới Users)
    TimerEnabled BIT DEFAULT 0,
    TestKey NVARCHAR(50) NOT NULL UNIQUE,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId),
    FOREIGN KEY (CreatorId) REFERENCES Users(UserId)
);

-- Bảng TestResults lưu kết quả của sinh viên
CREATE TABLE TestResults (
    ResultId INT PRIMARY KEY IDENTITY,
    TestId INT NOT NULL, -- ID bài kiểm tra (khóa ngoại tới Tests)
    StudentId INT NOT NULL, -- ID sinh viên thực hiện kiểm tra (khóa ngoại tới Users)
    Score INT,
    CompletionTime DATETIME,
    FOREIGN KEY (TestId) REFERENCES Tests(TestId),
    FOREIGN KEY (StudentId) REFERENCES Users(UserId)
);

-- Bảng StudentAnswers lưu các câu trả lời của sinh viên
CREATE TABLE StudentAnswers (
    AnswerId INT PRIMARY KEY IDENTITY,
    TestId INT NOT NULL, -- ID bài kiểm tra
    StudentId INT NOT NULL, -- ID sinh viên
    TermId INT NOT NULL, -- ID thuật ngữ mà sinh viên trả lời
    ChosenAnswer NVARCHAR(255) NOT NULL, -- Câu trả lời mà sinh viên chọn
    IsCorrect BIT, -- Đúng hay sai (1: đúng, 0: sai)
    FOREIGN KEY (TestId) REFERENCES Tests(TestId),
    FOREIGN KEY (StudentId) REFERENCES Users(UserId),
    FOREIGN KEY (TermId) REFERENCES Terms(TermId)
);

-- Thêm dữ liệu người dùng
INSERT INTO Users (Username, Password, Role, IsActive) VALUES 
('student1', '123', 'Student', 1),
('student2', '123', 'Student', 1),
('student3', '123', 'Student', 0),
('teacher1', '123', 'Teacher', 1),
('teacher2', '123', 'Teacher', 1),
('admin1', '123', 'Admin', 1),
('admin2', '123', 'Admin', 1);

-- Thêm khóa học (Courses)
INSERT INTO Courses (CourseName, CreatorId) VALUES
(N'Tin học cơ bản', 1), 
(N'Lập trình nâng cao', 3), 
(N'Tiếng Anh giao tiếp', 1); 

-- Thêm thuật ngữ (Terms)
INSERT INTO Terms (CourseId, TermText, CorrectAnswer, WrongAnswer1, WrongAnswer2, WrongAnswer3) VALUES 
(1, N'Hệ điều hành', N'Phần mềm quản lý tài nguyên máy tính', N'Một loại phần cứng', N'Thành phần mạng', N'Thiết bị lưu trữ'),
(1, N'RAM', N'Bộ nhớ truy cập ngẫu nhiên', N'Đĩa cứng', N'CPU', N'Card đồ họa'),
(2, N'Polymorphism', N'Tính đa hình', N'Tính đóng gói', N'Tính kế thừa', N'Tính trừu tượng'),
(2, N'Encapsulation', N'Tính đóng gói', N'Tính kế thừa', N'Tính đa hình', N'Tính trừu tượng'),
(3, N'Hello', N'Xin chào', N'Tạm biệt', N'Thân ái', N'Cảm ơn'),
(3, N'Goodbye', N'Tạm biệt', N'Xin chào', N'Thân ái', N'Chúc mừng');

-- Thêm bài kiểm tra (Tests)
INSERT INTO Tests (CourseId, CreatorId, TimerEnabled, TestKey) VALUES
(1, 3, 0, 'TEST101'), -- Test cho Tin học cơ bản
(2, 3, 1, 'TEST202'), -- Test cho Lập trình nâng cao
(3, 1, 0, 'TEST303'); -- Test cho Tiếng Anh giao tiếp

-- Thêm kết quả kiểm tra (TestResults)
INSERT INTO TestResults (TestId, StudentId, Score, CompletionTime) VALUES
(1, 1, 80, '2024-11-15 10:30:00'),
(1, 2, 90, '2024-11-15 10:35:00'),
(2, 1, 70, '2024-11-15 11:00:00'),
(3, 2, 100, '2024-11-15 12:00:00');

-- Thêm câu trả lời của sinh viên (StudentAnswers)
INSERT INTO StudentAnswers (TestId, StudentId, TermId, ChosenAnswer, IsCorrect) VALUES
(1, 1, 1, N'Phần mềm quản lý tài nguyên máy tính', 1),
(1, 1, 2, N'Bộ nhớ truy cập ngẫu nhiên', 1),
(1, 2, 1, N'Một loại phần cứng', 0),
(1, 2, 2, N'Bộ nhớ truy cập ngẫu nhiên', 1),
(2, 1, 3, N'Tính đa hình', 1),
(2, 1, 4, N'Tính đóng gói', 0),
(3, 2, 5, N'Xin chào', 1),
(3, 2, 6, N'Tạm biệt', 0);


-- Thêm dữ liệu mới cho Users
INSERT INTO Users (Username, Password, Role, IsActive) VALUES 
('student4', '123', 'Student', 1),
('student5', '123', 'Student', 1),
('student6', '123', 'Student', 1),
('teacher3', '123', 'Teacher', 1),
('teacher4', '123', 'Teacher', 1),
('admin3', '123', 'Admin', 1),
('admin4', '123', 'Admin', 0);

-- Thêm khóa học mới cho Courses
INSERT INTO Courses (CourseName, CreatorId) VALUES
(N'Triết học hiện đại', 4), 
(N'Trí tuệ nhân tạo', 3), 
(N'Vật lý lượng tử', 5), 
(N'Lịch sử Việt Nam', 4); 

-- Thêm thuật ngữ mới cho Terms
INSERT INTO Terms (CourseId, TermText, CorrectAnswer, WrongAnswer1, WrongAnswer2, WrongAnswer3) VALUES 
(4, N'Chủ nghĩa Mác', N'Hệ tư tưởng của Karl Marx và Friedrich Engels', N'Tư tưởng của Albert Einstein', N'Lý thuyết kinh tế của Adam Smith', N'Lý thuyết quản lý của Max Weber'),
(4, N'Hiện sinh', N'Phong trào triết học tập trung vào tự do cá nhân', N'Học thuyết chính trị', N'Thuyết giáo dục', N'Lý thuyết tài chính'),
(5, N'Học sâu', N'Một nhánh của học máy', N'Một hệ điều hành', N'Một ngôn ngữ lập trình', N'Một phương pháp truyền thông'),
(5, N'Xử lý ngôn ngữ tự nhiên', N'Lĩnh vực trí tuệ nhân tạo về phân tích ngôn ngữ', N'Một kỹ thuật lập trình', N'Một công cụ chỉnh sửa văn bản', N'Một giao thức mạng'),
(6, N'Cơ học lượng tử', N'Lý thuyết vật lý về hành vi của các hạt nhỏ', N'Thiết bị đo lường', N'Nguyên tắc nhiệt động lực học', N'Lý thuyết hóa học hữu cơ'),
(6, N'Schrodinger', N'Nhà khoa học với thí nghiệm con mèo nổi tiếng', N'Người sáng lập thuyết tương đối', N'Nhà phát minh động cơ hơi nước', N'Nhà triết học Hy Lạp cổ đại'),
(7, N'Trần Hưng Đạo', N'Một vị tướng lừng danh trong lịch sử Việt Nam', N'Nhà thơ thời kỳ Pháp thuộc', N'Nhà phát minh', N'Nhà vật lý hiện đại'),
(7, N'Lê Lợi', N'Người sáng lập triều đại Hậu Lê', N'Vị vua thời Nguyễn', N'Nhà tư tưởng hiện đại', N'Thi sĩ thời Lý');

-- Thêm bài kiểm tra mới cho Tests
INSERT INTO Tests (CourseId, CreatorId, TimerEnabled, TestKey) VALUES
(4, 4, 1, 'TEST404'), -- Test cho Triết học hiện đại
(5, 3, 1, 'TEST505'), -- Test cho Trí tuệ nhân tạo
(6, 5, 0, 'TEST606'), -- Test cho Vật lý lượng tử
(7, 4, 0, 'TEST707'); -- Test cho Lịch sử Việt Nam

-- Thêm kết quả kiểm tra mới cho TestResults
INSERT INTO TestResults (TestId, StudentId, Score, CompletionTime) VALUES
(4, 4, 85, '2024-11-15 13:00:00'),
(4, 5, 90, '2024-11-15 13:15:00'),
(5, 6, 95, '2024-11-15 14:00:00'),
(5, 4, 70, '2024-11-15 14:30:00'),
(6, 5, 80, '2024-11-15 15:00:00'),
(6, 6, 85, '2024-11-15 15:30:00'),
(7, 4, 88, '2024-11-15 16:00:00'),
(7, 5, 92, '2024-11-15 16:15:00');

-- Thêm câu trả lời mới cho StudentAnswers
INSERT INTO StudentAnswers (TestId, StudentId, TermId, ChosenAnswer, IsCorrect) VALUES
(4, 4, 7, N'Hệ tư tưởng của Karl Marx và Friedrich Engels', 1),
(4, 4, 8, N'Phong trào triết học tập trung vào tự do cá nhân', 1),
(5, 6, 9, N'Một nhánh của học máy', 1),
(5, 6, 10, N'Lĩnh vực trí tuệ nhân tạo về phân tích ngôn ngữ', 1),
(6, 5, 11, N'Lý thuyết vật lý về hành vi của các hạt nhỏ', 1),
(6, 5, 12, N'Nhà khoa học với thí nghiệm con mèo nổi tiếng', 1),
(7, 4, 13, N'Một vị tướng lừng danh trong lịch sử Việt Nam', 1),
(7, 4, 14, N'Người sáng lập triều đại Hậu Lê', 1);
