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