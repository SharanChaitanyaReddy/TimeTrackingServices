-- 1. TEAMS TABLE (no dependencies)
CREATE TABLE teams (
    id UUID PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 2. USERS TABLE (references teams, foreign keys added later)
CREATE TABLE users (
    id UUID PRIMARY KEY,
    name VARCHAR(100),
    email VARCHAR(255) UNIQUE,
    role VARCHAR(50),
    team_id UUID,
    hourly_rate DECIMAL(10,2),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 3. PROJECTS TABLE (references users)
CREATE TABLE projects (
    id UUID PRIMARY KEY,
    name VARCHAR(100),
    description TEXT,
    start_date DATE,
    end_date DATE,
    created_by UUID,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 4. TASKS TABLE (references projects, users)
CREATE TABLE tasks (
    id UUID PRIMARY KEY,
    project_id UUID,
    name VARCHAR(100),
    description TEXT,
    assigned_to UUID,
    status VARCHAR(50),
    due_date DATE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 5. TAGS TABLE (no dependencies)
CREATE TABLE tags (
    id UUID PRIMARY KEY,
    name VARCHAR(50) UNIQUE NOT NULL
);

-- 6. TIME_ENTRIES TABLE (references users, tasks, projects)
CREATE TABLE time_entries (
    id UUID PRIMARY KEY,
    user_id UUID,
    task_id UUID,
    project_id UUID,
    start_time TIMESTAMP NOT NULL,
    end_time TIMESTAMP NOT NULL,
    is_manual BOOLEAN DEFAULT FALSE,
    description TEXT,
    is_approved BOOLEAN DEFAULT FALSE,
    submitted_at TIMESTAMP,
    approved_at TIMESTAMP,
    approved_by UUID,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 7. APPROVALS TABLE (references time_entries, users)
CREATE TABLE approvals (
    id UUID PRIMARY KEY,
    time_entry_id UUID,
    status VARCHAR(50),
    comment TEXT,
    approved_by UUID,
    approved_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 8. TIME_ENTRY_TAGS TABLE (junction table references time_entries, tags)
CREATE TABLE time_entry_tags (
    time_entry_id UUID,
    tag_id UUID,
    PRIMARY KEY (time_entry_id, tag_id)
);

-- 9. LEAVES TABLE (references users)
CREATE TABLE leaves (
    id UUID PRIMARY KEY,
    user_id UUID,
    leave_type VARCHAR(50),
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    status VARCHAR(50) DEFAULT 'pending',
    approved_by UUID,
    approved_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 10. HOLIDAYS TABLE (no dependencies)
CREATE TABLE holidays (
    id UUID PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    holiday_date DATE NOT NULL,
    description TEXT,
    region VARCHAR(100)
);

-- 11. PRODUCTIVITY_BENCHMARKS TABLE (references users)
CREATE TABLE productivity_benchmarks (
    id UUID PRIMARY KEY,
    user_id UUID,
    week_start DATE NOT NULL,
    expected_hours INT,
    actual_hours INT,
    status VARCHAR(50),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 12. BILLING_ENTRIES TABLE (references time_entries, users)
CREATE TABLE billing_entries (
    id UUID PRIMARY KEY,
    time_entry_id UUID,
    user_id UUID,
    hourly_rate DECIMAL(10,2),
    billable BOOLEAN DEFAULT TRUE,
    total_amount DECIMAL(10,2),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 13. AUDIT_LOGS TABLE (references users, optional)
CREATE TABLE audit_logs (
    id UUID PRIMARY KEY,
    user_id UUID,
    action VARCHAR(100),
    resource_type VARCHAR(50),
    resource_id UUID,
    details JSONB,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =======================
-- Add Foreign Key Constraints
-- =======================

ALTER TABLE users
ADD CONSTRAINT fk_users_team
FOREIGN KEY (team_id) REFERENCES teams(id);

ALTER TABLE projects
ADD CONSTRAINT fk_projects_created_by
FOREIGN KEY (created_by) REFERENCES users(id);

ALTER TABLE tasks
ADD CONSTRAINT fk_tasks_project
FOREIGN KEY (project_id) REFERENCES projects(id);

ALTER TABLE tasks
ADD CONSTRAINT fk_tasks_assigned_to
FOREIGN KEY (assigned_to) REFERENCES users(id);

ALTER TABLE time_entries
ADD CONSTRAINT fk_time_entries_user
FOREIGN KEY (user_id) REFERENCES users(id);

ALTER TABLE time_entries
ADD CONSTRAINT fk_time_entries_task
FOREIGN KEY (task_id) REFERENCES tasks(id);

ALTER TABLE time_entries
ADD CONSTRAINT fk_time_entries_project
FOREIGN KEY (project_id) REFERENCES projects(id);

ALTER TABLE time_entries
ADD CONSTRAINT fk_time_entries_approved_by
FOREIGN KEY (approved_by) REFERENCES users(id);

ALTER TABLE approvals
ADD CONSTRAINT fk_approvals_time_entry
FOREIGN KEY (time_entry_id) REFERENCES time_entries(id);

ALTER TABLE approvals
ADD CONSTRAINT fk_approvals_approved_by
FOREIGN KEY (approved_by) REFERENCES users(id);

ALTER TABLE time_entry_tags
ADD CONSTRAINT fk_time_entry_tags_entry
FOREIGN KEY (time_entry_id) REFERENCES time_entries(id);

ALTER TABLE time_entry_tags
ADD CONSTRAINT fk_time_entry_tags_tag
FOREIGN KEY (tag_id) REFERENCES tags(id);

ALTER TABLE leaves
ADD CONSTRAINT fk_leaves_user
FOREIGN KEY (user_id) REFERENCES users(id);

ALTER TABLE leaves
ADD CONSTRAINT fk_leaves_approved_by
FOREIGN KEY (approved_by) REFERENCES users(id);

ALTER TABLE productivity_benchmarks
ADD CONSTRAINT fk_productivity_user
FOREIGN KEY (user_id) REFERENCES users(id);

ALTER TABLE billing_entries
ADD CONSTRAINT fk_billing_time_entry
FOREIGN KEY (time_entry_id) REFERENCES time_entries(id);

ALTER TABLE billing_entries
ADD CONSTRAINT fk_billing_user
FOREIGN KEY (user_id) REFERENCES users(id);
