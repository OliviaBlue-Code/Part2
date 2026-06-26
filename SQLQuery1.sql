CREATE DATABASE prog_tasks;
GO

USE [prog_tasks];
GO

CREATE TABLE demo_tasks (
    task_id          INT IDENTITY(1,1) PRIMARY KEY,
    task_name        VARCHAR(100) NOT NULL,
    task_description VARCHAR(100) NOT NULL,
    task_dueDate     VARCHAR(20),
    task_status      VARCHAR(20) DEFAULT 'pending'
);
GO

SELECT * FROM demo_tasks;
GO