DROP TABLE IF EXISTS todo_item;

CREATE TABLE todo_item
(
    key_uid             uuid NOT NULL,
    description         varchar(128) NOT NULL,
    is_complete         bool NOT NULL,
    time_created        timestamp with time zone NOT NULL
);

COMMENT ON TABLE todo_item IS 'Table of todo items';
COMMENT ON COLUMN todo_item.key_uid IS 'UUID of item';
COMMENT ON COLUMN todo_item.description IS 'description of item';
COMMENT ON COLUMN todo_item.is_complete IS 'Whether the item is complete';
COMMENT ON COLUMN todo_item.time_created IS 'When the item was created';

