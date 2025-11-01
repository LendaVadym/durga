-- Test Database Connection and Schema
-- ===================================
-- This script can be used to verify that the database schema is correctly created
-- and accessible. Run this with psql or any PostgreSQL client.

-- Check if uuid-ossp extension is enabled
SELECT * FROM pg_extension WHERE extname = 'uuid-ossp';

-- List all tables in the public schema
SELECT table_name, table_type 
FROM information_schema.tables 
WHERE table_schema = 'public'
ORDER BY table_name;

-- Check if Flyway schema history table exists
SELECT * FROM information_schema.tables 
WHERE table_schema = 'public' 
AND table_name = 'flyway_schema_history';

-- Count tables by category
SELECT 
    CASE 
        WHEN table_name LIKE 'AspNet%' THEN 'Identity Tables'
        WHEN table_name IN ('users', 'roles', 'user_roles', 'departments', 'teams', 'team_users') THEN 'Domain Tables'
        WHEN table_name = 'flyway_schema_history' THEN 'Migration Tables'
        ELSE 'Other Tables'
    END AS category,
    COUNT(*) as table_count
FROM information_schema.tables 
WHERE table_schema = 'public'
GROUP BY category
ORDER BY category;

-- Check indexes
SELECT 
    schemaname,
    tablename,
    indexname,
    indexdef
FROM pg_indexes
WHERE schemaname = 'public'
ORDER BY tablename, indexname;

-- Test UUID generation
SELECT uuid_generate_v4() as test_uuid;