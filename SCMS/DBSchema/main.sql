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



