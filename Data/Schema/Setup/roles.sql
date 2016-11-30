-- Drop users
DROP USER IF EXISTS rpcwebapp;
DROP USER IF EXISTS rpcwebdbeventhandler;
DROP USER IF EXISTS rpcicontroller;
DROP USER IF EXISTS rpctestrunner;
DROP USER IF EXISTS rpcwebsocketservice;

-- For use by the web application
CREATE USER rpcwebapp 
    WITH PASSWORD 'rpcwebapp';
