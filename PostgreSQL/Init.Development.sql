SELECT 'CREATE DATABASE yap' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'yap')\gexec

DO $$ BEGIN
    IF NOT EXISTS (SELECT * FROM pg_user WHERE usename = 'users_service') THEN
CREATE ROLE users_service LOGIN SUPERUSER password 'HteVuDclI4pMTiUMRp8fQN3wXfqMRf';
END IF;
END $$;