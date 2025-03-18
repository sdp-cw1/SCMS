CREATE TABLE IF NOT EXISTS `users` (
  `id` INTEGER PRIMARY KEY AUTO_INCREMENT,
  `username` VARCHAR(20) NOT NULL UNIQUE,
  `nic` VARCHAR(12) NOT NULL UNIQUE,
  `email` VARCHAR(50) NOT NULL UNIQUE,
  `phone` VARCHAR(12) NOT NULL UNIQUE,
  `password` VARCHAR(255) NOT NULL    
);

CREATE TABLE IF NOT EXISTS `admin` (
  `id` VARCHAR(6) PRIMARY KEY,
  `user_id` INTEGER NOT NULL UNIQUE,
  FOREIGN KEY (`user_id`) REFERENCES `users`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
);


-- Trigger for Admin Table automatic ID Generatation 

DELIMITER $$

CREATE TRIGGER after_insert_users
AFTER INSERT ON `users`
FOR EACH ROW
  BEGIN
    DECLARE next_id INT;

    -- Get the next auto-increment value for admin table ID
    SELECT COALESCE(MAX(CAST(SUBSTRING(id, 1) AS UNSIGNED)), 0) + 1 INTO next_id FROM `admin`;

    -- Insert into the 'admin' table using the newly inserted user's id
    INSERT INTO `admin` (`id`, `user_id`)
    VALUES (CONCAT('AD', LPAD(next_id, 4, '0')), NEW.id);
  END$$

DELIMITER ;


-- Stored Procedure for Inserting a new User and Admin 

DELIMITER $$

CREATE PROCEDURE InsertUserAndAdmin(
  IN p_username VARCHAR(20),
  IN p_nic VARCHAR(12),
  IN p_email VARCHAR(50),
  IN p_phone VARCHAR(12),
  IN p_password VARCHAR(255)
)
BEGIN
  DECLARE new_user_id INT;

  -- Insert into users table
  INSERT INTO users (username, nic, email, phone, password)
  VALUES (p_username, p_nic, p_email, p_phone, p_password);

  -- Get the last inserted ID
  SET new_user_id = LAST_INSERT_ID();

  -- Insert into admin table
  INSERT INTO admin (user_id)
  VALUES (new_user_id);
END$$

DELIMITER ;


CREATE TABLE IF NOT EXISTS `acc_staff` (
    `id` VARCHAR(6) PRIMARY KEY,
    `user_id` INTEGER NOT NULL UNIQUE,
    `dob` DATE NOT NULL ,    
    `first_name` VARCHAR(50) NOT NULL, 
    `last_name` VARCHAR(50) NOT NULL ,
    FOREIGN KEY (`user_id`) REFERENCES `users`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
);


-- Store Procedure for Inserting a new User and Account Staff

DELIMITER $$

CREATE PROCEDURE InsertUserAndAcc(
    IN p_username VARCHAR(20),
    IN p_nic VARCHAR(12),
    IN p_email VARCHAR(50),
    IN p_phone VARCHAR(12),
    IN p_password VARCHAR(255),
    IN p_dob DATE,
    IN p_firstname VARCHAR(50),
    IN p_lastname VARCHAR(50)
)
BEGIN
    DECLARE new_user_id INT;
    DECLARE next_id INT;
    DECLARE new_staff_id VARCHAR(6);

    -- Insert into users table
    INSERT INTO users (username, nic, email, phone, password)
    VALUES (p_username, p_nic, p_email, p_phone, p_password);

    -- Get the last inserted user ID
    SET new_user_id = LAST_INSERT_ID();

    -- Generate new acc_staff ID
    SELECT COALESCE(MAX(CAST(SUBSTRING(id, 3) AS UNSIGNED)), 0) + 1 
    INTO next_id 
    FROM acc_staff;

    SET new_staff_id = CONCAT('AS', LPAD(next_id, 4, '0'));

    -- Insert into acc_staff table with proper ID and details
    INSERT INTO acc_staff (id, user_id, dob, first_name, last_name)
    VALUES (new_staff_id, new_user_id, p_dob, p_firstname, p_lastname);
END$$

DELIMITER ;


CREATE TABLE IF NOT EXISTS `student` (
    `id` VARCHAR(6) PRIMARY KEY,
    `user_id` INTEGER NOT NULL UNIQUE,
    `dob` DATE NOT NULL ,    
    `first_name` VARCHAR(50) NOT NULL, 
    `last_name` VARCHAR(50) NOT NULL ,
    FOREIGN KEY (`user_id`) REFERENCES `users`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Store Procedure for Inserting a new User and Student


DELIMITER $$

CREATE PROCEDURE InsertUserAndStu(
    IN p_username VARCHAR(20),
    IN p_nic VARCHAR(12),
    IN p_email VARCHAR(50),
    IN p_phone VARCHAR(12),
    IN p_password VARCHAR(255),
    IN p_dob DATE,
    IN p_firstname VARCHAR(50),
    IN p_lastname VARCHAR(50)
)
BEGIN
    DECLARE new_user_id INT;
    DECLARE next_id INT;
    DECLARE new_student_id VARCHAR(6);

    -- Insert into users table
    INSERT INTO users (username, nic, email, phone, password)
    VALUES (p_username, p_nic, p_email, p_phone, p_password);

    -- Get the last inserted user ID
    SET new_user_id = LAST_INSERT_ID();

    -- Generate new student ID
    SELECT COALESCE(MAX(CAST(SUBSTRING(id, 3) AS UNSIGNED)), 0) + 1 
    INTO next_id 
    FROM student;

    SET new_student_id = CONCAT('ST', LPAD(next_id, 4, '0'));

    -- Insert into student table with proper ID and details
    INSERT INTO student (id, user_id, dob, first_name, last_name)
    VALUES (new_student_id, new_user_id, p_dob, p_firstname, p_lastname);
END$$

DELIMITER ;


-- Creating last all tables

CREATE TABLE IF NOT EXISTS modules (
    id VARCHAR(10) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    lecture integer,
    FOREIGN KEY (`lecture`) REFERENCES `users`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
    -- CONSTRAINT fk_modules_lecture FOREIGN KEY (lecture) REFERENCES users(id) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS courses (
    id VARCHAR(10) PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS course_modules (
    course_id VARCHAR(10),
    module_id VARCHAR(10),
    PRIMARY KEY (course_id, module_id),
    FOREIGN KEY (`course_id`) REFERENCES `courses`(`id`) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (`module_id`) REFERENCES `modules`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
    -- CONSTRAINT fk_course_modules_course FOREIGN KEY (course_id) REFERENCES courses(id) ON DELETE CASCADE,
    -- CONSTRAINT fk_course_modules_module FOREIGN KEY (module_id) REFERENCES modules(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS classes (
    id VARCHAR(10) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    seats INTEGER NOT NULL CHECK (seats > 0)
);

CREATE TABLE IF NOT EXISTS events (
    id VARCHAR(10) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    category VARCHAR(50) NOT NULL,
    organiser integer(10),
    FOREIGN KEY (`organiser`) REFERENCES `users`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
    -- CONSTRAINT fk_events_organiser FOREIGN KEY (organiser) REFERENCES users(id) ON DELETE SET NULL
);

CREATE TABLE IF NOT EXISTS schedules (
    id VARCHAR(10) PRIMARY KEY,
    event_id VARCHAR(10) NOT NULL,
    datetime DATETIME NOT NULL,
    location VARCHAR(255) NOT NULL,
    FOREIGN KEY (`event_id`) REFERENCES `events`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
    -- CONSTRAINT fk_schedules_event FOREIGN KEY (event_id) REFERENCES events(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS participants (
    schedule_id VARCHAR(10),
    user_id integer,
    PRIMARY KEY (schedule_id, user_id),
    FOREIGN KEY (`schedule_id`) REFERENCES `schedules`(`id`) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (`user_id`) REFERENCES `users`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
    -- CONSTRAINT fk_participants_schedule FOREIGN KEY (schedule_id) REFERENCES schedules(id) ON DELETE CASCADE,
    -- CONSTRAINT fk_participants_user FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

-- Adding a new column to the users table

ALTER TABLE users
ADD COLUMN AutoGenPassword VARCHAR(20);


-- Adding a new column to the student table
ALTER TABLE student
ADD COLUMN address VARCHAR(100) NOT NULL, 
ADD COLUMN notified_method VARCHAR(20) NOT NULL;